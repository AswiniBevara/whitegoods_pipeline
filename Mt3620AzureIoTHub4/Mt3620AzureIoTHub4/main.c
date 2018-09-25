#include <errno.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <time.h>
#include <unistd.h>
// applibs_versions.h defines the API struct versions to use for applibs APIs.
#include "applibs_versions.h"

#include <applibs/gpio.h>
#include <applibs/log.h>
#include <applibs/wificonfig.h>

#include "mt3620_rdb.h"
#include "led_blink_utility.h"
#include "azure_c_shared_utility/threadapi.h"

#include "state.h"

// This sample C application for a MT3620 Reference Development Board (Azure Sphere) demonstrates how to
// connect an Azure Sphere device to an Azure IoT Hub. To use this sample, you must first
// add the Azure IoT Hub Connected Service reference to the project (right-click
// References -> Add Connected Service -> Azure IoT Hub), which populates this project
// with additional sample code used to communicate with an Azure IoT Hub.
//
// The sample leverages these functionalities of the Azure IoT SDK C:
// - Device to cloud messages;
// - Cloud to device messages;
// - Direct Method invocation;
// - Device Twin management;
//
// A description of the sample follows:
// - LED 1 blinks constantly.
// - Pressing button A toggles the rate at which LED 1 blinks
//   between three values.
// - Pressing button B triggers the sending of a message to the IoT Hub.
// - LED 2 flashes red when button B is pressed (and a
//   message is sent) and flashes yellow when a message is received.
// - LED 3 indicates whether network connection to the Azure IoT Hub has been
//   established.
//
// Direct Method related notes:
// - Invoking the method named "LedColorControlMethod" with a payload containing '{"color":"red"}'
//   will set the color of LED 1 to red;
//
// Device Twin related notes:
// - Setting LedBlinkRateProperty in the Device Twin to a value from 0 to 2 causes the sample to
//   update the blink rate of LED 1 accordingly, e.g
//   '{"LedBlinkRateProperty": "2"}';
// - Upon receipt of the LedBlinkRateProperty desired value from the IoT hub, the sample updates
//   the device twin on the IoT hub with the new value for LedBlinkRateProperty.
// - Pressing button A causes the sample to report the blink rate to the device
//   twin on the IoT Hub.

// This sample uses the API for the following Azure Sphere application libraries:
// - gpio (digital input for button)
// - log (messages shown in Visual Studio's Device Output window during debugging)
// - wificonfig (configure WiFi settings)



#include "azure_iot_hub.h"

// An array defining the RGB GPIOs for each LED on the device
static const GPIO_Id ledsPins[3][3] = {
    {MT3620_RDB_LED1_RED, MT3620_RDB_LED1_GREEN, MT3620_RDB_LED1_BLUE}, {MT3620_RDB_LED2_RED, MT3620_RDB_LED2_GREEN, MT3620_RDB_LED2_BLUE}, {MT3620_RDB_LED3_RED, MT3620_RDB_LED3_GREEN, MT3620_RDB_LED3_BLUE}};

static size_t blinkIntervalIndex = 0;
static LedBlinkUtility_Colors ledBlinkColor = LedBlinkUtility_Colors_Blue;

static const struct timespec blinkIntervals[] = {{0, 125000000}, {0, 250000000}, {0, 500000000}};
static const size_t blinkIntervalsSize = sizeof(blinkIntervals) / sizeof(*blinkIntervals);

// Button GPIO file descriptors - initialized to invalid value
static int blinkRateButtonFd = -1;
static int messageSendButtonFd = -1;

// Button state
static GPIO_Value_Type blinkRateButtonState = GPIO_Value_High;
static GPIO_Value_Type messageSendButtonState = GPIO_Value_High;

// LED state
static RgbLed ledBlink;
static RgbLed ledMessageEventSentReceived;
static RgbLed ledNetworkStatus;
static RgbLed *rgbLeds[] = {&ledBlink, &ledMessageEventSentReceived, &ledNetworkStatus};
static const size_t rgbLedsCount = sizeof(rgbLeds) / sizeof(*rgbLeds);

