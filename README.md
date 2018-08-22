# Microsoft

# White Goods Solution

**Table of Contents** 

- [1 About WhiteGoods Solution](#1-about-whitegoods-solution)
- [2 Solution Architecture Diagram and data work flow Diagram](#2-solution-architecture-diagram-and-data-work-flow-diagram)
    - [2.1 Basic Architecture Diagram](#21-basic-architecture-diagram)
    - [2.2 Premium Architecture Diagram](#22-premium-architecture-diagram)
    - [2.3 Data work Flow Diagram](23#-data-work-flow-diagram)
- [3 Azure Services](#3-azure-services)
    - [3.1 IoT Hub](#31-iothub)
    - [3.2 Stream Analytics](#32-stream-analytics)
    - [3.3 4x4 device](#33-4x4-device)
    - [3.4 Notification Hub](#34-notification-hub)
    - [3.5 Web Application](#35-web-application)
    - [3.6 Data packet web application](#36-aata-packet-web-application)
    - [3.7 Device management web application](#37-device-management-web-application)
    - [3.8 Azure Active Directory](#38-azure-active-directory)
    - [3.9 Azure Run book](#39-azure-run-book)
    - [3.10 Cosmos DB](#310-cosmos-db)
    - [3.11 Log analytics](#311-log-analytics)
    - [3.12 Application Insights](#312-application-insights)
- [4 Solution Deployment Type & Cost](4#-solution-deployment-type-&-cost)
    - [4.1 Basic Costing Details](#41-basic-costing-details)
    - [4.2 Standard Costing details](#42-standard-costing-details)
    - [4.3 Premium Costing details](#43-premium-costing-details)
- [5 What are paired regions?](#5-what-are-paired-regions?)
- [6 Prerequisites for Deploying ARM Template](#6-prerequisites-for-deploying-arm-template)
    - [6.1 Integrating applications with Azure Active Directory](#61-integrating-applications-with-azure-active-directory)
        - [6.1.1 To register a new application using the Azure portal](#611-to-register-a-new-application-using-the-azure-portal)
        - [6.1.2 To add application credentials or permissions to access web APIs](#612-to-add-application-credentials-or-permissions-to-access-web-apis)
        - [6.1.3 To get Tenant ID](#613-to-get-tenant-id)
        - [6.1.4 To get application ID and authentication key](#614-to-get-application-id-and-authentication-key)
        - [6.1.5 Creating session ID](#615-creating-sessionid)  
- [7 ARM Template Input Parameters](#7-arm-template-input-parameters)
- [8 Getting Started](#8-getting-started)
    - [8.1 ARM Template Deployment Using Azure Portal](#81-arm-template-deployment-using-azure-portal)
        - [8.1.1 Inputs](#811-inputs)
        - [8.1.2 Outputs](#812-outputs)
    - [8.2 ARM Template Deployment Using Azure CLI](#82-arm-arm-template-deployment-using-azure-cli)
        - [8.2.1 Create Resource Group for oilandgas Solution](#821-create-resource-group-for-oilandgas-solution)
        - [8.2.2 Execute the Template Deployment](#822-execute-the-template-dDeployment)
- [9 Post Deployment Steps: Getting started with Azure Sphere](#9-post-deployment-steps-getting-started-with-azure-sphere)
    - [9.1 Set Up your Machine for Development Machine](#91-set-up-your-machine-for-development-machine)
        - [9.1.1 Connect the RDB](#911-connect-the-rdb)
        - [9.1.2 Install the TAP Driver](#912-install-the-tap-driver)
        - [9.1.3 Configure TAP Networking](#913-configure-tap-networking)
    - [9.2 ARM Install Visual Studio](#92-install-visual-studio)
    - [9.3 Install the Visual Studio Tools Preview for Azure Sphere](#93-install-the-visual-studio-tools-preview-for-azure-sphere)
        - [9.3.1 To install the Visual Studio Tools Preview for Azure Sphere](#931-to-install-the-visual-studio-tools-preview-for-azure-sphere)
    - [9.4 Update Device Software](#94-update-device-software)
        - [9.4.1 To Check the Software Version on your Device](#941-to-check-the-software-version-on-your-device)
    - [9.5 Claim your Device](#95-claim-your-device)
    - [9.6 Add to Device Group](#96-add-to-device-group)
    - [9.7 Configure Wi-Fi](#97-configure-wi-fi)
        - [9.7.1 To Set up Wi-Fi on your Device](#971-to-set-up-wi-fi-on-your-device)
    - [9.8 Connect to IoT Hub](#98-connect-to-iot-hub)
        - [9.8.1 Set Up Microsoft Azure Credentials](#981-set-up-microsoft-azure-credentials)
        - [9.8.2 Add your Device to the IoT Hub](#982-add-your-device-to-the-iot-hub)
    - [9.9 Run the Blink Applications](#99-run-the-blink-applications)
- [10 Verify data in IoT Hub Device Twin](#10-verify-data-in-iot-hub-device-twin)
- [11 Verify Device Management application](#11-verify-device-management-application)
- [12 Monitoring Components](#12-monitoring-components)
    - [12.1 Application Insights](#121-application-insights)
    - [12.2 OMS Log Analytics](#12.2-oms-log-analytics)
- [13 Performing DR Strategies](#13-performing-dr-strategies)
    - [13.1 Standard Solution Type](#131-standard-solution-type)
        - [13.1.1 IoT Hub Manual Failover](#1311-iot-hub-manual-failover)
        - [13.1.2 Stop Stream Analytics Job in Primary Region](#1312-stop-stream-analytics-job-in-primary-region)
        - [13.1.3 Accessing Traffic manager](#1313-accessing-traffic-manager)
        - [13.1.4 Stopping the Web App](#1314-stopping-the-web-app)
        - [13.1.5 Redeploy the Region 2 ARM Template](#1315-redeploy-the-region-2-arm-template)
        - [13.1.6 Configure Region 2 web app to Traffic Manager](#1316-configure-region-2-web-app-to-traffic-manager)
        - [13.1.7 Cosmos DB Geo replication](#1317-cosmos-db-geo-replication)
        - [13.1.8 Accessing Web App](#1318-accessing-web-app)
    - [13.2 Premium Solution Type](#132-premium-solution-type)
        - [13.2.1 IoT Hub Manual Failover](#1321-iot-hub-manual-failover)
        - [13.2.2 Stop Stream Analytics Job in Primary Region](#1322-stop-stream-analytics-job-in-primary-region)
        - [13.2.3 Accessing Traffic manager](#1323-accessing-traffic-manager)
        - [13.2.4 Stopping the Web App](#1324-stopping-the-web-app)
        - [13.2.5 Cosmos DB Geo replication](#1325-cosmos-db-geo-replication)
        - [13.2.6 Accessing Web App](#1326-accessing-web-app)
             
## 1 About White Goods Solution

In this Solution we are using Azure Sphere Device (MT3620) which is connected to Device Management Web Application. The Cloud to device message sending is done, whenever user select the appropriate blink rate feature on device management application depend upon that the azure sphere device’s led light will blink slowly or fast.

Similarly, Device to cloud communication is done, whenever on Azure Sphere Button A is pressed the LED light blink rate will change and send the blink rate feature update message to Cloud i.e., Device Management application.

## 2 Solution Architecture Diagram and data work flow Diagram

### 2.1 Basic Architecture Diagram

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/1.png)

### 2.2	Standard Architecture Diagram

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/2.png)

### 2.3 Premium Architecture Diagram

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/3.png)

### 2.4 Data work Flow Diagram

Below is the architecture diagram that shows data flow in between azure components.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/4.png)

## 3 Azure Services

### 3.1 IoT Hub

Azure IoT Hub is Microsoft’s Internet of Things connector to the cloud. It’s a fully managed cloud service that enables reliable and secure bi-directional communications between millions of IoT devices and a solution back end. Device-to-cloud telemetry data tells you about the state of your devices and assets. Cloud-to-device messages let you send commands and notifications to your connected devices. Device messages are sent in a durable way to accommodate intermittently connected devices.

4X4 device generates data and will be send to IoT Hub.

### 3.2 Steam Analytics

Stream Analytics is an event processing engine that can ingest events in real-time, whether from one data stream or multiple streams. Events can come from sensors, applications, devices, operational   systems, websites, and a variety of other sources. Stream Analytics provides high-throughput, low- latency processing, while supporting real-time stream computation operations.

In this Solution, Stream analytics pulls data from IoT Hub and send the exact data to CosmosDB’s Messages Collection 

### 3.3 4x4 device 

4x4 device is a physical data simulator, which is used to send message to IoT HUB, the message should be in JSON format.

4X4 device is used to generate simulated data which will be sent to IoT Hub.

### 3.4 Notification Hub

Azure Notification Hubs are push notification software engines designed to alert users about new content for a given site, service or app. Azure Notification Hubs are part of Microsoft Azure’s public cloud service offerings.

Notification hub is used to send notifications to Device Management web application whenever any event happened beyond defined metrics.

### 3.5 Web Application  

A Web application (Web app) is an application program that is stored on a remote server and delivered over the Internet through a browser interface.

In Solution we have two web applications in one app service plan. 

* Data packet web application
* Device management web application

### 3.6 Data packet web application

Data Packet web application will be useful to define data format for a particular device and create template for it.  We can define Customized data format for different types of Devices.

For the created template have simulator, which we are installing on system to which the 4x4 device is getting connected for sending the data to IoT Hub.

### 3.7 Device management web application

Device management web application is a dashboard where you can monitor devices. Provides in detailed insights for a particular device. We can view the Alert summary, Device Activation status and date, Shipment date.

We can also get to know device count and how many devices are in active and not active state. We can create Group to check the condition of the devices which we have created.

### 3.8 Azure Active Directory
  
Microsoft Azure Active Directory (Azure AD) is a cloud service that provides administrators with the ability to manage end user identities and access privileges. The service gives administrators the freedom to choose which information will stay in the cloud, who can manage or use the information, what services or applications can access the information and which end users can have access.

Azure Active directory is used to authenticate users to login to Web Application. Azure active Directory enables secure authentications to web application.

### 3.9 Azure Run book

Azure Automation enables the users to automate the tasks, which are manual and repetitive in nature by using Runbooks. 

Runbooks in Azure Automation are based on Windows PowerShell or Windows PowerShell Workflow. We can code and implement the logic, which we want to automate, using PowerShell.

Azure run books are used to create collections in Document DB. And will be used to update reply URLs in Active Directory Application.
 
### 3.10 Cosmos DB  

Azure Cosmos DB is a Microsoft cloud database that supports multiple ways of storing and processing data. As such, it is classified as a multi-model database. In multi-model databases, various database engines are natively supported and accessible via common APIs.

In this Solution, Cosmos DB have Templates, Messages and Groups Collections.

The templates which we create for device in Data packet application will gets updated in the Templates collections.

Cosmos DB Stores the JSON format data received from Stream analytics in Messages Collections. 

The Groups collections will get updated once any group has been created in Device Management Application.

### 3.11 Log analytics

Log Analytics is a service in Operations Management Suite (OMS) that monitors your cloud and on-premises environments to maintain their availability and performance. It collects data generated by resources in your cloud and on-premises environments and from other monitoring tools to provide analysis across multiple sources.

OMS Log analytics are helpful to monitor SQL Database, Web Apps and Other Azure Components.

### 3.12 Application Insights

Application Insights is an extensible Application Performance Management (APM) service for web developers on multiple platforms. Use it to monitor live web application. It will automatically detect performance anomalies. It includes powerful analytics tools to help diagnose issues and to understand what users actually do with web application.

Application Insights monitor below:

•	Request rates, response times, and failure rates
•	Dependency rates, response times, and failure rates
•	Exceptions 
•	Page views and load performance
•	AJAX calls
•	User and session counts
•	Performance counters
•	Host diagnostics from Docker or Azure
•	Diagnostic trace logs
•	Custom events and metrics

## 4 Solution Deployment Type & Cost

Deployment types will be helpful to deploy azure resources with minimum inputs from the user. User can choose different deployment based on requirements. Deployment types helpful to deploy azure resources with minimal knowledge on Azure. We have three types of costing solutions we have in this project. 

* Basic 
* Standard
* Premium

**Basic solution**

This Deployment type deploys minimum azure resources required to deploy the Solution with minimal available SKU. These solutions deploy only azure resources required excluding monitoring and high available components.

**Standard Solution**

This deployment type deploys the azure resources along with monitoring azure resources such as application Insights and OMS Log analytics. 

In this deployment method, azure resources will be deployed with minimal SKU and pre- enabled monitoring Configuration.

**Premium Solution**

This Deployment type deploys minimum azure resources required to run the Solution which allows to monitor the solution also provides high availability for webapi and SQL DB and including optional Components.

Below are the Costing details of each deployment type which explains SKU and Costing price details including optional components.

### 4.1 Basic Costing Details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | Basic Tier; 1 B1 (1 Core(s), 1.75 GB RAM, 10 GB Storage)      | PAYG         | $54.75 
| **Cosmos DB**   | Standard, throughput 1000 RU/s (Request Units per second) 10x100 Rus(Throughput)- $58.40 10 GB storage – $2.50   | PAYG  | $60.90 
| **IoT HUB**        | B1, Unlimited devices, 1 Unit-$10.00   400,000 messages/day                     | PAYG                       | $10.00   
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US       | PAYG                          | $2.30  
| **Azure Automation Account**        | Free      | PAYG                       | $0.00   
| **Notification Hub**       | Free                          | PAYG                      | $0.00 
| **Application Insight**       | Basic, 1GB * $2.30 Region: East US first 5GB free per month       | PAYG                  | $2.30 
| **Stream Analytics**   | Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US       | PAYG  | $80.30 
|                     |                       | **Estimated monthly cost**          | **$210.55**

### 4.2 Standard Costing details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | S2:2 Cores(s), 3.5 GB RAM, 50GB Storage, US$ 0.200, 1 Instance      | PAYG         | $146.00  
| **Cosmos DB**   | Standard, throughput 1000 RU/s (Request Units per second) 10x100 Rus(Throughput)- $58.40 10 GB storage – $2.50    | PAYG  | $60.90  
| **IoT HUB**        | S1, Unlimited devices, 1 Unit-$25.00/per month 400,000 messages/day           | PAYG                       | $25.00    
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US      | PAYG                          | $2.30   
| **Azure Automation Account**        | 2*Free      | PAYG                       | $0.00   
| **Notification Hub**       | 2*Free                          | PAYG                      | $0.00 
| **Application Insight**       | 2*Basic, 1GB * $2.30 Region: East US first 5GB free per month       | PAYG                  | $4.60 
| **Stream Analytics**   | 2 * Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US        | PAYG  | $160.60  
|                     |                       | **Estimated monthly cost**          | **$399.40**
           
### 4.3 Premium Costing details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | S2:2 Cores(s), 3.5 GB RAM, 50GB Storage, US$ 0.200, 1 Instance       | PAYG         | $146.0  
| **Cosmos DB**   | Standard, throughput 1000 RU/s (Request Units per second) 10x100 Rus(Throughput)- $58.40 10 GB storage – $2.50     | PAYG  | $60.90  
| **IoT HUB**        | S1, Unlimited devices, 1 Unit-$25.00/per month 400,000 messages/day         | PAYG                       | $25.00    
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US       | PAYG                   | $2.30  
| **Azure Automation Account**        | 2*Free      | PAYG                       | $0.00   
| **Notification Hub**       | 2*Free                          | PAYG                      | $0.00 
| **Application Insight**       | 2 * Basic, 1GB * $2.30 Region: East US first 5GB free per month        | PAYG                  | $4.60 
| **Stream Analytics**   | 2 * Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US         | PAYG  | $160.60  
|                     |                       | **Estimated monthly cost**          | **$549.40**
                     
## 5 What are paired regions? 

Azure operates in multiple geographies around the world. An Azure geography is a defined area of the world that contains at least one Azure Region. An Azure region is an area within a geography, containing one or more datacenters. 

Each Azure region is paired with another region within the same geography, together making a regional pair. The exception is Brazil South, which is paired with a region outside its geography. 

**IoT Hub Manual Failover Support Geo-Paired Regions**

| **S.No**           | **Geography**           | **Paired Regions**                 | **Paired Regions**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **1**       | North America       | East US 2       | Central US
| **2**   | North America    | Central US  | East US 2  
| **3**        | North America      | West US 2                       | West Central US   
| **4**      | North America      | West Central US                 | West US 2 
| **5**    | Canada   | Canada Central       | Canada East
| **6**        | Canada      | Canada East                       | Canada Central   
| **7**       | Australia                          | Australia East                     | Australia South East
| **8**       | Australia        | Australia South East                  | Australia East 
| **9**   | India    | Central India  | South India  
| **10**       | India      | South India       | Central India
| **11**   | Asia    | East Asia  | South East Asia
| **12**        | Asia      | South East Asia                       | East Asia   
| **13**      | Japan      | Japan West                 | Japan East
| **14**    | Japan  | Japan East       | Japan West
| **15**        | Korea      | Korea Central                       | Canada Central   
| **16**       | Korea                         | Korea South                     | Korea Central
| **17**       | UK        | UK South                  | UK West 
| **18**   | UK    | UK West  | UK South  

## 6 Prerequisites for Deploying ARM Template

Create an application in Azure Active Directory.

### 6.1 Integrating applications with Azure Active Directory

Any application that wants to use the capabilities of Azure AD must first be registered in an Azure AD tenant. This registration process involves giving Azure AD details about your application, such as the URL where it’s located, the URL to send replies after a user is authenticated, the URI that identifies the app, and so on.

#### 6.1.1 To register a new application using the Azure portal

1. Sign in to the Azure portal.

2. In the left-hand navigation pane, click the Azure Active Directory service, click App registrations, and click New application registration.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/5.png)

3. When the Create page appears, enter your application's registration information:

* Name: Enter the application name
* Application type:
      - Select "Native" for client applications that are installed locally on a device. This setting is used for OAuth public native clients.
      - Select "Web app / API" for client applications and resource/API applications that are installed on a secure server. This setting is used for OAuth confidential web clients and public user-agent-based clients. The same application can also expose both a client and resource/API.
* Sign-On URL: For "Web app / API" applications, provide the base URL of your app. For example, https://localhost might be the URL for a web app running on your local machine. Users would use this URL to sign in to a web client application.

4. When finished, click Create.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/6.png)

#### 6.1.2 To add application credentials, or permissions to access web APIs

1. Click the Azure Active Directory service, click App registrations, and then find/click the application you want to configure.

2. You are taken to the application's main registration page, which opens the Settings page for the application. To add a secret key for your web application's credentials:

3. Click the Keys section on the Settings page.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/7.png)

4. Add a description for your key and Select duration then click Save. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/8.png)

5. The right-most column will contain the key value, after you save the configuration changes. Be sure to copy the key for use in your client application code, as it is not accessible once you leave this page.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/9.png)

#### 6.1.3 To get Tenant ID

1. Select Azure Active Directory.

2. To get the tenant ID, select Properties for your Azure AD tenant and Copy the Directory ID. This value is your tenant ID.

3. Note down the Copied Directory ID which is highlighted in the below figure, this will be used while deploying the ARM template.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/10.png)

#### 6.1.4 To get application ID and authentication key

1. From App registrations in Azure Active Directory, select your application.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/11.png)

2. Copy the Application ID and object ID. The application ID value is referred as the client ID.

3. Note down the Copied Application ID and object ID which is highlighted in the below figure, this will be used while deploying the ARM template.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/12.png)

#### 6.1.5 Creating session ID

1. Use the below URL to generate GUID.

    **https://www.guidgenerator.com/**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/13.png)

2. Click Generate some GUIDs! This will generate GUID in Results box. 

3. Copy and Note down the generated GUID which is highlighted in the below figure, this will be used while deploying the ARM template.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/14.png)

## 7 ARM Template Input Parameters

In the parameters section of the template, specify the values as inputs when deploying the ARM Template. These parameter values enable you to customize the deployment by providing values that are tailored for your environment (such as dev, Staging and production). 

| **Parameter Name**  | **Description**     | **Allowed Values**    | **Default Values**                                                                                                               
| -------------        | -------------      | -----------------    | ------------ 
| **Solution Type**          | choose your solution type to deploy.     | Basic, Standard, Premium    | Core Solution
| **geo-paired-region**   | If solution type is chosen as standard and premium, enter location for geo-paired-region.  | EastUS2, CentralUS,WestUS2, WestCentralUS, CanadaCentral, CanadaEast, AustraliaEast, AustraliaSouthEast, CentralIndia, SouthIndia, EastAsia, SouthEastAsia,JapanWest, JapanEast, KoreaCentral, KoreaSouth, UKSouth, UKWest  | 
| **omsWorkspaceRegion**   | Choose location for OMS Log Analytics to deploy. For more information, see https://docs.microsoft.com/en-us/azure/log-analytics/log-analytics-overview.   | australiasoutheast, canadacentral, centralindia, eastus, japaneast, southeastasia, uksouth, westeurope    | eastus
| **appInsightsLocation**   | specify the region for application insights.  | eastus, northeurope, southcentralus, southeastasia, westeurope,westus2    | eastus
| **appInsightsLocationDr**    | If solution type is chosen as premium, enter location for application insights for disaster recovery. | eastus, northeurope, southcentralus, southeastasia, westeurope, westus2    | westus2
| **tenantId**   | Tenant Id of the created Azure active directory application. For instructions, see https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-howto-tenant in the Microsoft documentation   |  | 
| **clientId**  | Application ID of the created Azure active directory application. For information, see https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-integrating-applications in the Microsoft documentation.   |     | 
| **objectId**  | Object Id of the created Azure active directory application.  |     | 
| **clientSecret**  | clientSecret of the created Azure active directory application.For instructions, see https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal#get-application-id-and-authentication-key in the Microsoft documentation.  |   | 
| **azureAccountName**   | azure portal login username.       |    | 
| **azurePassword** | azure portal login password.       |  | 
| **sessionId** | Refer 5.1.5 section for getting provide the session ID.  |   |  
  
## 8 Getting started

Azure Resource Manager allows you to provision your applications using a declarative template. In a single template, you can deploy multiple services along with their dependencies. The template consists of JSON and expressions that you can use to construct values for your deployment. You use the same template to repeatedly deploy your application during every stage of the application lifecycle.

Resource Manager provides a consistent management layer to perform tasks through Azure PowerShell, Azure CLI, Azure portal, REST API, and client SDKs.

Resource manager provides the following feature:

•	Deploy, manage, and monitor all the resources for your solution as a group, rather than handling these resources individually.
•	Repeatedly deploy your solution through the development lifecycle and your resources are deployed in a consistent state.
•	Manage your infrastructure through declarative templates rather than scripts.
•	Define the dependencies between resources so they're deployed in the correct order.
•	Apply access control to all services in your resource group because Role-Based Access Control (RBAC) is natively integrated into the management platform.
•	Apply tags to resources to logically organize all the resources in your subscription.

### 8.1. ARM Template Deployment Using Azure Portal

1. Click the below Git hub repo URL.

**https://github.com/sysgain/whitegoods/tree/costing-models**

2. Select **main-template.json** from **master** branch as shown in the following figure.
 
![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/15.png)

3. Select **Raw** from the top right corner.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/16.png)

4. **Copy** the raw template and **paste** in your azure portal for template deployment.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/17.png)

To deploy a template for Azure Resource Manager, follow the below steps.

1.	Go to **Azure portal**.

2.	Navigate to **Create a resource (+)**, search for **Template deployment**.

3.	Click **Create** button and click **Build your own Template in the editor** as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/18.png)

4.	The **Edit template** page is displayed as shown in the following figure. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/19.png)

5.	**Replace / paste** the template and click **Save** button.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/20.png)

6.	The **Custom deployment** page is displayed as shown in the following.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/21.png)
![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/22.png)

#### 8.1.1 Inputs

These parameter values enable you to customize the deployment by providing values. There parameters allow to choose the solution type, region and credentials to authenticate SQL Database and Virtual Machines.

**Parameters for Basic Solution:**

7. If you want to deploy the Basic Solution you have to enter the below parameters.

Note: For basic solution, it is not necessary to give the values of OMS workspace region, app insights location, so keep the default values as it is.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/23.png)

**Parameters for Standard Solution:**

8. If you want to deploy the standard solution you have to enter the below parameters.

Note: For standard solution, select the geo-paired region for your template deployment.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/24.png)

**Parameters for Premium solution:**

9. If you want to deploy the Premium solution you have to enter the below parameters.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/25.png)

10. Once all the parameters are entered, select the **terms and conditions** check box and click **Purchase**.

11. After the successful deployment of the ARM template, the following **resources** are created in a **Resource Group**.

•	4 app services
•	1 application Insights
•	1 automation account
•	1 run book
•	1 azure cosmos DB
•	1 IoT Hub
•	1 log analytics
•	1 Notification Hub
•	1 Stream analytics job
•	Traffic manager

12. Once the solution is deployed successfully navigate to the resource group, select the created resource group to view the list of resources that are created in the Resource Group as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/26.png)
![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/27.png)

#### 8.1.2 Outputs

13. Go to **Resource group** -> click **deployments**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/28.png)

14. Click **Microsoft Template**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/29.png)

15. Click **outputs**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/30.png)

### 8.2 ARM Template Deployment Using Azure CLI

AAzure CLI is used to deploy your resources to Azure. The Resource Manager template you deploy, can either be a local file on your machine, or an external file that is in a repository like GitHub.  

Azure Cloud Shell is an interactive, browser-accessible shell for managing Azure resources. Cloud Shell enables access to a browser-based command-line experience built with Azure management tasks in mind. 

Deployment can proceed within the Azure Portal via Windows PowerShell.  

1. Customize main-template.parameters.json file.

2. Click on Windows PowerShell, run the following commands.

 **az login**

3. It will be showing device login URL and code to authenticate the device login.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/31.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/32.png)

4. Past the code and click Continue.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/33.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/34.png)

5. Paste your **main-template.parameters.json** in editor. 

6. Update the following parameters in **main-template.json** file 

* Solution Type 
* geo-paired-region
* omsWorkspaceRegion
* appInsightsLocation
* appInsightsLocationDr
* tenantId
* clientId
* objectId
* clientSecret
* azureAccountName
* azurePassword
* sessionId

#### 8.2.1 Create Resource Group for White Goods Solution 

Use the **az group create** command to create a **Resource Group** in your region, e.g.:

**Description:** To create a resource group, use **az group create** command, It uses the name parameter to specify the name for resource group (-n) and location parameter to specify the location (-l). 

**Syntax**: **az group create -n <resource group name> -l <location>** 

Ex: **az group create -n <****> -l <***>** 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/35.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/36.png)

#### 8.2.2 Execute the template deployment 

Use the **az group deployment create** command to deploy the ARM template.

**Description**: To deploy the ARM Template, you require two files: 

1. **main-template.json** – contains the resource & its dependency resources to be provisioned from the ARM template.

2. **main-template.parameters.json** –contains the input values that are required to provision respective SKU & Others details, for more details on the input parameter values navigate to Section 6 of this document. 

**Syntax**:  **az group deployment create --template-file './<main-template.json filename>' --parameters '@./<main-template.parameters.json filename>' -g < provide resource group name that created in the section 6.2.2> -n deploy >> <provide the outputs filename>** 

Ex: **az group deployment create --template-file './main-template.json' --parameters '@./main-template.parameters.json' -g oilandgas-iot -n deploy >> outputs.txt** 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/37.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/38.png)

Deployment may take between 15-20 minutes depending on deployment size. 

After successful deployment you can see the deployment outputs as follows.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/39.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/40.png)

## 9. Post Deployment steps: Getting started with Azure Sphere

### 9.1 Set Up your Machine for Development Machine

To connect to a reference development board (RDB), your development machine requires the following: 

•	Windows 10 Anniversary Update or later 
•	Support for the Visual Studio 2017 System Requirements 
•	A USB Port

#### 9.1.1 Connect the RDB

The RDB connects to a PC through a USB micro-connector. When plugged in, the RDB exposes three COM ports. 

The first time you plug in the board, the drivers should be automatically downloaded and installed. Installation can be slow; if the drivers are not installed automatically, right-click on the device name in Device Manager and select Update driver.

Click on search Device Manager in Windows Search Box to verify installation, open Device Manager and look for three COM ports:

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/41.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/42.png)

#### 9.1.2 Install the TAP Driver

The development board communicates with the PC over serial line internet protocol (SLIP). Tap-Windows provides a network interface driver for SLIP.

To install TAP and enable SLIP Communication

1. Install TAP-Windows, which came with your Azure Sphere software development kit (SDK). 

2. In the installation options, choose TAP Virtual Ethernet Adapter and TAP Utilities, but not TAP SDK.

3. Download the tap-windows-9.21.2.exe file from below link and Click on install tap-windows-9.21.2.exe 

**url: https://projectiot.blob.core.windows.net/whitegoods/Documents/CustomerDropTP4.0.1.zip**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/43.png)

4. Click on Next.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/44.png)

5. Click I Agree.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/45.png)

6. Choose TAP Virtual Ethernet Adapter and TAP Utilities, but not TAP SDK and click Next.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/46.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/47.png)

7.	If you are asked to authorize installation of the driver, select Install.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/48.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/49.png)

#### 9.1.3 Configure TAP Networking

1.	After you install the driver, configure TAP networking for the board.

2.	In Control Panel, search View Network Connections in the search box.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/50.png)

3.	And click to open the View Network Connections. Find the TAP-Windows Adapter V9.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/51.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/52.png)

4.	Select TAP-Windows Adapter v9 and rename it to sl0 (lower case S, lower case L, the number zero):

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/53.png)

5.	Open Properties for sl0 and disable all services except Internet Protocol Version 4 (TCP/IPv4):

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/54.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/55.png)

6.	Select Properties for TCP/IPv4 and configure it to use the IP address 192.168.35.1, subnet mask 255.255.255.0 and click OK.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/56.png)

7.	Now you can see the sl0 is enabled.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/57.png)

### 9.2 Install Visual Studio 

The Visual Studio Tools for Azure Sphere require Visual Studio Enterprise, Professional, or Community 2017 version 15.3 or later. To verify which version is installed, start the Visual Studio Installer and make sure that the version number is 15.3.0 or later. If the installer prompts you to update the Visual Studio Installer.

If you have installed an earlier version of Visual Studio 2017 15.3.0 (preview 4 or later) from the Visual Studio Preview channel, you can continue to use that version. However, use of the Preview channel is no longer required; you can use the current version from the regular Visual Studio website. 

To install Visual Studio, click Download Visual Studio, select the edition to install, and then run the installer. You can choose to install any workloads, or none. The Visual Studio Tools for Azure Sphere installation procedure automatically installs the workloads that the SDK requires.

### 9.3 Install the Visual Studio Tools Preview for Azure Sphere

The Visual Studio Tools Preview for Azure Sphere includes: 

•	A custom Azure Sphere Developer Command Prompt, which is available in the Start menu under Azure Sphere 
•	The GDB debugger for use with the Azure Sphere development board 
•	Device, cloud, and image utilities 
•	Libraries for application development 
•	Visual Studio extensions to support Azure Sphere development 

Azure_Sphere_VS_Dev_Tools_Preview.exe installs the complete Azure Sphere software development kit (SDK).

#### 9.3.1 To install the Visual Studio Tools Preview for Azure Sphere

1. Download VS_Tools_Preview_for_Azure_Sphere.exe from below link and run it.

**Url: : https://projectiot.blob.core.windows.net/whitegoods/Documents/CustomerDropTP4.0.1.zip**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/58.png)

2. To install the developer tools. Agree to the license terms and select Install.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/59.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/60.png)

3. If you have just installed Visual Studio for the first time, you might see the message, "No product to install SDK on." If this occurs, restart your PC and return to this step.

4. In the VSIX Installer window, confirm the Visual Studio version(s) for which to install the tools.

5. Accept the elevation prompt.

6. After installation starts, find the VSIX Installer window and bring it to the front. The installation process displays two installation windows: The Visual Studio Tools Preview for Azure Sphere window and the VSIX Installer window. The former reports progress and errors from the overall installation, and the latter reports information about the Visual Studio extension only. If the VSIX window becomes obscured during installation, you might not see error reports or requests for action.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/61.png)

7.	When setup completes, select Close in both the VSIX Installer window and the Visual Studio Tools Preview for Azure Sphere setup window.

8.	If the installer returns errors, try uninstalling and then reinstalling the tools. To uninstall the tools, use Add and Remove Programs in Control Panel.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/62.png)

### 9.4 Update Device Software

#### 9.4.1 To Check the Software Version on your Device

1. Open an Azure Sphere Developer Command Prompt. To find the Azure Sphere Developer Command Prompt, click the Start button and type Azure Sphere. 

2. Issue the following command: 

**dutil device sdkversion** 

**SDK version: 4.0.0+107652** 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/63.png)

3.	The number that follows the + should be 107652 or greater. If you see a smaller number, you must update the software by following the instructions in this document. 

### 9.5 Claim your Device

After you install Visual Studio and the SDK, you must claim your device. Claim your device only once. 

Every device has a unique and immutable Azure Sphere device ID that the Azure Sphere security service uses to identify and authenticate it. Before you update the device software or develop applications for the device, you must claim the device. Claiming the device associates it with your Azure Sphere tenant.

Refer  **HowToCreateATenant.docx** for information about creating a tenant.

Before you claim a device, be sure that you are logged in to the tenant that you plan to use with Azure Sphere services.

Refer the cutil login command in the Command-line Utilities  for more information.

**To claim your device:**

1. Connect your board to the PC by USB. 

2. Open an Azure Sphere Developer Command Prompt. To find the Azure Sphere Developer Command Prompt, click the Start button and type Azure Sphere.

3. Run the cutil cloud utility with the device command as follows:

**cutil device claim --attached**

4.	This command reads the Azure Sphere device ID from the board and associates it with your current tenant. If you are prompted to log in to Microsoft Azure, do so using your Azure Sphere credentials.

### 9.6 Add to Device Group

Before you configure Wi-Fi on your device, you must add it to the Microsoft-created System Software device group. The group is named System Software and has the following device group ID:

**63bbe6ea-14be-4d1a-a6e7-03591d882b42**

Use the following command to add your device to the group:

**Cutil device setdg –attached –devicegroupid  63bbe6ea-14be-4d1a-a6e7-03591d882b42**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/64.png)

### 9.7 Configure Wi-Fi

You must configure the device for Wi-Fi before it can communicate with the Azure IoT Hub or receive over-the-air (OTA) updates.

#### 9.7.1 To Set up Wi-Fi on your Device

1.	If your device is not connected to your PC, connect it now.

2.	Open an Azure Sphere Developer Command Prompt and issue a command in the following form:

**dutil wifi add –s SYSG-SEEC3 -k XXXXXXXXX**

3.	The -s flag specifies the network SSID, and the -k flag specifies the WPA2 key. Network SSIDs are case-sensitive and can include only ASCII characters. To add an open Wi-Fi access point, omit the -k flag.

4.	You should see:

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/65.png)

### 9.8 Connect to IoT Hub

#### 9.8.1 Set Up Microsoft Azure Credentials

To run this sample, you must have a Microsoft Azure subscription and an IoT Hub. 

After you set up the subscription, you can create a hub. Log into the Azure Portal and follow these instructions to set up your hub.

#### 9.8.2 Add your Device to the IoT Hub

To use an IoT Hub in an Azure Sphere application, you identify the IoT Hub that you plan to use and then add your device to that hub. The connected service retrieves the IoT Hub connection string and records it in a file named azure_iot_hub.c. The connected service then adds azure_iot_hub.c and the header file azure_iot_hub.h to your application.

Download **Mt3620AzureIoTHub4.zip** and extract it.

Click on open **Mt3620AzureIoTHub4.sln** in your local system.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/66.png)

In Solution Explorer, right-click References and then select Add Connected Service.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/67.png)

It will open Connected Services window, click on find more services.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/68.png)

In the search box search Azure IoT Hub Sample of Device Simulation and click on Download and Install.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/69.png)

Installing connected service for azure IoT hub.

![alt text](hhttps://github.com/sysgain/whitegoods/raw/master/Images/70.png)

After modification completed, click on Close.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/71.png)

Once the Azure IoT Hub Sample of Device Simulation complete close and re-open the visual studio for existing solution. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/72.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/73.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/74.png)

Select Azure IoT Hub (Azure Sphere) from the list of connected services.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/75.png)

Log into Microsoft Azure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/76.png)

Enter the user credentials.

Note: You must have a Microsoft Azure subscription. If your organization does not already have them, follow these instructions to set up a free trial subscription to Microsoft Azure. After you set up the subscription. Log into the Azure Portal and follow these instructions to login.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/77.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/78.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/79.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/80.png)

Select Hardcode shared access key in application's code, and then click Next.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/81.png)

In the list of hubs, select deployed IoT Hub from your resource group.

Here we are selecting IoTHubWGck4qt and click Next.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/82.png)

If your device does not appear in the list, open the New Device tab. In the Device ID field, type a string that you will use to identify your device in the IoT Hub, and click Create. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/83.png)

Copy the device primary connection string and then click finish.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/84.png)

### 9.9 Run the Blink Applications 

In Blink app Solution Explorer, select azure_iot_hub.c in your solution and update the IoT Hub connection string which was copied from above step and save it.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/85.png)

Now open the app_manifest.json file and update the IoT Hub Host name.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/86.png)

Now you can build the application and use the Azure IoT Hub. In this walkthrough, we will use existing IoT Hub tools to monitor and communicate with your device.

Now click on Remote GDB Debugger and click on Yes to build the Blink App.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/87.png)

The output in the below screen shot shows that the build has been completed successfully and the Wi-Fi has been connected.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/88.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/89.png)

Once the application is running the output also shows the IoT Hub connected Status and Report Status of the device as below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/90.png)

## 10 Verify data in IoT Hub Device Twin

Go to **Resource Group** -> Click on **IoT Hub**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/100.png)

Click on **IoT devices** in left side menu and click **vivektest1**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/101.png)

Here you can see the **Device Details** page. Click on **Device Twin**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/102.png)

Here you can see the **device twin** data.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/103.png)

## 11 Verify Device Management application 

Go to **Resource Group** -> click **Device Management**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/104.png)

After running the blink App the connected Status of the device will get reflected in the device management web app.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/105.png)

Click on **connected**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/106.png)

The features of the blink app show disabled. Similarly the same status can be seen by Clicking on **View JSON**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/107.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/108.png)

Click on **Activate** button on any of the blink app feature like **blinkrate1**. Once it is activated the same will be reflected in IoT Hub’s device twin.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/109.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/110.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/111.png)

After selecting blink rate1 feature we can see LED light blink slowly on Azure Sphere device and similarly if you select blink rate2 the LED light will blink 2-3 times in a second on Azure Sphere device.

The LED Light will continuously blink if you select blinkrate3.

Similarly, if you select blinkrate2 or blinkrate3 features of blink app appropriate features will get reflected in IoT Hub’s device twin.

## 12 Monitoring Components

### 12.1 Application Insights

Go to **Azure Portal**, select your **Resource Group** and select **Application Insights** as shown below. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/112.png)

On **Overview** page, Summary details are displayed as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/113.png)

Click on **Live Metric Stream**, which is available at left side menu to check the live requests of **Device Management Application**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/114.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/115.png)

Go back to the **Application Insights Overview** page in **Azure Portal** to view metrics of the application.

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

### 12.2 OMS Log Analytics

Open **Azure Portal** -> **Resource Group** -> Click the **OMS Workspace** in resource group to view **OMS Overview** Section. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/129.png)

Click **Azure Resources** on left side menu to view available Azure Resources. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/130.png)

