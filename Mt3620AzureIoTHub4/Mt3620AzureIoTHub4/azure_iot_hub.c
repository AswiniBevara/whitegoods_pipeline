﻿#include <stdlib.h>
#include <stdio.h>
#include <stdarg.h>
#include "azure_c_shared_utility/platform.h"
#include "azure_c_shared_utility/threadapi.h"
#include "iothub_client.h"
#include "iothubtransportmqtt.h"
#include "jsondecoder.h"
#include "jsonencoder.h"
#include "azure_iot_hub.h"
#include "utils.h"
#include "state.h"

#include <applibs/log.h>

//
// String containing Hostname, Device Id & Device Key in the format:
// "HostName=<host_name>;DeviceId=<device_id>;SharedAccessKey=<device_key>"
//
// Note: this connection string is specific to the device "MT3620DJ". To configure other devices,
// see information on iothub-explorer at http://aka.ms/iothubgetstartedVSCS
//
static const char connection_string[] =
    "HostName=IoTHubWGck4qt.azure-devices.net;DeviceId=MT3620DJ;SharedAccessKey=oUX01b+y58V/wlF+7y3HZBu6CsPJoGY6whYVtBO0OHM=";

/// <summary>
///     Maximum amount of time to attempt reconnection when the connection to the IoT Hub drops.
/// </summary>
/// <remarks>Time expressed in seconds. A value of 0 means to retry forever.</remarks>
static const size_t retryTimeoutSeconds = 0;

// Refer to http://aka.ms/azure-iot-hub-vs-cs-wiki for more information on Connected Service for
// Azure IoT Hub

