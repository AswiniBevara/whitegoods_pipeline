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
- [4 Solution Deployment Type & Cost](4#-solution-deployment-type-&-cost)
    - [4.1 Basic Costing Details](#41-basic-costing-details)
    - [4.2 Standard Costing details](#42-standard-costing-details)
    - [4.3 Premium Costing details](#43-premium-costing-details)
- [5 What are paired regions?](#5-what-are-paired-regions?)
- [6 ARM Template Input Parameters](#6-arm-template-input-parameters)
- [7 Getting Started](#7-getting-started)
    - [7.1 ARM Template Deployment Using Azure Portal](#71-arm-template-deployment-using-azure-portal)
        - [7.1.1 Inputs](#711-inputs)
        - [7.1.2 Outputs](#712-outputs)
    - [7.2 ARM Template Deployment Using Azure CLI](#72-arm-arm-template-deployment-using-azure-cli)
        - [7.2.1 Customize main-template.parameters.json file](#721-customize-main-template.parameters.json-file)
        - [7.2.2 Create Resource Group for oil and gas solution](#722-create-resource-group-for-oil-and-gas-solution)
        - [7.2.3 Execute the template deployment](#723-execute-the-template-deployment)

## 1 About White Goods Solution

In this Solution we are using Azure Sphere Device (MT3620) which is connected to Device Management Web Application. The Cloud to device message sending is done, whenever user select the appropriate blink rate feature on device management application depend upon that the azure sphere device’s led light will blink slowly or fast.

Similarly, Device to cloud communication is done, whenever on Azure Sphere Button A is pressed the LED light blink rate will change and send the blink rate feature update message to Cloud i.e., Device Management application.

## 2 Solution Architecture Diagram and data work flow Diagram

### 2.1 Basic Architecture Diagram

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/1.jpg)

### 2.2	Standard Architecture Diagram

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/2.jpg)

### 2.3 Premium Architecture Diagram

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/Hardening%20Architecture%20Diagram-colored(23-07-18).jpg)
![#33FF33](https://placehold.it/15/33FF33/000000?text=+) `Automation Resources`
![#FF66FF](https://placehold.it/15/FF66FF/000000?text=+) `HA/DR Resources`
![#FFB570](https://placehold.it/15/FFB570/000000?text=+) `Monitoring Resources`
![#FFFF33](https://placehold.it/15/FFFF33/000000?text=+) `MachineLearning Resources`
![#CC6600](https://placehold.it/15/CC6600/000000?text=+) `Optional Resources`

### 2.4 Data work Flow Diagram

Below is the architecture diagram that shows data flow in between azure components.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/4.jpg)

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

* The templates which we create for device in Data packet application will gets updated in the Templates collections.

* Cosmos DB Stores the JSON format data received from Stream analytics in Messages Collections. 

* The Groups collections will get updated once any group has been created in Device Management Application.

## 4 Solution Deployment Type & Cost

Deployment types will be helpful to deploy azure resources with minimum inputs from the user. User can choose different deployment based on requirements. Deployment types helpful to deploy azure resources with minimal knowledge on Azure. We have three types of costing solutions we have in this project. 

* Basic solution
* Standard Solution
* Premium Solution

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
| **Time Series Insights**    | S1 ,10,00,000 messages/day, US$ 150.00/Month, 1 units * US$ 4.84   | PAYG        | $150.0
| **Azure Automation Account**        | Free      | PAYG                       | $0.00   
| **Notification Hub**       | Free                          | PAYG                      | $0.00 
| **Application Insight**       | Basic, 1GB * $2.30 Region: East US first 5GB free per month       | PAYG                  | $2.30 
| **Stream Analytics**   | Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US       | PAYG  | $80.30 
|                     |                       | **Estimated monthly cost**          | **$360.55**

### 4.2 Standard Costing details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | S2:2 Cores(s), 3.5 GB RAM, 50GB Storage, US$ 0.200, 1 Instance      | PAYG         | $146.00  
| **Cosmos DB**   | Standard, throughput 1000 RU/s (Request Units per second) 10x100 Rus(Throughput)- $58.40 10 GB storage – $2.50    | PAYG  | $60.90  
| **IoT HUB**        | S1, Unlimited devices, 1 Unit-$25.00/per month 400,000 messages/day           | PAYG                       | $25.00    
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US      | PAYG                          | $2.30   
| **Time Series Insights**    | S1 ,10,00,000 messages/day, US$ 150.00/Month, 1 units * US$ 4.84   | PAYG        | $150.0
| **Azure Automation Account**        | 2*Free      | PAYG                       | $0.00   
| **Notification Hub**       | 2*Free                          | PAYG                      | $0.00 
| **Application Insight**       | 2*Basic, 1GB * $2.30 Region: East US first 5GB free per month       | PAYG                  | $4.60 
| **Stream Analytics**   | 2 * Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US        | PAYG  | $160.60  
|                     |                       | **Estimated monthly cost**          | **$549.40**
           
### 4.3 Premium Costing details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | S2:2 Cores(s), 3.5 GB RAM, 50GB Storage, US$ 0.200, 1 Instance       | PAYG         | $146.0  
| **Cosmos DB**   | Standard, throughput 1000 RU/s (Request Units per second) 10x100 Rus(Throughput)- $58.40 10 GB storage – $2.50     | PAYG  | $60.90  
| **IoT HUB**        | S1, Unlimited devices, 1 Unit-$25.00/per month 400,000 messages/day         | PAYG                       | $25.00    
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US       | PAYG                   | $2.30  
| **Time Series Insights**    | S1 ,10,00,000 messages/day, US$ 150.00/Month, 1 units * US$ 4.84   | PAYG        | $150.0
| **Azure Automation Account**        | 2*Free      | PAYG                       | $0.00   
| **Notification Hub**       | 2*Free                          | PAYG                      | $0.00 
| **Application Insight**       | 2 * Basic, 1GB * $2.30 Region: East US first 5GB free per month        | PAYG                  | $4.60 
| **Stream Analytics**   | 2 * Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US         | PAYG  | $160.60  
|                     |                       | **Estimated monthly cost**          | **$549.40**
                     
## 5 What are paired regions? 

Azure operates in multiple geographies around the world. An Azure geography is a defined area of the world that contains at least one Azure Region. An Azure region is an area within a geography, containing one or more datacenters. 

Each Azure region is paired with another region within the same geography, together making a regional pair. The exception is Brazil South, which is paired with a region outside its geography. 

**IoT Hub Manual Failover Support Geo-Paired Regions**

| **S.No**      | **Geography**    | **Paired Regions**    | **Paired Regions**  |

| --------      | -------------    | -------------------  | ------------------   |

| 1         | North America      | East US 2        | Central US    |
| 2         | North America   | Central US   | East US 2    |
| 3      | North America  | West US 2   | West Central US     |
| 4         | North America      | West Central US        | West US 2   | 
| 5         | Canada   | Canada Central   | Canada East   |
| 6      | Canada   | Canada East    | Canada Central    |
| 7         | Australia      | Australia East        | Australia South East  |
| 8        | Australia   | Australia South East    | Australia East    |
| 9      | India  | Central India    | South India      |
| 10         | India      | South India        | Central India    | 
| 11        | Asia   | East Asia   | South East Asia    |
| 12     | Asia  | South East Asia    | East Asia     |
| 13        | Japan   | Japan West    | Japan East    |
| 14     | Japan  | Japan East    | Japan West      |
| 15         | Korea       | Korea Central        | Korea South     |
| 16        | Korea   | Korea South   | Korea Central    |
| 17     | UK  | UK South    | UK West   |
| 18     | UK   | UK West   | UK South     |

## 5 ARM Template Input Parameters

In the parameters section of the template, specify which values you can input when deploying the resources. These parameter values enable you to customize the deployment by providing values that are tailored for an environment (such as dev, test, and production). You are limited to 255 parameters in a template. You can reduce the number of parameters by using objects that contain multiple properties.

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
| **timeSeriesInsights** | select 'Yes' for deploying time series insights for monitoring of iot hub otherwise select 'No'.To explore, see https://docs.microsoft.com/en-in/azure/time-series-insights/time-series-quickstart.  | Yes, No   | No
| **timeSeriesInsightslocation** | specify the region for time series insights. | East US, East US2, North Europe, West Europe, West US | East US 
  
**Note**: Allowed Values are updated based on the availability of azure resources based on Region w.e.f Dt 24th July 2018. Microsoft may introduce availability of azure resources in future. More information can be found in **https://azure.microsoft.com/en-in/status/** 

## 6 Getting started

Azure Resource Manager allows you to provision your applications using a declarative template. In a single template, you can deploy multiple services along with their dependencies. The template consists of JSON and expressions that you can use to construct values for your deployment. You use the same template to repeatedly deploy your application during every stage of the application lifecycle.

Resource Manager provides a consistent management layer to perform tasks through Azure PowerShell, Azure CLI, Azure portal, REST API, and client SDKs.

* Deploy, manage, and monitor all the resources for your solution as a group, rather than handling these resources individually.
* Repeatedly deploy your solution throughout the development lifecycle and have confidence your resources are deployed in a consistent state.
* Manage your infrastructure through declarative templates rather than scripts.
* Define the dependencies between resources so they're deployed in the correct order.
* Apply access control to all services in your resource group because Role-Based Access Control (RBAC) is natively integrated into the management platform.
* Apply tags to resources to logically organize all the resources in your subscription.

### 6.1	 ARM Template Deployment

1.	Browse to **Oil&Gas Git Hub Repository** on your Edge Browser.

**https://github.com/sysgain/Oil-Gas/tree/master**

2. Select **main-template.json** from **master** branch as shown in the following figure.
 
![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/t1.png)

3. Select **Raw** from the top right corner.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/t2.png)

4. **Copy** the raw template and **paste** in your azure portal for template deployment.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/t3.png)

To deploy a template for Azure Resource Manager, follow the below steps.

1.	Go to **Azure portal**.

2.	Navigate to **Create a resource (+)**, search for **Template deployment**.

3.	Click Create button and click **Build your own Template in the editor** as shown in the following figure.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/8.png)

4.	The **Edit template** page is displayed as shown in the following figure. 

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/9.png)

5.	**Replace / paste** the template and click **Save** button.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/10.png)

6.	The **Custom deployment** page is displayed as shown in the following.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/b1.png)

#### 6.1.1 Inputs

These parameter values enable you to customize the deployment by providing values. There parameters allow to choose the solution type, region and credentials to authenticate SQL Database and Virtual Machines.

7. If you choose **yes** for **Edge VM + Simulator VM** then Edge VM and Simulator VMs will be deployed with in the solution.

8.	If you choose **No** then the **Edge VM and Simulator VM** will be not deployed with in the solution.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/b2.png)

9.	If you choose **yes** for **ML VM** then Pre-installed **docker** will be **deployed** with in the solution.

10.	If you choose **No** for **ML VM** then Pre-installed **docker** will **not be deployed** with in the solution.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/b3.png)

**Parameters for core Solution**

11.	Deploy the template by providing the parameters in custom deployment settings as shown in the following figure.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/b4.png)

**Parameters for core with monitoring**

12.	If you want to deploy the core with monitoring you must enter the below parameters

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/b5.png)

**Parameters for core with hardening and monitoring**

13.	If you want to deploy the core with Hardening and Monitoring you must enter the below parameters.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/b6.png)

14. Once all the parameters are entered, select the **terms and conditions** check box and click **Purchase**.

15. After the successful deployment of the ARM template, the following **resources** are created in a **Resource Group**.

16.	When you choose Costing model as **Core-with-monitoring-hardening** and the **Edge VM + Simulator VM** and **ML VM** as **Yes**, then the below components will be installed.

* 3-Virtual machines (2 windows & 1 Linux)

Windows VMS: 

**Dyno card VM** which install pre-installed software's for dyno card VM.

**ML VM** which install pre-installed docker.

Linux VM:

**Edge VM** is used to create IoT Edge device and installs modules in IoT Edge device

* 2-Web App
* 1-Application Insights
* 1-Data Lake Storage
* 1-IoT HUB
* 1-Log analytics
* 1-Logic app
* 1-service bus namespace
* 2-SQL database
* 2-Storage account
* 1-Stream Analytics job
* 1-Traffic Manager
* 1-Machine Learning Experiment Account
* 1-Machine Learning Management Account

17.	Once the solution is deployed successfully navigate to the resource group, select the created **resource group** to view the list of resources that are created in the Resource Group as shown in the following figure.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/17.png)
![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/18.png)
![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/19.png)
![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/20.png)

#### 6.1.2 Outputs

The Outputs section consists of the values that are returned from deployment. The output values can be used for further steps in Solution Configuration.

Go to **Resource group** -> click **deployments** -> select **Microsoft Template**.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/21.png)

Click **outputs**.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/22.png)

### 6.2 ARM Template Deployment Using Azure CLI

Azure CLI is used to deploy your resources to Azure. The Resource Manager template you deploy, can either be a local file on your machine, or an external file that is in a repository like GitHub.  

Azure Cloud Shell is an interactive, browser-accessible shell for managing Azure resources. Cloud Shell enables access to a browser-based command-line experience built with Azure management tasks in mind. 

Deployment can proceed within the Azure Portal via Azure Cloud Shell. 

#### 6.2.1 Customize main-template.parameters.json file 

1. Log in to **Azure portal**. 

2. Open the prompt. 
 
3. Select **Bash (Linux)** environment as shown in the following figure. 

4. Select your preferred **Subscription** from the dropdown list.  

5. Click **Create Storage** button as shown in the following figure. 

6. Copy **main-template.json** and **main-template.parameters.json** to your Cloud Shell before updating the parameters. 

7. Create **main-template.json** using the below command. 

    **vim main-template.json** 

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/c1.png)