Select your **Resource Group** name from the dropdown list.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/131.png)

Access OMS portal from OMS Workspace from the left side menu. Click on **OMS Workspace** > **OMS Portal**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/132.png)

Once you click **OMS Workspace**, OMS Home Page is displayed. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/133.png)

Click Search tab to search the IoT hub, Stream Analytics, Cosmos DB. 

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

Select your workspace from the **OMS Workspaces** page as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/139.png)

Click **Save**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/140.png)

Go to **OMS Portal** and Click Search tab for **Stream Analytics logs**.

Copy **Stream Analytics Job** resource name, paste it in the **Converter** box and click **RUN**. 

The Stream Analytics Job information is displayed below the page as shown in the following figure.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/141.png) 

## 13 Performing DR Strategies

### 13.1 Standard Solution Type

In this scenario, there is again a primary and a secondary Azure region. All the traffic goes to the active deployment on the primary region. The secondary region is better prepared for disaster recovery because the database is running on both regions. 

Only the primary region has a deployed cloud service application. Both regions are synchronized with the contents of the database. When a disaster occurs, there are fewer activation requirements. You redeploy azure resources in the secondary region.

Redeployment approach, you should have already stored the service packages. However, you don’t incur most of the overhead that database restore operation requires, because the database is ready and running.  This saves a significant amount of time, making this an affordable DR pattern.