const char AzureIoTCertificatesX[] =
    /* Baltimore */
    "-----BEGIN CERTIFICATE-----\r\n"
    "MIIDdzCCAl+gAwIBAgIEAgAAuTANBgkqhkiG9w0BAQUFADBaMQswCQYDVQQGEwJJ\r\n"
    "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\r\n"
    "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTAwMDUxMjE4NDYwMFoX\r\n"
    "DTI1MDUxMjIzNTkwMFowWjELMAkGA1UEBhMCSUUxEjAQBgNVBAoTCUJhbHRpbW9y\r\n"
    "ZTETMBEGA1UECxMKQ3liZXJUcnVzdDEiMCAGA1UEAxMZQmFsdGltb3JlIEN5YmVy\r\n"
    "VHJ1c3QgUm9vdDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBAKMEuyKr\r\n"
    "mD1X6CZymrV51Cni4eiVgLGw41uOKymaZN+hXe2wCQVt2yguzmKiYv60iNoS6zjr\r\n"
    "IZ3AQSsBUnuId9Mcj8e6uYi1agnnc+gRQKfRzMpijS3ljwumUNKoUMMo6vWrJYeK\r\n"
    "mpYcqWe4PwzV9/lSEy/CG9VwcPCPwBLKBsua4dnKM3p31vjsufFoREJIE9LAwqSu\r\n"
    "XmD+tqYF/LTdB1kC1FkYmGP1pWPgkAx9XbIGevOF6uvUA65ehD5f/xXtabz5OTZy\r\n"
    "dc93Uk3zyZAsuT3lySNTPx8kmCFcB5kpvcY67Oduhjprl3RjM71oGDHweI12v/ye\r\n"
    "jl0qhqdNkNwnGjkCAwEAAaNFMEMwHQYDVR0OBBYEFOWdWTCCR1jMrPoIVDaGezq1\r\n"
    "BE3wMBIGA1UdEwEB/wQIMAYBAf8CAQMwDgYDVR0PAQH/BAQDAgEGMA0GCSqGSIb3\r\n"
    "DQEBBQUAA4IBAQCFDF2O5G9RaEIFoN27TyclhAO992T9Ldcw46QQF+vaKSm2eT92\r\n"
    "9hkTI7gQCvlYpNRhcL0EYWoSihfVCr3FvDB81ukMJY2GQE/szKN+OMY3EU/t3Wgx\r\n"
    "jkzSswF07r51XgdIGn9w/xZchMB5hbgF/X++ZRGjD8ACtPhSNzkE1akxehi/oCr0\r\n"
    "Epn3o0WC4zxe9Z2etciefC7IpJ5OCBRLbf1wbWsaY71k5h+3zvDyny67G7fyUIhz\r\n"
    "ksLi4xaNmjICq44Y3ekQEe5+NauQrz4wlHrQMz2nZQ/1/I6eYs9HRCwBXbsdtTLS\r\n"
    "R9I4LtD+gdwyah617jzV/OeBHRnDJELqYzmp\r\n"
    "-----END CERTIFICATE-----\r\n"
    /* MSIT */
    "-----BEGIN CERTIFICATE-----\r\n"
    "MIIFhjCCBG6gAwIBAgIEByeaqTANBgkqhkiG9w0BAQsFADBaMQswCQYDVQQGEwJJ\r\n"
    "RTESMBAGA1UEChMJQmFsdGltb3JlMRMwEQYDVQQLEwpDeWJlclRydXN0MSIwIAYD\r\n"
    "VQQDExlCYWx0aW1vcmUgQ3liZXJUcnVzdCBSb290MB4XDTEzMTIxOTIwMDczMloX\r\n"
    "DTE3MTIxOTIwMDY1NVowgYsxCzAJBgNVBAYTAlVTMRMwEQYDVQQIEwpXYXNoaW5n\r\n"
    "dG9uMRAwDgYDVQQHEwdSZWRtb25kMR4wHAYDVQQKExVNaWNyb3NvZnQgQ29ycG9y\r\n"
    "YXRpb24xFTATBgNVBAsTDE1pY3Jvc29mdCBJVDEeMBwGA1UEAxMVTWljcm9zb2Z0\r\n"
    "IElUIFNTTCBTSEEyMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEA0eg3\r\n"
    "p3aKcEsZ8CA3CSQ3f+r7eOYFumqtTicN/HJq2WwhxGQRlXMQClwle4hslAT9x9uu\r\n"
    "e9xKCLM+FvHQrdswbdcaHlK1PfBHGQPifaa9VxM/VOo6o7F3/ELwY0lqkYAuMEnA\r\n"
    "iusrr/466wddBvfp/YQOkb0JICnobl0JzhXT5+/bUOtE7xhXqwQdvDH593sqE8/R\r\n"
    "PVGvG8W1e+ew/FO7mudj3kEztkckaV24Rqf/ravfT3p4JSchJjTKAm43UfDtWBpg\r\n"
    "lPbEk9jdMCQl1xzrGZQ1XZOyrqopg3PEdFkFUmed2mdROQU6NuryHnYrFK7sPfkU\r\n"
    "mYsHbrznDFberL6u23UykJ5jvXS/4ArK+DSWZ4TN0UI4eMeZtgzOtg/pG8v0Wb4R\r\n"
    "DsssMsj6gylkeTyLS/AydGzzk7iWa11XWmjBzAx5ihne9UkCXgiAAYkMMs3S1pbV\r\n"
    "S6Dz7L+r9H2zobl82k7X5besufIlXwHLjJaoKK7BM1r2PwiQ3Ov/OdgmyBKdHJqq\r\n"
    "qcAWjobtZ1KWAH8Nkj092XA25epCbx+uleVbXfjQOsfU3neG0PyeTuLiuKloNwnE\r\n"
    "OeOFuInzH263bR9KLxgJb95KAY8Uybem7qdjnzOkVHxCg2i4pd+/7LkaXRM72a1o\r\n"
    "/SAKVZEhZPnXEwGgCF1ZiRtEr6SsxwUQ+kFKqPsCAwEAAaOCASAwggEcMBIGA1Ud\r\n"
    "EwEB/wQIMAYBAf8CAQAwUwYDVR0gBEwwSjBIBgkrBgEEAbE+AQAwOzA5BggrBgEF\r\n"
    "BQcCARYtaHR0cDovL2N5YmVydHJ1c3Qub21uaXJvb3QuY29tL3JlcG9zaXRvcnku\r\n"
    "Y2ZtMA4GA1UdDwEB/wQEAwIBhjAdBgNVHSUEFjAUBggrBgEFBQcDAQYIKwYBBQUH\r\n"
    "AwIwHwYDVR0jBBgwFoAU5Z1ZMIJHWMys+ghUNoZ7OrUETfAwQgYDVR0fBDswOTA3\r\n"
    "oDWgM4YxaHR0cDovL2NkcDEucHVibGljLXRydXN0LmNvbS9DUkwvT21uaXJvb3Qy\r\n"
    "MDI1LmNybDAdBgNVHQ4EFgQUUa8kJpz0aCJXgCYrO0ZiFXsezKUwDQYJKoZIhvcN\r\n"
    "AQELBQADggEBAHaFxSMxH7Rz6qC8pe3fRUNqf2kgG4Cy+xzdqn+I0zFBNvf7+2ut\r\n"
    "mIx4H50RZzrNS+yovJ0VGcQ7C6eTzuj8nVvoH8tWrnZDK8cTUXdBqGZMX6fR16p1\r\n"
    "xRspTMn0baFeoYWTFsLLO6sUfUT92iUphir+YyDK0gvCNBW7r1t/iuCq7UWm6nnb\r\n"
    "2DVmVEPeNzPR5ODNV8pxsH3pFndk6FmXudUu0bSR2ndx80oPSNI0mWCVN6wfAc0Q\r\n"
    "negqpSDHUJuzbEl4K1iSZIm4lTaoNKrwQdKVWiRUl01uBcSVrcR6ozn7eQaKm6ZP\r\n"
    "2SL6RE4288kPpjnngLJev7050UblVUfbvG4=\r\n"
    "-----END CERTIFICATE-----\r\n"
    /* *.azure-devices.net */
    "-----BEGIN CERTIFICATE-----\r\n"
    "MIIGcjCCBFqgAwIBAgITWgABtrNbz7vBeV0QWwABAAG2szANBgkqhkiG9w0BAQsF\r\n"
    "ADCBizELMAkGA1UEBhMCVVMxEzARBgNVBAgTCldhc2hpbmd0b24xEDAOBgNVBAcT\r\n"
    "B1JlZG1vbmQxHjAcBgNVBAoTFU1pY3Jvc29mdCBDb3Jwb3JhdGlvbjEVMBMGA1UE\r\n"
    "CxMMTWljcm9zb2Z0IElUMR4wHAYDVQQDExVNaWNyb3NvZnQgSVQgU1NMIFNIQTIw\r\n"
    "HhcNMTUwODI3MDMxODA0WhcNMTcwODI2MDMxODA0WjAeMRwwGgYDVQQDDBMqLmF6\r\n"
    "dXJlLWRldmljZXMubmV0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA\r\n"
    "nXC/qBUdlnfIm5K3HYu0o/Mb5tNNcsr0xy4Do0Puwq2W1tz0ZHvIIS9VOANhkNCb\r\n"
    "VyOncnP6dvmM/rYYKth/NQ8RUiZOYlROZ0SYC8cvxq9WOln4GXtEU8vNVqJbYrJj\r\n"
    "rPMHfxqLzTE/0ZnQffnDT3iMUE9kFLHow0YgaSRU0KZsc9KAROmzBzu+QIB1WGKX\r\n"
    "D7CN361tG1UuN68Bz7MSnbgk98Z+DjDxfusoDhiiy/Y9MLOJMt4WIy5BqL3lfLnn\r\n"
    "r+JLqmpiFuyVUDacFQDprYJ1/AFgcsKYu/ydmASARPzqJhOGaC2sZP0U5oBOoBzI\r\n"
    "bz4tfn8Bi0kJKmS53mQt+wIDAQABo4ICOTCCAjUwCwYDVR0PBAQDAgSwMB0GA1Ud\r\n"
    "JQQWMBQGCCsGAQUFBwMBBggrBgEFBQcDAjAdBgNVHQ4EFgQUKpYehBSNA53Oxivn\r\n"
    "aLCz3+eFUJ0wXQYDVR0RBFYwVIITKi5henVyZS1kZXZpY2VzLm5ldIIaKi5hbXFw\r\n"
    "d3MuYXp1cmUtZGV2aWNlcy5uZXSCISouc3UubWFuYWdlbWVudC1henVyZS1kZXZp\r\n"
    "Y2VzLm5ldDAfBgNVHSMEGDAWgBRRryQmnPRoIleAJis7RmIVex7MpTB9BgNVHR8E\r\n"
    "djB0MHKgcKBuhjZodHRwOi8vbXNjcmwubWljcm9zb2Z0LmNvbS9wa2kvbXNjb3Jw\r\n"
    "L2NybC9tc2l0d3d3Mi5jcmyGNGh0dHA6Ly9jcmwubWljcm9zb2Z0LmNvbS9wa2kv\r\n"
    "bXNjb3JwL2NybC9tc2l0d3d3Mi5jcmwwcAYIKwYBBQUHAQEEZDBiMDwGCCsGAQUF\r\n"
    "BzAChjBodHRwOi8vd3d3Lm1pY3Jvc29mdC5jb20vcGtpL21zY29ycC9tc2l0d3d3\r\n"
    "Mi5jcnQwIgYIKwYBBQUHMAGGFmh0dHA6Ly9vY3NwLm1zb2NzcC5jb20wTgYDVR0g\r\n"
    "BEcwRTBDBgkrBgEEAYI3KgEwNjA0BggrBgEFBQcCARYoaHR0cDovL3d3dy5taWNy\r\n"
    "b3NvZnQuY29tL3BraS9tc2NvcnAvY3BzADAnBgkrBgEEAYI3FQoEGjAYMAoGCCsG\r\n"
    "AQUFBwMBMAoGCCsGAQUFBwMCMA0GCSqGSIb3DQEBCwUAA4ICAQCrjzOSW+X6v+UC\r\n"
    "u+JkYyuypXN14pPLcGFbknJWj6DAyFWXKC8ihIYdtf/szWIO7VooplSTZ05u/JYu\r\n"
    "ZYh7fAw27qih9CLhhfncXi5yzjgLMlD0mlbORvMJR/nMl7Yh1ki9GyLnpOqMmO+E\r\n"
    "yTpOiE07Uyt2uWelLHjMY8kwy2bSRXIp7/+A8qHRaIIdXNtAKIK5jo068BJpo77h\r\n"
    "4PljCb9JFdEt6sAKKuaP86Y+8oRZ7YzU4TLDCiK8P8n/gQXH0vvhOE/O0n7gWPqB\r\n"
    "n8KxsnRicop6tB6GZy32Stn8w0qktmQNXOGU+hp8OL6irULWZw/781po6d78nmwk\r\n"
    "1IFl2TB4+jgyblvJdTM0rx8vPf3F2O2kgsRNs9M5qCI7m+he43Bhue0Fj/h3oIIo\r\n"
    "Qx7X/uqc8j3VTNE9hf2A4wksSRgRydjAYoo+bduNagC5s7Eucb4mBG0MMk7HAQU9\r\n"
    "m/gyaxqth6ygDLK58wojSV0i4RiU01qZkHzqIWv5FhhMjbFwyKEc6U35Ps7kP/1O\r\n"
    "fdGm13ONaYqDl44RyFsLFFiiDYxZFDSsKM0WDxbl9ULAlVc3WR85kEBK6I+pSQj+\r\n"
    "7/Z5z2zTz9qOFWgB15SegTbjSR7uk9mEVnj9KDlGtG8W1or0EGrrEDP2CMsp0oEj\r\n"
    "VTJbZAxEaZ3cVCKva5sQUxFMjwG32g==\r\n"
    "-----END CERTIFICATE-----\r\n";