8. Paste your **main-template.json** in editor as shown below and save the file. 

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/c2.png)

10.	Paste your **main-template.parameters.json** in editor. 

11.	Update the following parameters in main-template.json file 

* Solution Type 
* Edge VM + Simulator VM
* MLVM
* dataLakelocation
* machineLearningLocation
* locationDR
* oms-region
* appInsightsLocation
* sqlAdministratorLogin
* sqlAdministratorLoginPassword
* azureLogin
* tenantId
* vmsUsername
* vmsPassword

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/c3.png)

#### 6.2.2 Create Resource Group for Oil&gas olution 

Use the **az group create** command to create a **Resource Group** in your region.

**Description:** To create a resource group, use **az group create** command, It uses the name parameter to specify the name for resource group (-n) and location parameter to specify the location (-l). 

**Syntax**: **az group create -n <resource group name> -l <location>** 

Ex: **az group create -n <****> -l <***>** 

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/c4.png)

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/c5.png)

#### 6.2.3 Execute the template deployment 

Use the **az group deployment create** command to deploy the ARM template.

**Description**: To deploy the ARM Template, you require two files: 

1. 	**main-template.json** – contains the resource & its dependency resources to be provisioned from the ARM template.

2.	**main-template.parameters.json** –contains the input values that are required to provision respective SKU & Others details, for more details on the input parameter values navigate to Section 6 of this document. 