// Connectivity state
static bool connectedToIoTHub = false;

// Termination state
static bool terminationRequested = false;

/// <summary>
///     Signal handler for termination requests. This handler must be async-signal-safe.
/// </summary>
static void TerminationHandler(int signalNumber)
{
    // Don't use Log_Debug here, as it is not guaranteed to be async signal safe
    terminationRequested = true;
}

/// <summary>
///     Store a sample WiFi network.
/// </summary>
static void AddSampleWiFiNetwork(void)
{
    int result = 0;
    const char *wifiSsid = "A_WiFi_SSID";
    result = WifiConfig_StoreOpenNetwork((uint8_t *)wifiSsid, strlen(wifiSsid));
    // To add a WPA2 network instead of an open network
    // comment the line above and uncomment the lines below
    // const char *wifiPsk = "SomePassword";
    // result = WifiConfig_StoreWpa2Network((uint8_t*)wifiSsid, strlen(wifiSsid),
    //                                      wifiPsk, strlen(wifiPsk));
    if (result < 0) {
        if (errno == EEXIST) {
            Log_Debug("The \"%s\" WiFi network is already stored on the device.\n", wifiSsid);
        } else {
            Log_Debug(
                "ERROR: WifiConfig_StoreOpenNetwork failed to store WiFi network \"%s\" with "
                "result %d. Errno: %s (%d).\n",
                wifiSsid, result, strerror(errno), errno);
        }
    } else {
        Log_Debug("Successfully stored WiFi network: \"%s\".\n", wifiSsid);
    }
}

/// <summary>
///     List all stored WiFi networks.
/// </summary>
static void DebugPrintStoredWiFiNetworks(void)
{
    int result = WifiConfig_GetStoredNetworkCount();
    if (result < 0) {
        Log_Debug(
            "ERROR: WifiConfig_GetStoredNetworkCount failed to get stored network count with "
            "result %d. Errno: %s (%d).\n",
            result, strerror(errno), errno);
    } else if (result == 0) {
        Log_Debug("There are no stored WiFi networks.\n");
    } else {
        Log_Debug("There are %d stored WiFi networks:\n", result);
        size_t networkCount = (size_t)result;
        WifiConfig_StoredNetwork *networks =
            (WifiConfig_StoredNetwork *)malloc(sizeof(WifiConfig_StoredNetwork) * networkCount);
        int result = WifiConfig_GetStoredNetworks(networks, networkCount);
        if (result < 0) {
            Log_Debug(
                "ERROR: WifiConfig_GetStoredNetworks failed to get stored networks with "
                "result %d. Errno: %s (%d).\n",
                result, strerror(errno), errno);
        } else {
            networkCount = (size_t)result;
            for (size_t i = 0; i < networkCount; ++i) {
                Log_Debug("%3d: SSID \"%.*s\"\n", i, networks[i].ssidLength, networks[i].ssid);
            }
        }
        free(networks);
    }
}

/// <summary>
///     Show details of the currently connected WiFi network.
/// </summary>
static void DebugPrintCurrentlyConnectedWiFiNetwork(void)
{
    WifiConfig_ConnectedNetwork network;
    int result = WifiConfig_GetCurrentNetwork(&network);
    if (result < 0) {
        Log_Debug("Not currently connected to a WiFi network.\n");
    } else {
        Log_Debug("Currently connected WiFi network: ");
        Log_Debug("SSID \"%.*s\", BSSID %02x:%02x:%02x:%02x:%02x:%02x, Frequency %dMHz\n",
                  network.ssidLength, network.ssid, network.bssid[0], network.bssid[1],
                  network.bssid[2], network.bssid[3], network.bssid[4], network.bssid[5],
                  network.frequencyMHz);
    }
}