Standard Solution requires redeployment of azure resources in secondary region when there is primary region is down.

When user chooses Standard Solution type below azure resources will be deployed in primary region and SQL in both Regions.

App service

•	Application insights
•	Automation account
•	Azure cosmos DB
•	IoT Hub
•	Log analytics
•	Notification Hub
•	Stream analytics job
•	Traffic manager profile.

When there is primary region is down, and user needs to redeploy azure resources. When redeployment is completed below resources will be deployed.

•	App service
•	Application insights
•	Notification hub
•	Stream analytics Job

##### 13.1.1 IoT Hub Manual Failover

Go to **Resource Group** -> Click on **IoT Hub**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/142.png)

Go to **Manual Failover (Preview)** from left side menu.

Click on **Initiate failover** to initiate manual failover of IoT Hub.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/143.png)

When failover process started, a pop up will be displayed on right top corner. Once Manual Failover process completed, Primary Location and secondary location will interchange.

**Note**: This process will take around 15 mins. To initial failover again, user needs wait for 1 hour to run failover again.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/144.png)

#### 13.1.2 Stop Stream Analytics Job in Primary Region

Go to **Resource Group** and click on primary **Stream Analytics job**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/145.png)

Stop the Stream analytics job by click on **Stop** and click on **Yes** for confirmation.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/146.png)

