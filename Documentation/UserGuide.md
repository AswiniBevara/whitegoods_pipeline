- [1 Login to Device Management Application](#1-login-to-device-management-application)
- [2 Connect to IoT Hub](#2-connect-to-iot-hub)
    - [2.1 Set Up Microsoft Azure Credentials](#21-set-up-microsoft-azure-credentials)
    - [2.2 Add your Device to the IoT Hub](#22-add-your-device-to-the-iot-hub)
- [3 Run the Blink Applications](#3-run-the-blink-applications)
- [4 Verify data in IoT Hub Device Twin](#4-verify-data-in-iot-hub-device-twin)
- [5 Verify Device Management application](#5-verify-device-management-application)
- [6 Validating Blink App code with Azure Sphere](#6-validating-blin-app-code-with-azure-sphere)
- [7 Monitoring Components](#7-monitoring-components)
    - [7.1 Application Insights](#71-application-insights)
    - [7.2 OMS Log Analytics](#72-oms-log-analytics)

## 1. Login to Device Management Application

Once the solution is deployed successfully, navigate to the resource group and select the created resource group to view the list of resources that are created in the Resource Group as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u1.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u2.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u3.png)

Go to **Resource Group** -> Click on the deployed **App Service**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u4.png)

Now you can see the app service **overview** page, copy the **URL** and browse it in new web browser.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u5.png)

To access the Device Management portal, login with your credentials.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u6.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u7.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u8.png)

Now you can view the **Device Management Portal** Dashboard.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/u9.png)

## 2 Connect to IoT Hub

### 2.1 Set Up Microsoft Azure Credentials

To run this sample, you must have a Microsoft Azure subscription and an IoT Hub. 

After you set up the subscription, you can create a hub. Log into the Azure Portal and follow these instructions to set up your hub.

### 2.2 Add your Device to the IoT Hub

To use an IoT Hub in an Azure Sphere application, you identify the IoT Hub that you plan to use and then add your device to that hub. The connected service retrieves the IoT Hub connection string and records it in a file named azure_iot_hub.c. The connected service then adds azure_iot_hub.c and the header file azure_iot_hub.h to your application.

Download **Mt3620AzureIoTHub4.zip** and extract it.

Click on open **Mt3620AzureIoTHub4.sln** in your local system.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/66.png)

In Solution Explorer, **right-click References** and then select **Add Connected Service**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/67.png)

After the Connected Services window appears, **click** on **find more services**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/68.png)

In the search box at right side menu, search for **Azure IoT Hub Sample of Device Simulation** and click on **Download** and **Install**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/69.png)

Close the Visual Studio to start the Installation of **Azure IoT Hub sample of Device Simulation**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/70.png)

After you see the below screen with **Modifications Complete**, click on **Close**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/71.png)

Re-open the Visual Studio and open the Blink application downloaded earlier. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/72.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/73.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/74.png)

Select **Azure IoT Hub (Azure Sphere)** from the list of **connected services**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/75.png)

**Log in** to Microsoft Azure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p10.png)

Enter the user credentials.

**Note:** You must have a Microsoft Azure subscription. If your organization does not already have them, follow these instructions to set up a free trial subscription to Microsoft Azure. After you set up the subscription. Log into the Azure Portal and follow these instructions to login.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/77.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p11.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/79.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/80.png)

Select **Hardcode shared access key** in application's code, and then **click Next**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p12.png)

In the list of hubs, select deployed IoT Hub from your resource group.

click **Next**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/82.png)

Open the **New Device tab**, in the Device ID field type device name that need to be created and click on **Create**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/83.png)

**Copy** the **device primary connection string**, which we will use later while running the Blink Application and click on **cancel**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p13.png)

## 3 Run the Blink Applications 

In Blink app Solution Explorer, select **azure_iot_hub.c** in your solution and update the IoT Hub connection string which was copied from above step and **save** it.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/85.png)

Now **open** the **app_manifest.json** file and update the IoT Hub Host name from the connection string.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/86.png)

Now you can build the application and use the Azure IoT Hub. In this walkthrough, we will use existing IoT Hub tools to monitor and communicate with your device.

Now **click on Remote GDB Debugger** and click on **Yes** to build the Blink App.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/87.png)

The output in the below screen shot shows that the build has been completed successfully and the Wi-Fi has been connected.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/88.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/89.png)

Once the application is running the output also shows the IoT Hub connected Status and Report Status of the device as below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/90.png)

## 4 Verify data in IoT Hub Device Twin

Go to **Resource Group** -> Click on **IoT Hub**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/100.png)

Click on **IoT devices** in left side menu and click **created device**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/101.png)

Here you can see the **Device Details** page. Click on **Device Twin**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/102.png)

Below is the Device Twin json file with Blink Application features as false.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/103.png)

## 5 Verify Device Management application 

Go to **Resource Group** -> click **Device Management**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/104.png)

After running the blink App the connected Status of the device will get reflected in the device management web app.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/105.png)