/// <summary>
///     Trigger a WiFi scan and list found WiFi networks.
/// </summary>
static void DebugPrintScanFoundNetworks(void)
{
    int result = WifiConfig_TriggerScanAndGetScannedNetworkCount();
    if (result < 0) {
        Log_Debug(
            "ERROR: WifiConfig_TriggerScanAndGetScannedNetworkCount failed to get scanned "
            "network count with result %d. Errno: %s (%d).\n",
            result, strerror(errno), errno);
    } else if (result == 0) {
        Log_Debug("Scan found no WiFi network.\n");
    } else {
        size_t networkCount = (size_t)result;
        Log_Debug("Scan found %d WiFi networks:\n", result);
        WifiConfig_ScannedNetwork *networks =
            (WifiConfig_ScannedNetwork *)malloc(sizeof(WifiConfig_ScannedNetwork) * networkCount);
        result = WifiConfig_GetScannedNetworks(networks, networkCount);
        if (result < 0) {
            Log_Debug(
                "ERROR: WifiConfig_GetScannedNetworks failed to get scanned networks with "
                "result %d. Errno: %s (%d).\n",
                result, strerror(errno), errno);
        } else {
            // Log SSID, signal strength and frequency of the found WiFi networks
            networkCount = (size_t)result;
            for (size_t i = 0; i < networkCount; ++i) {
                Log_Debug("%3d: SSID \"%.*s\", Signal Level %d, Frequency %dMHz\n", i,
                          networks[i].ssidLength, networks[i].ssid, networks[i].signalRssi,
                          networks[i].frequencyMHz);
            }
        }
        free(networks);
    }
}

/// <summary>
///     Helper function to open a file descriptor for the given GPIO as an input.
/// </summary>
/// <param name="button">The GPIO to which the button is attached.</param>
/// <param name="outGpioButtonFd">File descriptor of the opened GPIO.</param>
/// <returns>True if successful, false if an error occurred.</return>
static bool OpenButton(GPIO_Id button, int *outGpioButtonFd)
{
    *outGpioButtonFd = GPIO_OpenAsInput(button);
    if (*outGpioButtonFd < 0) {
        Log_Debug("ERROR: Could not open button GPIO\n");
        return false;
    }

    return true;
}

/// <summary>
///     Toggles the blink speed of the blink LED between 3 values, and updates the device twin.
/// </summary>
static void ToggleBlinkSpeed()
{
    blinkIntervalIndex = (blinkIntervalIndex + 1) % (blinkIntervalsSize);
    LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(
        &ledBlink, blinkIntervals[blinkIntervalIndex], ledBlinkColor);

    if (connectedToIoTHub) {
        // Report the current state to the Device Twin on the IoT Hub
        char blinkIntervalIndexString[128];
        snprintf(blinkIntervalIndexString, sizeof(blinkIntervalIndexString), "\"%d\"",
                 blinkIntervalIndex);
        AzureIoT_TwinReportState("LedBlinkRateProperty", blinkIntervalIndexString);
    } else {
        Log_Debug("WARNING: Cannot send reported property; not connected to the IoT Hub\n");
    }
}

/// <summary>
///     Sends a message to the IoT Hub.
/// </summary>
static void SendMessageToIotHub()
{
    if (connectedToIoTHub) {
		const unsigned char *message = "{\"deviceId\":\"mt3620dj\",\"messageId\":\"3\",\"text\":\" Hello from new device\"}";
        // Send a message
        //AzureIoT_SendMessage("Hello from Azure IoT sample!");

        // Set the send/receive LED to blink once immediately to indicate the message has been
        // queued
        LedBlinkUtility_BlinkNow(&ledMessageEventSentReceived, LedBlinkUtility_Colors_Red);
    } else {
        Log_Debug("WARNING: Cannot send message: not connected to the IoT Hub\n");
    }
}