// Forward declarations.
static void sendMessageCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void *context);
static IOTHUBMESSAGE_DISPOSITION_RESULT receiveMessageCallback(IOTHUB_MESSAGE_HANDLE message,
                                                               void *context);

static void twinCallback(DEVICE_TWIN_UPDATE_STATE updateState, const unsigned char *payLoad,
                         size_t size, void *userContextCallback);

static int directMethodCallback(const char *methodName, const unsigned char *payload, size_t size,
                                unsigned char **response, size_t *response_size,
                                void *userContextCallback);

static void hubConnectionStatusCallback(IOTHUB_CLIENT_CONNECTION_STATUS result,
                                        IOTHUB_CLIENT_CONNECTION_STATUS_REASON reason,
                                        void *userContextCallback);

#define MAX_CONNECTION_STRING_SIZE 512
_Static_assert(sizeof(connection_string) <= MAX_CONNECTION_STRING_SIZE,
               "Connection string too long");

/// <summary>
///     The handle to the IoT Hub client used for communication with the hub.
/// </summary>
static IOTHUB_CLIENT_LL_HANDLE iothub_client_handle = NULL;

/// <summary>
///     Used to set the keepalive period over MQTT to 20 seconds.
/// </summary>
static int keepalivePeriodSeconds = 20;