Click on **connected**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/106.png)

The features of the blink app show disabled. Similarly the same status can be seen by Clicking on **View JSON**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/107.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/108.png)

Click on **Activate** button on any of the blink app feature like **blinkrate1**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/109.png)

Once it is activated the same will be reflected in IoT Hub’s device twin.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/d1.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/110.png)

### 6 Validating Blink App code with Azure Sphere

To validate the blink rate, go to **resource group** -> go to **device management web app**. Select the device to which Azure Sphere Device is associated.

Under features section of the device, select Blink Rate1 as active.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/b1.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/111.png)

After selecting blink rate1 feature we can see LED light blinking slowly on Azure Sphere device like below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/b2.png)

For video reference on Blink Rate1 go through the below link.

**https://projectiot.blob.core.windows.net/whitegoods/BlinkRateVideos/BlinkRate1.mp4** 

Uncheck Blink Rate1 and select Blink Rate2 as active.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/b3.png)

When you select blink rate2 the LED light will blink a little faster on Azure Sphere device as shown in below screen shot.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/b4.png)

For video reference on Blink Rate2 go through the below link.

**https://projectiot.blob.core.windows.net/whitegoods/BlinkRateVideos/BlinkRate2.mp4** 

Uncheck Blink Rate2 and select Blink Rate 3 as active.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/b5.png)

when you select Blink Rate3 the LED Light will continuously blink on Azure Sphere device as shown in below screenshot.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/b6.png)

For video reference on Blink Rate3 go through the below link.

**https://projectiot.blob.core.windows.net/whitegoods/BlinkRateVideos/BlinkRate3.mp4**

## 7 Monitoring Components

### 7.1 Application Insights

Go to **Azure Portal**, select your **Resource Group** and select **Application Insights** as shown below. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/112.png)

On **Overview** page, Summary details are displayed as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/113.png)

Click on **Live Metric Stream**, which is available at left side menu to check the live requests of **Device Management Application**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/114.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/115.png)

Go back to the **Application Insights Overview** page in **Azure Portal**.

Then click **Metrics Explorer** on the left side of the page as shown below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/116.png)

Click **Edit** as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/117.png)

You can select any of the listed **Metrics** to view application logs.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/118.png)

If you want to **add new chart** click Add new chart as shown in the following figure and click **Edit** to add the specific metrics.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/119.png)

Go back to **Application Insights**, in the **Overview** page click **Analytics** icon in the **Health** section as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/120.png)

The **Application Insights** page is displayed and double click on **requests**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/121.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/122.png)

You can click **run** to see the specific requests of application as below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/123.png)

Now click on **chart** then click **Yes** to see the graph.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/124.png)

After click on chart you can see the **requests graph** like below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/125.png)

Now do the same process for checking graph for custom metrics.

Double click on **custom metrics**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/126.png)

You can click **run** to see the specific custom metrics of application as below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/127.png)

Now click on **chart** then click **Yes** to see the graph. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/128.png)

Similarly, you can run the other common Pre-defined queries by navigating back to the **Home Page**.

### 7.2 OMS Log Analytics

Open **Azure Portal** -> **Resource Group** -> Click the **OMS Workspace** in resource group to view **OMS Overview** Section. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/129.png)

Click **Azure Resources** on left side menu to view available Azure Resources. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/130.png)

Select your **Resource Group** name from the dropdown list.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/131.png)

Go to overview blade and click on **OMS Portal** as shown below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/132.png)

Once you click **OMS Workspace**, OMS Home Page will take few seconds to load and displayed as below. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/133.png)

Click **Search tab** to search the IoT hub, Stream Analytics, Cosmos DB. 

Click **Show legacy language converter**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/134.png)

Copy **IoT Hub** resource name, paste it in the **Converter** box and click **RUN**. 

The IoT Hub information is displayed below the page as shown in the following figure. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/135.png)

Copy **Cusmos DB** resource name, paste it in the **Converter** box and click **RUN**. 

The cosmos db information is displayed in the below page as shown in the following figure. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/136.png)

For **Stream Analytics logs**, first you need to enable the **Diagnostics logs**. 

Go to **Azure Portal**, click **STREAM ANALYTICS JOB** as shown below. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/137.png)

Click **Diagnostics logs** on the left pane and select **Add diagnostics setting** as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/138.png)

In the **Diagnostics settings** page, enter the name in the **Name** field. 

Select **Send to Log Analytics** checkbox. 

Select the **Execution** and **Authoring** checkboxes under the **LOG** section. 

Select **Allmetrics** checkbox under the **METRIC** section. 

Configure your workspace from the **OMS Workspaces page** as shown in the following figure.  

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/139.png)

Click **Save**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/140.png)

Go to **OMS Portal** and Click Search tab for **Stream Analytics logs**.

Copy **Stream Analytics Job** resource name, paste it in the **Converter** box and click **RUN**. 

The Stream Analytics Job information is displayed below the page as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/141.png) 