/// <summary>
///    Check for button presses and respond if one is detected
/// </summary>
/// <returns>0 if the check was successful, or -1 in the case of a failure</returns>
static int CheckForButtonPresses()
{
    GPIO_Value_Type newGpioButtonState;

    // Check for a button press on blinkRateButtonFd
    int result = GPIO_GetValue(blinkRateButtonFd, &newGpioButtonState);
    if (result != 0) {
        Log_Debug("ERROR: Could not read button GPIO\n");
        return -1;
    }

    if (newGpioButtonState != blinkRateButtonState) {
        // If the button state has changed, then respond
        // The button has GPIO_Value_Low when pressed and GPIO_Value_High when released
        if (newGpioButtonState == GPIO_Value_Low) {
            ToggleBlinkSpeed();
        }
        // Save the button's new state
        blinkRateButtonState = newGpioButtonState;
    }

    // Check for a button press on messageSendButtonFd
    result = GPIO_GetValue(messageSendButtonFd, &newGpioButtonState);
    if (result != 0) {
        Log_Debug("ERROR: Could not read button GPIO\n");
        return -1;
    }

    if (newGpioButtonState != messageSendButtonState) {
        // If the button state has changed, then respond
        // The button has GPIO_Value_Low when pressed and GPIO_Value_High when released
        if (newGpioButtonState == GPIO_Value_Low) {
            SendMessageToIotHub();
        }
        // Save the button's new state
        messageSendButtonState = newGpioButtonState;
    }

    return 0;
}

/// <summary>
///     MessageReceived callback function, called when a message is received from the Azure IoT Hub.
/// </summary>
/// <param name="payload">The payload of the received message.</param>
static void MessageReceived(const char *payload)
{
    // Set the send/receive LED to blink once immediately to indicate a message has been received
    LedBlinkUtility_BlinkNow(&ledMessageEventSentReceived, LedBlinkUtility_Colors_Yellow);
}

/// <summary>
///     Device Twin update callback function, called when an update is received from the Azure IoT
///     Hub.
/// </summary>
/// <param name="treeRoot">Handle to the Device Twin properties JSON tree received from the Azure
/// IoT Hub.</param>

void updateBlinkRate(const char *propertyName, const char *valueString) {
	if (strcmp(propertyName, "BlinkRate1") == 0) {
		if (strcmp(valueString, "true") == 0) {
			blinkIntervalIndex = 3;
			LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(&ledBlink, blinkIntervals[blinkIntervalIndex], ledBlinkColor);
		}
		else {
			LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(&ledBlink, blinkIntervals[blinkIntervalIndex], LedBlinkUtility_Colors_Off);
		}
	}
	if (strcmp(propertyName, "BlinkRate2") == 0) {
		if (strcmp(valueString, "true") == 0) {
			blinkIntervalIndex = 2;
			LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(&ledMessageEventSentReceived, blinkIntervals[blinkIntervalIndex], ledBlinkColor);
		}
		else {
			LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(&ledMessageEventSentReceived, blinkIntervals[blinkIntervalIndex], LedBlinkUtility_Colors_Off);
		}
	}
	if (strcmp(propertyName, "BlinkRate3") == 0) {
		if (strcmp(valueString, "true") == 0) {
			blinkIntervalIndex = 1;
			LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(&ledNetworkStatus, blinkIntervals[blinkIntervalIndex], ledBlinkColor);
		}
		else {
			LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(&ledNetworkStatus, blinkIntervals[blinkIntervalIndex], LedBlinkUtility_Colors_Off);
		}
	}
}