/// <summary>
///     Log a message related to the Azure IoT Hub client with
///		prefix [Azure IoT Hub client]:"
/// </summary>
/// <param name="message">The format string containing the error to output along with
/// placeholders</param>
/// <param name="...">The list of arguments to populate the format string placeholders</param>
void LogMessage(char *message, ...)
{
    va_list args;
    va_start(args, message);
    Log_Debug("[Azure IoT Hub client]:");
    Log_DebugVarArgs(message, args);
    va_end(args);
}

/// <summary>
///     Sets up the client in order to establish the communication channel to Azure IoT Hub.
///
///     The client is created by using the IoT Hub connection string that is provisioned
///     on the device or hardcoded into the source. The client is setup with the following
///     options:
///     - IOTHUB_CLIENT_RETRY_INTERVAL retry policy which, when the network connection
///       to the IoT Hub drops, attempts a reconnection each 5 seconds for a period of time
///       of 5000 seconds before giving up;
///     - MQTT procotol 'keepalive' value of 20 seconds; when no PINGRESP is received after
///       20 seconds, the connection is believed to be down and the retry policy kicks in;
///     - MQTT protocol output trace disabled; by enabling the traces it is possible to
///       see all the MQTT messages exchanged with the hub.
/// </summary>
/// <returns>'true' if the client has been properly set up. 'false' when a fatal error occurred
/// while setting up the client.</returns>
/// <remarks>This function is a no-op when the client has already been set up, i.e. this
/// function has already completed successfully.</remarks>
bool AzureIoT_SetupClient(void)
{
    if (iothub_client_handle != NULL)
        return true;

    iothub_client_handle =
        IoTHubClient_LL_CreateFromConnectionString(connection_string, MQTT_Protocol);

    if (iothub_client_handle == NULL) {
        return false;
    }

    IOTHUB_CLIENT_RESULT azureRes =
        IoTHubClient_LL_SetOption(iothub_client_handle, "TrustedCerts", AzureIoTCertificatesX);
    if (azureRes != IOTHUB_CLIENT_OK) {
        LogMessage("ERROR: failure to set option \"TrustedCerts\"\n");
        return false;
    }

    // Disable trace-level logs due to verbosity.
    bool logTraceEnabled = false;
    if (IoTHubClient_LL_SetOption(iothub_client_handle, OPTION_LOG_TRACE, &logTraceEnabled) !=
        IOTHUB_CLIENT_OK) {
        LogMessage("ERROR: failure setting option \"%s\"\n", OPTION_LOG_TRACE);
        return false;
    }

    if (IoTHubClient_LL_SetOption(iothub_client_handle, OPTION_KEEP_ALIVE,
                                  &keepalivePeriodSeconds) != IOTHUB_CLIENT_OK) {
        LogMessage("ERROR: failure setting option \"%s\"\n", OPTION_KEEP_ALIVE);
        return false;
    }

    // Set callbacks for Message, MethodCall and Device Twin features.
    IoTHubClient_LL_SetMessageCallback(iothub_client_handle, receiveMessageCallback, NULL);
    IoTHubClient_LL_SetDeviceMethodCallback(iothub_client_handle, directMethodCallback, NULL);
    IoTHubClient_LL_SetDeviceTwinCallback(iothub_client_handle, twinCallback, NULL);

    // Set callbacks for connection status related events reported by the IoT Hub sdk.
    if (IoTHubClient_LL_SetConnectionStatusCallback(
            iothub_client_handle, hubConnectionStatusCallback, NULL) != IOTHUB_CLIENT_OK) {
        LogMessage("ERROR: failure setting callback\n");
        return false;
    }

    // Set retry policy for the connection to the IoT Hub.
    if (IoTHubClient_LL_SetRetryPolicy(iothub_client_handle, IOTHUB_CLIENT_RETRY_INTERVAL,
                                       retryTimeoutSeconds) != IOTHUB_CLIENT_OK) {
        LogMessage("ERROR: failure setting retry policy\n");
        return false;
    }

    return true;
}

/// <summary>
///     Destroys the Azure IoT Hub client.
/// </summary>
void AzureIoT_DestroyClient(void)
{
    if (iothub_client_handle != NULL) {
        IoTHubClient_LL_Destroy(iothub_client_handle);
        iothub_client_handle = NULL;
    }
}

/// <summary>
///     Periodicallly outputs a provided format string with a variable number of arguments.
/// </summary>
/// <param value="lastInvokedTime">Pointer where to store the 'last time this function has been
/// invoked' information</param>
/// <param value="periodInSeconds">The desired period of the output</param>
/// <param value="format">The format string</param>
static void PeriodicLogVarArgs(time_t *lastInvokedTime, time_t periodInSecond, const char *format,
                               ...)
{
    struct timespec ts;
    int timeOk = timespec_get(&ts, TIME_UTC);
    if (timeOk) {
        if (ts.tv_sec > *lastInvokedTime + periodInSecond) {
            va_list args;
            va_start(args, format);
            Log_DebugVarArgs(format, args);
            va_end(args);
            *lastInvokedTime = ts.tv_sec;
        }
    }
}

