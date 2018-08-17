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