static void DeviceTwinUpdate(const char *childName, const char *propertyName, const char *valueString)
{
	if (strcmp(childName, "features") == 0) {
		for (int i = 0; i < 3; i++) {
			if (strcmp(propertyName, FEATURES[i].name) == 0) {
				updateBlinkRate(propertyName,valueString);
				AzureIoT_TwinReportStateFeature(FEATURES[i].name, "isActivated", valueString);
			}
		}
	}
}
//static void DeviceTwinUpdate(MULTITREE_HANDLE treeRoot)
//{
//    const char *valueString = NULL;
//	const void *value = NULL;
//	if (MULTITREE_OK == MultiTree_GetLeafValue(treeRoot, "SwitchOn", &value)) {
//		blinkIntervalIndex = 0;
//		LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(
//			&ledBlink, blinkIntervals[blinkIntervalIndex], ledBlinkColor);
//		Log_Debug("hello from devicetwinupdate");
//	}
//    // Get the value of the LedBlinkRateProperty and print it.
//    if (MULTITREE_OK ==
//        MultiTree_GetLeafValue(treeRoot, "LedBlinkRateProperty", (const void **)&valueString)) {
//        Log_Debug("Property LedBlinkRateProperty changed, new value is %s\n", valueString);
//        if (valueString[0] ==
//            '\"') // If the value is a string with double quote, just skip the first double quote
//            valueString += 1;
//        unsigned long newValue = strtoul(valueString, NULL, 10);
//
//        blinkIntervalIndex =
//            newValue % blinkIntervalsSize; // Clamp value to [0..blinkIntervalsSize)
//
//        LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(
//            &ledBlink, blinkIntervals[blinkIntervalIndex], ledBlinkColor);
//
//        if (connectedToIoTHub) {
//            // Report the new state back to the Device Twin on the IoT Hub
//            char blinkIntervalIndexString[128];
//            snprintf(blinkIntervalIndexString, sizeof(blinkIntervalIndexString), "\"%d\"",
//                     blinkIntervalIndex);
//            AzureIoT_TwinReportState("LedBlinkRateProperty", blinkIntervalIndexString);
//        }
//    }
//}



static void *SetupHeapMessage(const char *messageFormat, size_t maxLength, ...)
{
    va_list args;
    va_start(args, maxLength);
    char *message =
        malloc(maxLength + 1); // Ensure there is space for the null terminator put by vsnprintf.
    if (message != NULL) {
        vsnprintf(message, maxLength, messageFormat, args);
    }
    va_end(args);
    return message;
}