/// <summary>
///     Keeps IoT Hub Client alive by exchanging data with the Azure IoT Hub.
/// </summary>
/// <remarks>
///     This function must to be invoked periodically so that the Azure IoT Hub
///     SDK can accomplish its work (e.g. sending messages, invocation of callbacks, reconnection
///     attempts, and so forth).
/// </remarks>
void AzureIoT_DoPeriodicTasks(void)
{
    static time_t lastTimeLogged;
    PeriodicLogVarArgs(&lastTimeLogged, 5, "%s calls in progress...\n", __func__);

    // Send all the buffered events to the IoT Hub, and receive all the buffered events from the IoT
    // Hub.
    IOTHUB_CLIENT_STATUS status;
    do {
        IoTHubClient_LL_DoWork(iothub_client_handle);
    } while ((IoTHubClient_LL_GetSendStatus(iothub_client_handle, &status) == IOTHUB_CLIENT_OK) &&
             (status == IOTHUB_CLIENT_SEND_STATUS_BUSY));
}

/// <summary>
///     Creates and enqueues a message to be delivered the IoT Hub. The message is not actually sent
///     immediately, but it is sent on the next invocation of AzureIoT_DoPeriodicTasks().
/// </summary>
/// <param name="messagePayload">The payload of the message to send.</param>
void AzureIoT_SendMessage(const char *messagePayload)
{
    if (iothub_client_handle == NULL) {
        LogMessage("ERROR: IoT Hub client not initialized\n");
    } else {
        IOTHUB_MESSAGE_HANDLE message_handle = IoTHubMessage_CreateFromString(messagePayload);

        if (message_handle == 0) {
            LogMessage("ERROR: unable to create a new IoTHubMessage\n");
        } else {
            if (IoTHubClient_LL_SendEventAsync(iothub_client_handle, message_handle,
                                               sendMessageCallback,
                                               /*&callback_param*/ 0) != IOTHUB_CLIENT_OK) {
                LogMessage("ERROR: failed to hand over the message to IoTHubClient");
            } else {
                LogMessage("INFO: IoTHubClient accepted the message for delivery\n");
            }
        }
    }
}

/// <summary>
///		Function invoked to provide the result of the Device Twin reported properties
///     delivery.
/// </summary>
static DeviceTwinDeliveryConfirmationFnType deviceTwinConfirmationCb = 0;

/// <summary>
///     Sets the function to be invoked whenever the Device Twin properties have been delivered to
///     the IoT Hub.
/// </summary>
/// <param name="callback">The function pointer to the callback function.</param>
void AzureIoT_SetDeviceTwinDeliveryConfirmationCallback(
    DeviceTwinDeliveryConfirmationFnType callback)
{
    deviceTwinConfirmationCb = callback;
}

/// <summary>
///     Callback invoked when the Device Twin reported properties are accepted by IoT Hub.
/// </summary>
static void reportStatusCallback(int result, void *context)
{
    LogMessage("INFO: Reported state accepted by IoT Hub. Result is: %d\n", result);
    if (deviceTwinConfirmationCb)
        deviceTwinConfirmationCb(result);
}

/// <summary>
///     Empty clone function for multitree.
/// </summary>
static int NOPCloneFunction(void **destination, const void *source)
{
    *destination = (void **)source;
    return 0;
}

/// <summary>
///     Empty free function for multitree.
/// </summary>
static void NoFreeFunction(void *value)
{
    (void)value;
}

/// <summary>
///     Creates and enqueues a report containing the name and value pair of a Device Twin reported
///     property.
///     The report is not actually sent immediately, but it is sent on the next invocation of
///     AzureIoT_DoPeriodicTasks().
/// </summary>
void AzureIoT_TwinReportState(const char *propertyName, const char *propertyValue)
{
    if (iothub_client_handle == NULL) {
        LogMessage("ERROR: client not initialized\n");
    } else {
        MULTITREE_HANDLE tree = MultiTree_Create(NOPCloneFunction, NoFreeFunction);

        if (tree != NULL) {
            MULTITREE_RESULT mtreeResult = MULTITREE_OK;

            if (mtreeResult == MULTITREE_OK) {
                mtreeResult = MultiTree_AddLeaf(tree, propertyName, propertyValue);
            }

            if (MULTITREE_OK == mtreeResult) {
                STRING_HANDLE res = STRING_new();

                if (res != NULL) {
                    if (JSON_ENCODER_OK ==
                        JSONEncoder_EncodeTree(tree, res, JSONEncoder_CharPtr_ToString)) {
                        const char *json = STRING_c_str(res);

                        if (IoTHubClient_LL_SendReportedState(
                                iothub_client_handle, (unsigned char *)json, strlen(json),
                                reportStatusCallback, 0) != IOTHUB_CLIENT_OK) {
                            LogMessage("ERROR: failed to set reported state\n");
                        } else {
                            LogMessage("INFO: Reported state set\n");
                        }
                    } else {
                        LogMessage("ERROR: failed to encode JSON\n");
                    }

                    STRING_delete(res);
                } else {
                    LogMessage("ERROR: failed to allocate buffer for JSON\n");
                }
            }

            MultiTree_Destroy(tree);
        } else {
            LogMessage("ERROR: Failed to allocate multitree\n");
        }
    }
}

/// <summary>
///     Function invoked whenever a message is received from the IoT Hub.
/// </summary>
static MessageReceivedFnType messageReceivedCb = 0;