#### 13.1.3 Accessing Traffic manager 

Go to **Resource Group** and click on **Traffic manager** resource.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/147.png)

Now you can see the web app as the endpoint of the traffic manager.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/148.png)

#### 13.1.4 Stopping the Web App

Navigate to the **Web App** from resource group.

click on **Stop** then click on **Yes** to stop the Web App.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/149.png)

The Web App in the primary region has been stopped as failover.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/150.png)

Verify the same in traffic manager.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/151.png)

#### 13.1.5 Redeploy the Region 2 ARM Template

Go to **Github** and select **re-deploy.json** file from the **master** branch.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/152.png)

Click on **Raw**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/153.png)

Take **copy** the Raw.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/154.png)

Click on **Add** in existing resource group and re-deploy the ARM template.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/155.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/156.png)

#### 13.1.6 Configure Region 2 web app to Traffic Manager

Go to **Resource Group** -> **Traffic manager profile** -> **Endpoints**, click on **add**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/157.png)

**Enter** the name for the **second End point** of traffic manager and select the second web app as the target resource.

Click on **Ok**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/158.png)

You can see the second region web app with online status under endpoint of traffic manager and **Copy** the **DNS Name**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/159.png)

#### 13.1.7 Cosmos DB Geo replication

Go to **Resource Group** -> click **Cosmos DB**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/160.png)