**Syntax**:  **az group deployment create --template-file './<main-template.json filename>' --parameters '@./<main-template.parameters.json filename>' -g < provide resource group name that created in the section 6.2.2> -n deploy >> <provide the outputs filename>** 

Ex: **az group deployment create --template-file './main-template.json' --parameters '@./main-template.parameters.json' -g oilandgas-iot -n deploy >> outputs.txt** 

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/c6.png)

Deployment may take between 15-20 minutes depending on deployment size. 

After successful deployment you can see the deployment outputs as follows.

![alt text](https://github.com/sysgain/Oil-Gas/raw/master/images/c7.png)

## 7. Post Deployment steps

### 7.1. Verify Containers in EdgeVM and Azure Portal

Go to **Resource Group** ->click **iotEdge** VM.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z1.png)

**Copy** the **Public IP Address** of the iotEdge VM.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z2.png)

**Login** to the VM through **putty**.

**Paste** the **public ip** at Host Name (or IP address) and **click** on **open**.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z3.png)

Enter credentials:

Enter the login username as: **adminuser**

Enter the Password as: **Password@1234**

Once login successful the below welcome message is displayed.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z4.png)

Here you can check the device and device modules in IoT Edge VM.

**docker ps** 

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z5.png)

Go to resource group -> click **iothub26hs3**

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z6.png)

click **IoT Edge** under **Automatic Device Management**.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z7.png)

Here we created and configured device from the IoT Edge VM. Click on **iot-dynocard-demo-device_1** device you can see the modules.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z8.png)

We can see the created modules in device details 

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z9.png)

### 7.2. Starting Stream Analytics job

Go to **Resource group** -> click **StreamAnalytics26hs3**

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z10.png)

After click on stream analytics job, you will have directed to screen like below.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z11.png)

**Note**: Before **Start** the Stream analytic job, first you need to **Renew the authorization**of the output DataLake.

Click on **Datalake** in the output section it will navigate the output details page.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z12.png)

Click on **Renew authorization** button and click **save**.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z13.png)

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z14.png)

Now, you can click the **start** button to start the Stream analytics job and you can see the **Starting Streaming job** message in notifications window.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z15.png)

Now you can see the Stream analytics job is **Running**.

![alt text](https://github.com/sysgain/Oil-Gas/blob/master/images/z16.png)