/// <summary>
///     Sets a callback function invoked whenever a message is received from IoT Hub.
/// </summary>
/// <param name="callback">The callback function invoked when a message is received</param>
void AzureIoT_SetMessageReceivedCallback(MessageReceivedFnType callback)
{
    messageReceivedCb = callback;
}

/// <summary>
///		Function invoked to report the delivery confirmation of a message sent to the IoT
///     Hub.
/// </summary>
static MessageDeliveryConfirmationFnType messageDeliveryConfirmationCb = 0;

/// <summary>
///     Sets the function to be invoked whenever the message to the Iot Hub has been delivered.
/// </summary>
/// <param name="callback">The function pointer to the callback function.</param>
void AzureIoT_SetMessageConfirmationCallback(MessageDeliveryConfirmationFnType callback)
{
    messageDeliveryConfirmationCb = callback;
}

/// <summary>
///     Function invoked when the message delivery confirmation is being reported.
/// </summary>
/// <param name="result">Message delivery status</param>
/// <param name="context">User specified context</param>
static void sendMessageCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void *context)
{
    LogMessage("INFO: Message received by IoT Hub. Result is: %d\n", result);
    if (messageDeliveryConfirmationCb)
        messageDeliveryConfirmationCb(result == IOTHUB_CLIENT_CONFIRMATION_OK);
}

/// <summary>
///     Callback function invoked when a message is received from IoT Hub.
/// </summary>
/// <param name="message">The handle of the received message</param>
/// <param name="context">The user context specified at IoTHubClient_LL_SetMessageCallback()
/// invocation time</param>
/// <returns>Return value to indicates the message procession status (i.e. accepted, rejected,
/// abandoned)</returns>
static IOTHUBMESSAGE_DISPOSITION_RESULT receiveMessageCallback(IOTHUB_MESSAGE_HANDLE message,
                                                               void *context)
{
    const unsigned char *buffer;
    size_t size;
    if (IoTHubMessage_GetByteArray(message, &buffer, &size) != IOTHUB_MESSAGE_OK) {
        LogMessage("ERROR: failure performing IoTHubMessage_GetByteArray\n");
    } else {
        /*buffer is not zero terminated*/
        unsigned char *str_msg = malloc(size + 1);

        if (str_msg != NULL) {
            memcpy(str_msg, buffer, size);
            str_msg[size] = '\0';

            if (messageReceivedCb != 0)
                messageReceivedCb(str_msg);
            else
                LogMessage(
                    "WARNING: no user callback set up for event 'message received from IoT Hub'\n");

            LogMessage("INFO: Received message '%s' from IoT Hub\n", str_msg);
            free(str_msg);
        } else {
            LogMessage("ERROR: could not allocate buffer for received message\n");
            abort();
        }
    }

    return IOTHUBMESSAGE_ACCEPTED;
}

/// <summary>
///     Function invoked whenever a Direct Method call is received from the IoT Hub.
/// </summary>
static DirectMethodCallFnType directMethodCallCb = 0;

/// <summary>
///     Sets the function to be invoked whenever a Direct Method call from the IoT Hub is received.
/// </summary>
/// <param name="callback">The callback function invoked when a Direct Method call is
/// received</param>
void AzureIoT_SetDirectMethodCallback(DirectMethodCallFnType callback)
{
    directMethodCallCb = callback;
}

/// <summary>
///     Function invoked whenever a Device Twin update is received from the IoT Hub.
/// </summary>
static TwinUpdateFnType twinUpdateCb = 0;

/// <summary>
///     Sets the function callback invoked whenever a Device Twin update from the IoT Hub is
///     received.
/// </summary>
/// <param name="callback">The callback function invoked when a Device Twin update is
/// received</param>
void AzureIoT_SetDeviceTwinUpdateCallback(TwinUpdateFnType callback)
{
    twinUpdateCb = callback;
}

/// <summary>
///     Callback when direct method is called.
/// </summary>
static int directMethodCallback(const char *methodName, const unsigned char *payload, size_t size,
                                unsigned char **response, size_t *responseSize,
                                void *userContextCallback)
{
    LogMessage("INFO: Trying to invoke method %s\n", methodName);

    int result = 404;

    if (directMethodCallCb != NULL) {
        char *responseFromCallback = NULL;
        size_t responseFromCallbackSize = 0;

        result = directMethodCallCb(methodName, payload, size, &responseFromCallback,
                                    &responseFromCallbackSize);
        *responseSize = responseFromCallbackSize;
        *response = responseFromCallback;
    } else {
        LogMessage("INFO: No method '%s' found, HttpStatus=%d\n", methodName, result);
        static const char methodNotFound[] = "\"No method found\"";
        *responseSize = strlen(methodNotFound);
        *response = (unsigned char *)malloc(*responseSize);
        if (*response != NULL) {
            strncpy((char *)(*response), methodNotFound, *responseSize);
        } else {
            LogMessage("ERROR: Cannot create response message for method call.\n");
            abort();
        }
    }

    return result;
}

/// <summary>
///     Process to the firing of a desired property (features or device) 
/// </summary>
static void twinDesiredPropertyProcess(const char *childName, const char *propertyName, MULTITREE_HANDLE subtree) {
	const void *value = NULL;
	if (MULTITREE_OK == MultiTree_GetLeafValue(subtree, propertyName, &value))
	{
		const char *valueString = (const char*)value;
		if (twinUpdateCb)
			twinUpdateCb(childName, propertyName, valueString);
		Log_Debug("INFO: Property %s changed, new value is %s\n", propertyName, valueString);
	}
}