Navigate to **Replicate data globally** under Settings section then click **Manual failover**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/161.png)

**Select** the **Read Region** to become the **new write region**, check in the check box and click **ok**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/162.png)

#### 13.1.8 Accessing Web App

Go to **Resource Group** -> **Settings** -> **Deployments**, select **Microsoft Template**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/163.png)

Select **output** and **copy** the **devicemanagement_trafficmanager** url.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/164.png)

**paste** the copied URL in new browser to access the Web App using your credentials.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/165.png)

You can see the **Device summary** as shown below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/166.png)

### 13.2 Premium Solution Type

Both the primary region and the secondary region have a full deployment. This deployment includes the cloud services and a synchronized database. However, only the primary region is actively handling network requests. The secondary region becomes active only when the primary region experiences a service disruption. In that case, all new network requests route to the secondary region. Azure Traffic Manager can manage this failover automatically.

Failover occurs faster than the database-only variation because the services are already deployed. This topology provides a very low RTO. The secondary failover region must be ready to go immediately after failure of the primary region.

For the fastest response time with this model, you must have similar scale (number of role instances) in the primary and secondary regions. 

When user chooses premium as solution type below azure resource will be deployed in both regions.

•	2 app services
•	2 application insights
•	1 automation account
•	1 cosmos DB
•	1 IoT Hub
•	1 Log Analytics
•	2 Notification Hubs
•	2 Stream analytics job
•	1 Traffic manager