/// <summary>
///     Direct Method callback function, called when a Direct Method call is received from the Azure
///     IoT Hub.
/// </summary>
/// <param name="methodName">The name of the method being called.</param>
/// <param name="payload">The payload of the method.</param>
/// <param name="responsePayload">The response payload content. This must be a heap-allocated
/// string, 'free' will be called on this buffer by the Azure IoT Hub SDK.</param>
/// <param name="responsePayloadSize">The size of the response payload content.</param>
/// <returns>200 HTTP status code if the method name is "LedColorControlMethod" and the color is
/// correctly parsed;
/// 400 HTTP status code is the color has not been recognised in the payload;
/// 404 HTTP status code if the method name is unknown.</returns>
static int DirectMethodCall(const char *methodName, const char *payload, size_t payloadSize,
                            char **responsePayload, size_t *responsePayloadSize)
{
    // Prepare the payload for the response. This is a heap allocated null terminated string.
    // The Azure IoT Hub SDK is responsible of freeing it.
    *responsePayload = NULL;  // Reponse payload content.
    *responsePayloadSize = 0; // Response payload content size.

    int result = 404; // HTTP status code

    if (strcmp(methodName, "LedColorControlMethod") == 0) {
        LedBlinkUtility_Colors ledColor = LedBlinkUtility_Colors_Unknown;
        // The payload should contains a json such as: { "color": "red"}
        MULTITREE_HANDLE jsonRoot = NULL;
        const char *colorValue;
        char *directMethodCallContent =
            malloc(payloadSize + 1); // +1 to store null char at the end.
        if (directMethodCallContent == NULL) {
            Log_Debug("ERROR: Could not allocate buffer for direct method request payload.\n");
            abort();
        }
        memcpy(directMethodCallContent, payload, payloadSize);
        directMethodCallContent[payloadSize] = 0; // Null terminated string.
        if (JSON_DECODER_OK == JSONDecoder_JSON_To_MultiTree(directMethodCallContent, &jsonRoot)) {
            MULTITREE_HANDLE colorAttribute = NULL;
            if (MULTITREE_OK == MultiTree_GetChildByName(jsonRoot, "color", &colorAttribute)) {
                if (MULTITREE_OK == MultiTree_GetValue(colorAttribute, (const void **)&colorValue))
                    ledColor = LedBlinkUtility_GetColorFromString(
                        colorValue + 1, payloadSize); // +1 to skip the double quotes.
            }
        }

        // If color's name has not been identified.
        if (ledColor == LedBlinkUtility_Colors_Unknown) {
            result = 400;
            Log_Debug("INFO: Unrecognised payload format.\n");

            static const char noColorResponse[] =
                "{ \"success\" : false, \"message\" : \"request does not contain an identifiable "
                "color\" }";
            size_t responseMaxLength = sizeof(noColorResponse);
            *responsePayload = SetupHeapMessage(noColorResponse, responseMaxLength);
            if (*responsePayload == NULL) {
                Log_Debug("ERROR: Could not allocate buffer for direct method response payload.\n");
                abort();
            }
            *responsePayloadSize = strlen(*responsePayload);
        } else {
            // Color's name has been identified.
            result = 200;
            const char *colorString = LedBlinkUtility_GetStringFromColor(ledColor);
            Log_Debug("INFO: Color set to: '%s'.\n", colorString);
            // Set the blinking LED color.
            ledBlinkColor = ledColor;
            LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(
                &ledBlink, blinkIntervals[blinkIntervalIndex], ledColor);

            static const char colorOkResponse[] =
                "{ \"success\" : true, \"message\" : \"led color set to %s\" }";
            size_t responseMaxLength = sizeof(colorOkResponse) + strlen(payload);
            *responsePayload = SetupHeapMessage(colorOkResponse, responseMaxLength, colorString);
            if (*responsePayload == NULL) {
                Log_Debug("ERROR: Could not allocate buffer for direct method response payload.\n");
                abort();
            }
            *responsePayloadSize = strlen(*responsePayload);
        }
    } else {
        result = 404;
        Log_Debug("INFO: Method not found called: '%s'.\n", methodName);

        static const char noMethodFound[] = "\"method not found '%s'\"";
        size_t responseMaxLength = sizeof(noMethodFound) + strlen(methodName);
        *responsePayload = SetupHeapMessage(noMethodFound, responseMaxLength, methodName);
        if (*responsePayload == NULL) {
            Log_Debug("ERROR: Could not allocate buffer for direct method response payload.\n");
            abort();
        }
        *responsePayloadSize = strlen(*responsePayload);
    }

    return result;
}

/// <summary>
///     IoT Hub connection status callback function.
/// </summary>
/// <param name="connected">'true' when the connection to the IoT Hub is established.</param>
static void IoTHubConnectionStatusChanged(bool connected)
{
    connectedToIoTHub = connected;
}

/// <summary>
///     Initialize peripherals, termination handler, and Azure IoT
/// </summary>
/// <returns>0 on success, or -1 on failure</returns>
static int Init(void)
{
    // Register a SIGTERM handler for termination requests
    struct sigaction action;
    memset(&action, 0, sizeof(struct sigaction));
    action.sa_handler = TerminationHandler;
    sigaction(SIGTERM, &action, NULL);

    // Open button A
    Log_Debug("Opening MT3620_RDB_BUTTON_A\n");
    if (!OpenButton(MT3620_RDB_BUTTON_A, &blinkRateButtonFd)) {
        return -1;
    }

    // Open button B
    Log_Debug("Open MT3620_RDB_BUTTON_B\n");
    if (!OpenButton(MT3620_RDB_BUTTON_B, &messageSendButtonFd)) {
        return -1;
    }

    // Open file descriptors for the RGB LEDs and store them in the rgbLeds array (and in turn in
    // the ledBlink, ledMessageEventSentReceived, ledNetworkStatus variables)
    LedBlinkUtility_OpenLeds(rgbLeds, rgbLedsCount, ledsPins);

    // Set ledBlink to blink blue, with rate specified in the configuration device storage
    LedBlinkUtility_SetBlinkingLedHandleAndPeriodAndColor(
        &ledBlink, blinkIntervals[blinkIntervalIndex], ledBlinkColor);

    // Initialize the Azure IoT SDK
    if (!AzureIoT_Initialize()) {
        Log_Debug("ERROR: Cannot initialize Azure IoT Hub SDK.\n");
        return -1;
    }

    // Set the Azure IoT hub related callbacks
    AzureIoT_SetMessageReceivedCallback(&MessageReceived);
    AzureIoT_SetDeviceTwinUpdateCallback(&DeviceTwinUpdate);
    AzureIoT_SetDirectMethodCallback(&DirectMethodCall);
    AzureIoT_SetConnectionStatusCallback(&IoTHubConnectionStatusChanged);

    // Perform WiFi network setup and debug printing
    AddSampleWiFiNetwork();
    DebugPrintStoredWiFiNetworks();
    DebugPrintCurrentlyConnectedWiFiNetwork();
    DebugPrintScanFoundNetworks();

    return 0;
}