/// <summary>
///     Callback invoked when a Device Twin update is received from IoT Hub.
/// </summary>
static void twinCallback(DEVICE_TWIN_UPDATE_STATE updateState, const unsigned char *payLoad,
                         size_t payLoadSize, void *userContextCallback)
{
    size_t nullTerminatedJsonSize = payLoadSize + 1;
    char *nullTerminatedJsonString = (char *)malloc(nullTerminatedJsonSize);
    if (nullTerminatedJsonString == NULL) {
        LogMessage("ERROR: Could not allocate buffer for twin update payload.\n");
        abort();
    } else {
        // Copy the provided buffer to a null terminated buffer.
        memcpy(nullTerminatedJsonString, payLoad, payLoadSize);
        nullTerminatedJsonString[nullTerminatedJsonSize - 1] =
            0; // Add the null terminator at the end.

		MULTITREE_HANDLE tree = NULL;
		if (JSON_DECODER_OK == JSONDecoder_JSON_To_MultiTree(nullTerminatedJsonString, &tree)) {
			MULTITREE_HANDLE child = NULL;
			MULTITREE_HANDLE features = NULL;

			//if (MULTITREE_OK == MultiTree_GetChildByName(tree, "desired", &child)) {
			//	//child = tree;
			//}

			if (MULTITREE_OK == MultiTree_GetChildByName(tree, "features", &features)) {
				for (int i = 0; i < 3; i++) {
					/*if (twinUpdateCb)
						twinUpdateCb(features, FEATURES[i].name, FEATURES);*/
						//twinUpdateCb(features);
					twinDesiredPropertyProcess("features", FEATURES[i].name, features);
				}
				
			}
		}


        //MULTITREE_HANDLE propertiesRoot = NULL;
        //if (JSON_DECODER_OK ==
        //    JSONDecoder_JSON_To_MultiTree(nullTerminatedJsonString, &propertiesRoot)) {

        //    // Get the root node of the desired properties.
        //    MULTITREE_HANDLE desiredProperties = propertiesRoot;
        //    // Get the desired properties root node. When it is not present,
        //    // there propertiesRoot already points to the root node of desired
        //    // properties.
        //    MultiTree_GetChildByName(propertiesRoot, "desired", &desiredProperties);

        //    // Call the provided Twin Device callback if any.
        //    if (twinUpdateCb)
        //        twinUpdateCb(desiredProperties);

        //    // Destroy the multitree.
        //    MultiTree_Destroy(propertiesRoot);
        //} else {
        //    LogMessage("ERROR: Cannot parse the string as JSON content.\n");
        //}

        free(nullTerminatedJsonString);
    }
}

/// <summary>
///     Function invoked whenever the connection status to the IoT Hub changes.
/// </summary>
static ConnectionStatusFnType hubConnectionStatusCb = 0;

/// <summary>
///     Sets the function to be invoked whenever the connection status to the IoT Hub changes.
/// </summary>
/// <param name="callback">The callback function invoked when Azure IoT Hub network connection
/// status changes.</param>
void AzureIoT_SetConnectionStatusCallback(ConnectionStatusFnType callback)
{
    hubConnectionStatusCb = callback;
}

/// <summary>
///     Callback function invoked whenever the connection status to IoT Hub changes.
/// </summary>
/// <param name="result">Current authentication status</param>
/// <param name="reason">result's specific reason</param>
/// <param name="userContextCallback">User specified context</param>
static void hubConnectionStatusCallback(IOTHUB_CLIENT_CONNECTION_STATUS result,
                                        IOTHUB_CLIENT_CONNECTION_STATUS_REASON reason,
                                        void *userContextCallback)
{
    bool authenticated = (result == IOTHUB_CLIENT_CONNECTION_AUTHENTICATED);
    if (hubConnectionStatusCb)
        hubConnectionStatusCb(result == IOTHUB_CLIENT_CONNECTION_AUTHENTICATED);
    if (!authenticated) {
        LogMessage("INFO: IoT Hub connection is down, retrying connection in 5 seconds...\n");
    } else {
        LogMessage("INFO: connection to the IoT Hub has been established.\n");
    }
}

/// <summary>
///     Initializes the Azure IoT Hub SDK.
/// </summary>
/// <return>'true' if initialization has been successful.</param>
bool AzureIoT_Initialize(void)
{
    if (platform_init() != 0) {
        LogMessage("ERROR: failed initializing platform.\n");
        return false;
    }
    return true;
}

/// <summary>
///     Deinitializes the Azure IoT Hub SDK.
/// </summary>
void AzureIoT_Deinitialize(void)
{
    platform_deinit();
}

/// <summary>
///      Updates device twin reported properties during initialization
/// </summary>

MULTITREE_RESULT  AzureIoT_InitFeatureReportState(MULTITREE_HANDLE treeNode, const char *featureName, const char *featureDisplayName, const char *featureMethods, const bool featureIsActivated) {
	MULTITREE_RESULT mtreeResult = MULTITREE_OK;
	MULTITREE_HANDLE child = MultiTree_Create(NOPCloneFunction, NoFreeFunction);

#define abcd "displayName"

	mtreeResult = MultiTree_AddChild(treeNode, featureName, &child);

	if (mtreeResult == MULTITREE_OK)
	{
		mtreeResult = MultiTree_AddLeaf(child, abcd, featureDisplayName);
		if (mtreeResult == MULTITREE_OK)
			mtreeResult = MultiTree_AddLeaf(child, "methods", featureMethods);
		if (mtreeResult == MULTITREE_OK)
			mtreeResult = MultiTree_AddLeaf(child, "isActivated", featureIsActivated ? "true" : "false");
	}
	return mtreeResult;
} 

