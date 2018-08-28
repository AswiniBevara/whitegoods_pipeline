# Microsoft

# White Goods Solution

**Table of Contents** 

- [1 About WhiteGoods Solution](#1-about-whitegoods-solution)
- [2 Solution Architecture Diagram and data work flow Diagram](#2-solution-architecture-diagram-and-data-work-flow-diagram)
    - [2.1 Basic Architecture Diagram](#21-basic-architecture-diagram)
    - [2.2 Standard Architecture Diagram](#22-standard-architecture-diagram)
    - [2.3 Premium Architecture Diagram](#23-premium-architecture-diagram)
    - [2.4 Data work Flow Diagram](24#-data-work-flow-diagram)
- [3 Azure Services](#3-azure-services)
    - [3.1 IoT Hub](#31-iothub)
    - [3.2 Stream Analytics](#32-stream-analytics)
    - [3.3 Azure Sphere](#33-azure-sphere)
    - [3.4 Notification Hub](#34-notification-hub)
    - [3.5 Web Application](#35-web-application)
    - [3.6 Device management web application](#36-device-management-web-application)
    - [3.7 Azure Active Directory](#37-azure-active-directory)
    - [3.8 Azure Run book](#38-azure-run-book)
    - [3.9 Cosmos DB](#39-cosmos-db)
    - [3.10 Log analytics](#310-log-analytics)
    - [3.11 Application Insights](#311-application-insights)
- [4 Solution Deployment Type & Cost](4#-solution-deployment-type-&-cost)
    - [4.1 Basic Costing Details](#41-basic-costing-details)
    - [4.2 Standard Costing details](#42-standard-costing-details)
    - [4.3 Premium Costing details](#43-premium-costing-details)
- [5 What are paired regions?](#5-what-are-paired-regions?)
- [6 Deployment Guide for the Solution](#6-deployment-guide-for-the-solution) 
- [7 Administrator Guide for the Solution](#7-administrator-guide-for-the-solution)
- [8 User Guide for the Solution](#8-user-guide-for-the-solution)
   
## 1 About White Goods Solution

The Azure Sphere Solution Accelerator ecosystem enables your company to setup connected device products quickly, securely, and cost-effectively. Connect your product to the cloud, define collected data, and simulate device-to-cloud communication. Monitor your connected products, analyze trends, gain insights in channel management.

## 2 Solution Architecture Diagram and data work flow Diagram

### 2.1 Basic Architecture Diagram

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/c1.png)

### 2.2	Standard Architecture Diagram

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/c2.png)

### 2.3 Premium Architecture Diagram

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/c3.png)

### 2.4 Data work Flow Diagram

Below is the architecture diagram that shows data flow in between azure components.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/c4.jpg)

## 3 Azure Services

### 3.1 IoT Hub

Azure IoT Hub is Microsoft’s Internet of Things connector to the cloud. It’s a fully managed cloud service that enables reliable and secure bi-directional communications between millions of IoT devices and a solution back end. Device-to-cloud telemetry data tells you about the state of your devices and assets. Cloud-to-device messages let you send commands and notifications to your connected devices. Device messages are sent in a durable way to accommodate intermittently connected devices.

Azure Sphere device generates data and sends to IoT Hub.

### 3.2 Steam Analytics

Stream Analytics is an event processing engine that can ingest events in real-time, whether from one data stream or multiple streams. Events can come from sensors, applications, devices, operational   systems, websites, and a variety of other sources. Stream Analytics provides high-throughput, low-  latency processing, while supporting real-time stream computation operations.

### 3.3 Azure Sphere

The Azure Sphere solution brings together the best of Microsoft’s expertise in cloud, software, and silicon—resulting in a unique approach to security that starts in the silicon and extends to the cloud. Azure Sphere contains three components that work together to keep devices protected and secured in today’s dynamic threat landscape: Azure Sphere certified MCUs, the Azure Sphere OS and the Azure Sphere Security Service.

The first Azure Sphere chip will be the MediaTek MT3620, which represents years of close collaboration and testing between MediaTek and Microsoft. This new cross-over class of MCU includes built-in Microsoft security technology, built-in connectivity, and combines the versatility and power of a Cortex-A processor with the low overhead and real-time guarantees of a Cortex-M class processor.

### 3.4 Notification Hub

Azure Notification Hubs are push notification software engines designed to alert users about new content for a given site, service or app. Azure Notification Hubs are part of Microsoft Azure’s public cloud service offerings.

Notification hub is used to send notifications to Mobile application whenever an event occurs beyond the defined metrics.

### 3.5 Web Application  

A Web application (Web app) is an application program that is stored on a remote server and delivered over the Internet through a browser interface.
In Solution we have two web applications in one app service plan.

* Device management web application

### 3.6 Device management web application

Device management web application is a dashboard where you can view the Summary of Devices such Device count, status of device connection. It also provides the detailed insights of a device. We can view the Alert summary, Device Activation status and date, Shipment date.

### 3.7 Azure Active Directory
  
Microsoft Azure Active Directory (Azure AD) is a cloud service that provides administrators with the ability to manage end user identities and access privileges. The service gives administrators the freedom to choose which information will stay in the cloud, who can manage or use the information, what services or applications can access the information and which end users can have access.

Azure Active directory is used to authenticate users to login to Web Application. Azure active Directory enables secure authentications to web application.

### 3.8 Azure Run book

Azure Automation enables the users to automate the tasks, which are manual and repetitive in nature by using Runbooks. 

Runbooks in Azure Automation are based on Windows PowerShell or Windows PowerShell Workflow. We can code and implement the logic, which we want to automate, using PowerShell.

In this Solution Azure run books are used to create collections in Document DB. And will be used to update reply URLs in Active Directory Application.
 
### 3.9 Cosmos DB  

Azure Cosmos DB is a Microsoft cloud database that supports multiple ways of storing and processing data. As such, it is classified as a multi-model database. In multi-model databases, various database engines are natively supported and accessible via common APIs.

In this Solution, Cosmos DB have Templates, Messages and Groups Collections. The Messages collections will get updated with the telemetry data of the Device.

### 3.10 Log analytics

Log Analytics is a service in Operations Management Suite (OMS) that monitors your cloud and on-premises environments to maintain their availability and performance. It collects data generated by resources in your cloud and on-premises environments and from other monitoring tools to provide analysis across multiple sources.

OMS Log analytics are helpful to monitor SQL Database, Web Apps and Other Azure Components.

### 3.11 Application Insights

Application Insights is an extensible Application Performance Management (APM) service for web developers on multiple platforms. Use it to monitor live web application. It will automatically detect performance anomalies. It includes powerful analytics tools to help diagnose issues and to understand what users actually do with web application.

Application Insights monitor below:

* Request rates, response times, and failure rates
* Dependency rates, response times, and failure rates
* Exceptions 
* Page views and load performance
* AJAX calls
* User and session counts
* Performance counters
* Host diagnostics from Docker or Azure
* Diagnostic trace logs
* Custom events and metrics

## 4 Solution Deployment Type & Cost

Deployment types will be helpful to deploy azure resources with minimum inputs from the user. User can choose different deployment based on requirement. Deployment types are helpful to deploy azure resources with minimal knowledge on Azure. We have three types of costing solutions in this project.  

* Basic 
* Standard
* Premium

**Basic**

This deployment type deploys minimum azure resources with minimal available SKU. This solution deploys only required azure resources excluding high available components. 

**Standard**

This deployment type deploys the minimum azure resources along with monitoring azure resources such as application Insights and OMS Log analytics.
In case of Region failover, re-deploy ARM template will be deployed.

**Premium**

This Deployment type deploys minimum azure resources required to run the Solution which allows to monitor the solution also provides high availability for webapi and CosmosDB and including optional Components. 

Below are the Costing details of each deployment type which explains SKU and Costing price details including optional components. 

### 4.1 Basic Costing Details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | Basic Tier; 1 B1 (1 Core(s), 1.75 GB RAM, 10 GB Storage)      | PAYG         | $54.75 
| **Cosmos DB**   | Standard, throughput 400 RU/s (Request Units per second) 4x100 Rus(Throughput)- $23.36 10 GB storage – $2.50    | PAYG  | $25.86 
| **IoT HUB**        | S1, Unlimited devices, 1 Unit-$25.00/per month 400,000 messages/day                      | PAYG                       | $25.00    
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US. After finishing 5GB, $2.30 per GB.      | PAYG                          | $2.30  
| **Azure Automation Account**        | Capability: Process Automation 500 minutes of process automation and 744 hours of watchers are free each month.     | PAYG       | $0.00   
| **Notification Hub**       | Free                          | PAYG                      | $0.00 
| **Application Insight**       | Basic, 1GB * $2.30 Region: East US first 5GB free per month       | PAYG                  | $2.30 
| **Stream Analytics**   | Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US      | PAYG  | $80.30 
|                     |                       | **Estimated monthly cost**          | **$190.51**

### 4.2 Standard Costing details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | Standard Tier; S1: 2 (Core(s), 1.75 GB RAM, 50 GB Storage) x 730 Hours; Windows OS     | PAYG         | $146.00  
| **Cosmos DB**   | Standard, throughput 400 RU/s (Request Units per second) 4x100 Rus(Throughput)- $23.36 10 GB storage – $2.50   | PAYG  | $25.86
| **IoT HUB**        | S1, Unlimited devices, 1 Unit-$25.00/per month 400,000 messages/day          | PAYG                       | $25.00    
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US. After finishing 5GB, $2.30 per GB.     | PAYG                          | $2.30   
| **Azure Automation Account**        | 2*Capability: Process Automation 500 minutes of process automation and 744 hours of watchers are free each month.    | PAYG                       | $0.00   
| **Notification Hub**       | 2*Free                          | PAYG                      | $0.00 
| **Application Insight**       | 2 * Basic, 1GB * $2.30 Region: East US first 5GB free per month       | PAYG                  | $4.60 
| **Stream Analytics**   | 2 * Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US         | PAYG  | $160.60  
| **Traffic Manager**     | External endpoints 2*$0.54 Region: East US    | PAYG    | $1.08
|                     |                       | **Estimated monthly cost**          | **$365.44**
           
### 4.3 Premium Costing details

| **Resource Name**           | **Size**           | **Resource costing model**                 | **Azure Cost/month**                                                                                                                
| -------------              | -------------       | --------------------                       | ------------                                                                                                             
| **App Service Plan**       | Standard Tier; S1: 2 (Core(s), 1.75 GB RAM, 50 GB Storage) x 730 Hours; Windows OS      | PAYG         | $146.0  
| **Cosmos DB**   | Standard, throughput 400 RU/s (Request Units per second) 4x100 Rus(Throughput)- $23.36 10 GB storage – $2.50     | PAYG  | $25.86 
| **IoT HUB**        | S1, Unlimited devices, 1 Unit-$25.00/per month 400,000 messages/day        | PAYG                       | $25.00    
| **Log Analytics**      | First 5GB of data storage is free. Per GB(Standalone) Region East US. After finishing 5GB, $2.30 per GB.        | PAYG                   | $2.30  
| **Azure Automation Account**        | 2*Capability: Process Automation 500 minutes of process automation and 744 hours of watchers are free each month.     | PAYG        | $0.00   
| **Notification Hub**       | 2*Free                          | PAYG                      | $0.00 
| **Application Insight**       | 2 * Basic, 1GB * $2.30 Region: East US first 5GB free per month         | PAYG                  | $4.60 
| **Stream Analytics**   | 2 * Standard Streaming Unit, 1 unit(s) 1 * $80.30 Region: East US       | PAYG  | $160.60  
| **Traffic Manager**     | External endpoints 2*$0.54 Region: East US    | PAYG    | $1.08
|                     |                       | **Estimated monthly cost**          | **$365.44**
                     
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

## 6 Deployment Guide for the Solution

To deploy the Basic , Standard or Premium Solution please refer below **Deployment Guide Documentation** URL.

**https://github.com/sysgain/whitegoods/blob/master/Documentation/DeploymentGuide.md**

## 7 User Guide for the Solution

For Running Blink Application and verifying the Device Management Web application, please refer **User Guide Documentation**.

## 8 Administrator Guide for the Solution

To configure and validate the Standard and Premium Solution, please refer the **Administrator Guide**.