/// <summary>
///     Close peripherals and Azure IoT
/// </summary>
static void ClosePeripherals(void)
{
    Log_Debug("Closing GPIOs and Azure IoT\n");

    // Close the button file descriptors
    if (blinkRateButtonFd >= 0) {
        int result = close(blinkRateButtonFd);
        if (result != 0) {
            Log_Debug("WARNING: Problem occurred closing blinkRateButton GPIO: %s (%d).\n",
                      strerror(errno), errno);
        }
    }

    if (messageSendButtonFd >= 0) {
        int result = close(messageSendButtonFd);
        if (result != 0) {
            Log_Debug("WARNING: Problem occurred closing messageSendButton GPIO: %s (%d).\n",
                      strerror(errno), errno);
        }
    }

    // Close the LEDs and leave then off
    LedBlinkUtility_CloseLeds(rgbLeds, rgbLedsCount);

    // Destroy the IoT Hub client
    AzureIoT_DestroyClient();
    AzureIoT_Deinitialize();
}

/// <summary>
///     Main entry point for this application.
/// </summary>
int main(int argc, char *argv[])
{
	bool initializedReport = false;
    Log_Debug("Azure IoT application starting\n");

    int initResult = Init();
    if (initResult != 0) {
        terminationRequested = true;
    }

    const struct timespec timespec_1ms = {0, 1000000};
	// Set network status LED color
	LedBlinkUtility_Colors color =
		(connectedToIoTHub ? LedBlinkUtility_Colors_Green : LedBlinkUtility_Colors_Off);
    // Main loop
    while (!terminationRequested) {
      
        /*if (LedBlinkUtility_SetLed(&ledNetworkStatus, color) != 0) {
            Log_Debug("Error: Set color for network status LED failed\n");
            break;
        }*/

        // Trigger LEDs to blink as appropriate
        if (LedBlinkUtility_BlinkLeds(rgbLeds, rgbLedsCount) != 0) {
            Log_Debug("ERROR: Blinking LEDs failed\n");
            break;
        }

        // Setup the IoT Hub client.
        // Notes:
        // - it is safe to call this function even if the client has already been set up, as in
        //   this case it would have no effect;
        // - a failure to setup the client is a fatal error.
        if (!AzureIoT_SetupClient()) {
            Log_Debug("ERROR: Failed to set up IoT Hub client\n");
            break;
        }

        // AzureIoT_DoPeriodicTasks() needs to be called frequently in order to keep active
        // the flow of data with the Azure IoT Hub
		if (!initializedReport) {
			AzureIoT_InitReportState();
			initializedReport = true;
		}
		
        AzureIoT_DoPeriodicTasks();

        if (CheckForButtonPresses() != 0) {
            break;
        }

        nanosleep(&timespec_1ms, NULL);
    }

    ClosePeripherals();
    Log_Debug("Application exiting\n");
    return 0;
}