void AzureIoT_InitReportState() {
	if (iothub_client_handle == NULL) {
		Log_Debug("Error: Not Initialized\n");
	}
	else {
		MULTITREE_HANDLE tree = MultiTree_Create(NOPCloneFunction, NoFreeFunction);

		if (tree != NULL)
		{
			char str_heartbeat[31];
			//char str_ip_address[42];
			//char **str_device_properties;

			MULTITREE_RESULT mtreeResult = MULTITREE_OK;
			//MULTITREE_HANDLE child_device = MultiTree_Create(NOPCloneFunction, NoFreeFunction);
			MULTITREE_HANDLE child_feature = MultiTree_Create(NOPCloneFunction, NoFreeFunction);

			if (mtreeResult == MULTITREE_OK) {
				Utils_GetFormattedDateFromTime(str_heartbeat, sizeof(str_heartbeat), time(NULL));
				mtreeResult = MultiTree_AddLeaf(tree, "heartbeat", str_heartbeat);
			}

			if (mtreeResult == MULTITREE_OK) {
				char activationDate[31];
				Utils_GetFormattedDateFromTime(activationDate, sizeof(activationDate), time(NULL));
				mtreeResult = MultiTree_AddLeaf(tree, "activationDate", activationDate);
			}

			if (mtreeResult == MULTITREE_OK) {
				const char *statusCode;
				statusCode = "0";
				mtreeResult = MultiTree_AddLeaf(tree, "statusCode", statusCode);
			}
			
			if (mtreeResult == MULTITREE_OK) {
				mtreeResult = MultiTree_AddChild(tree, "featuresDefinitions", &child_feature);
				for (int i = 0; i < 3; i++)
				{
					if (mtreeResult == MULTITREE_OK)
					{
						mtreeResult = AzureIoT_InitFeatureReportState(child_feature, FEATURES[i].name, FEATURES[i].displayName, FEATURES[i].methods, false);
					}
				}
			}

			else
			{
				Log_Debug("ERROR: Failed to create child node featuresDefinitions\n");
			}

			if (mtreeResult == MULTITREE_OK) {
				STRING_HANDLE res = STRING_new();

				if (res != NULL) {
					if (JSON_ENCODER_OK == JSONEncoder_EncodeTree(tree, res, JSONEncoder_CharPtr_ToString))
					{
						const char* json = STRING_c_str(res);

						if (IoTHubClient_LL_SendReportedState(iothub_client_handle, (unsigned char*)json, strlen(json), reportStatusCallback, 0) != IOTHUB_CLIENT_OK)
						{
							Log_Debug("ERROR: Failed to set reported state \n");
						}
						else
						{
							Log_Debug("INFO: Reported state set from initial report\n");
						}
					}
					else
					{
						Log_Debug("ERROR: Failed to encode JSON\n");
					}

					STRING_delete(res);
				}
				else
				{
					Log_Debug("ERROR: Failed to allocate buffer for JSON\n");
				}
			}
			MultiTree_Destroy(tree);
		}
	}
}

void AzureIoT_TwinReportStateFeature(const char *featureName, const char *propertyName, const char *propertyValue)
{
	if (iothub_client_handle == NULL)
	{
		Log_Debug("ERROR: Not initialized\n");
	}
	else
	{
		MULTITREE_HANDLE tree = MultiTree_Create(NOPCloneFunction, NoFreeFunction);

		if (tree != NULL)
		{
			MULTITREE_RESULT mtreeResult = MULTITREE_OK;
			MULTITREE_HANDLE features = MultiTree_Create(NOPCloneFunction, NoFreeFunction);
			MULTITREE_HANDLE child = MultiTree_Create(NOPCloneFunction, NoFreeFunction);

			if (mtreeResult == MULTITREE_OK) {
				mtreeResult = MultiTree_AddChild(tree, "featuresDefinitions", &features);
			}

			if (mtreeResult == MULTITREE_OK) {
				mtreeResult = MultiTree_AddChild(features, featureName, &child);
			}

			if (mtreeResult == MULTITREE_OK) {
				mtreeResult = MultiTree_AddLeaf(child, propertyName, propertyValue);

				if (MULTITREE_OK == mtreeResult)
				{
					STRING_HANDLE res = STRING_new();

					if (res != NULL)
					{
						if (JSON_ENCODER_OK == JSONEncoder_EncodeTree(tree, res, JSONEncoder_CharPtr_ToString))
						{
							const char* json = STRING_c_str(res);

							if (IoTHubClient_LL_SendReportedState(iothub_client_handle, (unsigned char*)json, strlen(json), reportStatusCallback, 0) != IOTHUB_CLIENT_OK)
							{
								Log_Debug("ERROR: Failed to set reported state\n");
							}
							else
							{
								Log_Debug("INFO: Reported state set\n");
							}
						}
						else
						{
							Log_Debug("ERROR: Failed to encode JSON\n");
						}

						STRING_delete(res);
					}
					else
					{
						Log_Debug("ERROR: Failed to allocate buffer for JSON\n");
					}
				}
				//MultiTree_Destroy(child);
			}
			else
			{
				Log_Debug("ERROR: Failed to create child node %s\n", featureName);
			}

			MultiTree_Destroy(tree);
		}
		else
		{
			Log_Debug("ERROR: Failed to allocate multitree\n");
		}
	}
}