#### 13.2.1 IoT Hub Manual Failover

Go to **Resource Group** and Click on **IoT Hub**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/167.png)

Go to **Manual Failover(Preview)** from left side menu.

Click on **Initiate failover** to initiate manual failover of IoT Hub.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/168.png)

You can check the status of the failover.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/169.png)

When failover process started, a pop up will be displayed on right top corner. Once Manual Failover process completed, Primary Location and secondary location will interchange.

**Note**: This process will take around 15 mins. To initial failover again, user needs wait for 1 hour to run failover again.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/170.png)

#### 13.2.2 Stop Stream Analytics Job in Primary Region

Go to **Resource Group** and click on primary **Stream Analytics job**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/171.png)

Stop the Stream analytics job by click on **Stop** and click on **Yes** for confirmation.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/172.png)

You can see the stream analytics job has been stopped.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/173.png)

#### 13.2.3 Accessing the Traffic manager 

Go to **Resource Group** and click on **Traffic manager** resource.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/174.png)

Now you can see the web app as the endpoint of the traffic manager.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/175.png)

#### 13.2.4 Stopping the Web App

Navigate to the **Web App** from the resource group.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/176.png)

Select the **primary Web App** click on **Stop** then click on **Yes** to stop the Web App.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/177.png)

The Web App in the primary region has been stopped as failover.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/178.png)

Verify the Status of primary web app in traffic manager.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/179.png)

#### 13.2.5 Cosmos DB Geo replication

Go to **Resource Group** -> click **Cosmos DB**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/180.png)

Navigate to **Replicate data globally** under Settings section then click **Manual failover**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/181.png)

**Select** the **Read Region** to become the **new write region**, check in the check box and click **ok**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/182.png)

#### 13.2.6 Accessing Web App

Go to **Resource Group** -> **Settings** -> **Deployments**, select **Microsoft Template**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/183.png)

Select **output** and **copy** the **devicemanagement_trafficmanager** url.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/184.png)

**paste** the copied URL in new browser to access the Web App using your credentials.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/185.png)

You can see the **Device summary** as shown below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/186.png)


