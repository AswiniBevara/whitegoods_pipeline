# Microsoft

# White Goods Solution

**Table of Contents** 

- [1 Prerequisites for Deploying ARM Template](#1-prerequisites-for-deploying-arm-template)
    - [1.1 Integrating applications with Azure Active Directory](#11-integrating-applications-with-azure-active-directory)
        - [1.1.1 To register a new application using the Azure portal](#111-to-register-a-new-application-using-the-azure-portal)
        - [1.1.2 To add application credentials or permissions to access web APIs](#112-to-add-application-credentials-or-permissions-to-access-web-apis)
        - [1.1.3 To get Tenant ID](#113-to-get-tenant-id)
        - [1.1.4 To get application ID and authentication key](#114-to-get-application-id-and-authentication-key)
        - [1.1.5 Creating session ID](#115-creating-sessionid)  
- [2 ARM Template Input Parameters](#2-arm-template-input-parameters)
- [3 Getting Started](#3-getting-started)
    - [3.1 ARM Template Deployment Using Azure Portal](#31-arm-template-deployment-using-azure-portal)
        - [3.1.1 Inputs](#311-inputs)
        - [3.1.2 Outputs](#312-outputs)
    - [3.2 ARM Template Deployment Using Azure CLI](#32-arm-arm-template-deployment-using-azure-cli)
        - [3.2.1 Create Resource Group for oilandgas Solution](#321-create-resource-group-for-oilandgas-solution)
        - [3.2.2 Execute the Template Deployment](#322-execute-the-template-dDeployment)
- [4 Post Deployment Steps: Getting started with Azure Sphere](#4-post-deployment-steps-getting-started-with-azure-sphere)
    - [4.1 Set Up your Machine for Development Machine](#41-set-up-your-machine-for-development-machine)
        - [4.1.1 Connect the RDB](#411-connect-the-rdb)
        - [4.1.2 Install the TAP Driver](#412-install-the-tap-driver)
        - [4.1.3 Configure TAP Networking](#413-configure-tap-networking)
    - [4.2 Install Visual Studio](#42-install-visual-studio)
    - [4.3 Install the Visual Studio Tools Preview for Azure Sphere](#43-install-the-visual-studio-tools-preview-for-azure-sphere)
        - [4.3.1 To install the Visual Studio Tools Preview for Azure Sphere](#431-to-install-the-visual-studio-tools-preview-for-azure-sphere)
    - [4.4 Update Device Software](#44-update-device-software)
        - [4.4.1 To Check the Software Version on your Device](#441-to-check-the-software-version-on-your-device)
    - [4.5 Claim your Device](#45-claim-your-device)
    - [4.6 Add to Device Group](#46-add-to-device-group)
    - [4.7 Configure Wi-Fi](#47-configure-wi-fi)
        - [4.7.1 To Set up Wi-Fi on your Device](#471-to-set-up-wi-fi-on-your-device)
    - [4.8 Connect to IoT Hub](#98-connect-to-iot-hub)
        - [4.8.1 Set Up Microsoft Azure Credentials](#481-set-up-microsoft-azure-credentials)
        - [4.8.2 Add your Device to the IoT Hub](#482-add-your-device-to-the-iot-hub)
    
## 1 Prerequisites for Deploying ARM Template

Create an application in Azure Active Directory.

### 1.1 Integrating applications with Azure Active Directory

Any application that wants to use the capabilities of Azure AD must first be registered in an Azure AD tenant. This registration process involves giving Azure AD details about your application, such as the URL where it’s located, the URL to send replies after a user is authenticated, the URI that identifies the app, and so on.

#### 1.1.1 To register a new application using the Azure portal

1. **Sign in** to the **Azure portal**.

2. In the left-hand navigation pane, **click** the **Azure Active Directory** service, click **App registrations** and click **New application registration**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/5.png)

3. When the Create page appears, enter your application's registration information:

* **Name:** Enter the application name
* **Application type:**
      - Select "**Web app / API**" for client applications and resource/API applications that are installed on a secure server. This setting is used for OAuth confidential web clients and public user-agent-based clients. The same application can also expose both a client and resource/API.
* **Sign-On URL:** For "**Web app/API**" applications, provide the base URL of your app. For example, **https://localhost** might be the URL for a web app running on your local machine. Users would use this URL to sign in to a web client application.

4. When finished, click **Create**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/6.png)

#### 1.1.2 To add application credentials, or permissions to access web APIs

1. Click the **Azure Active Directory** service, click **App registrations** and then find/click the **application** you want to configure.

2. You are taken to the application's main registration page, which opens the **Settings** page for the application. To add a secret key for your web application's credentials:

3. **Click** the **Keys** section on the **Settings** page.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/7.png)

4. Add a description for your key and Select duration then click **Save**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/8.png)

5. The right-most column will contain the key value, after you save the configuration changes. **Be sure to copy the key** for use in your client application code, as it is not accessible once you leave this page.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/9.png)

#### 1.1.3 To get Tenant ID

1. Select **Azure Active Directory**.

2. To get the **tenant ID**, select **Properties** for your Azure AD tenant and **Copy** the **Directory ID**. This value is your **tenant ID**.

3. **Note down** the Copied **Directory ID** which is highlighted in the below figure, this will be used while deploying the **ARM template**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/10.png)

#### 1.1.4 To get application ID and authentication key

1. From **App registrations** in Azure Active Directory, **select** your **application**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/11.png)

2. **Copy** the **Application ID** and **object ID**. The application ID value is referred as the **client ID**.

3. **Note down** the Copied **Application ID** and **object ID** which is highlighted in the below figure, this will be used while deploying the ARM template.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/12.png)

#### 1.1.5 Creating session ID

1. Use the below URL to generate GUID.

    **https://www.guidgenerator.com/**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/13.png)

2. Click **Generate some GUIDs!** This will generate GUID in Results box. 

3. **Copy** and **Note down** the generated **GUID** which is highlighted in the below figure, this will be used while deploying the ARM template.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/14.png)

## 2 ARM Template Input Parameters

In the parameters section of the template, specify the values as inputs when deploying the ARM Template. These parameter values enable you to customize the deployment by providing values that are tailored for your environment (such as dev, Staging and production). 

| **Parameter Name**  | **Description**     | **Allowed Values**    | **Default Values**                                                                                                               
| -------------        | -------------      | -----------------    | ------------ 
| **Solution Type**          | choose your solution type to deploy.     | Basic, Standard, Premium    | 
| **geo-paired-region**   | If solution type is chosen as standard and premium, enter location for geo-paired-region,for more information about the geo-paired-region navigate to section 5 in the Readme.md(https://github.com/sysgain/whitegoods/blob/master/README.md) | EastUS2, CentralUS,WestUS2, WestCentralUS, CanadaCentral, CanadaEast, AustraliaEast, AustraliaSouthEast, CentralIndia, SouthIndia, EastAsia, SouthEastAsia,JapanWest, JapanEast, KoreaCentral, KoreaSouth, UKSouth, UKWest  | 
| **omsWorkspaceRegion**   | Choose location for OMS Log Analytics to deploy. For more information, see https://docs.microsoft.com/en-us/azure/log-analytics/log-analytics-overview.   | australiasoutheast, canadacentral, centralindia, eastus, japaneast, southeastasia, uksouth, westeurope    | eastus
| **appInsightsLocation**   | specify the region for application insights.  | eastus, northeurope, southcentralus, southeastasia, westeurope,westus2    | eastus
| **appInsightsLocationDr**    | If solution type is chosen as premium, enter location for application insights for disaster recovery. | eastus, northeurope, southcentralus, southeastasia, westeurope, westus2    | westus2
| **tenantId**   | Tenant Id of the created Azure active directory application. For instructions, see https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-howto-tenant in the Microsoft documentation   |  | 
| **clientId**  | Application ID of the created Azure active directory application. For information, see https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-integrating-applications in the Microsoft documentation.   |     | 
| **objectId**  | Object Id of the created Azure active directory application.  |     | 
| **clientSecret**  | clientSecret of the created Azure active directory application.For instructions, see https://docs.microsoft.com/en-us/azure/azure-resource-manager/resource-group-create-service-principal-portal#get-application-id-and-authentication-key in the Microsoft documentation.  |   | 
| **azureAccountName**   | azure portal login username.       |    | 
| **azurePassword** | azure portal login password.       |  | 
| **sessionId** | Refer 1.1.5 section for getting the session ID.  |   |  
  
## 3 Getting started

Azure Resource Manager allows you to provision your applications using a declarative template. In a single template, you can deploy multiple services along with their dependencies. The template consists of JSON and expressions that you can use to construct values for your deployment. You use the same template to repeatedly deploy your application during every stage of the application lifecycle.

Resource Manager provides a consistent management layer to perform tasks through Azure PowerShell, Azure CLI, Azure portal, REST API, and client SDKs.

Resource manager provides the following feature:

* Deploy, manage, and monitor all the resources for your solution as a group, rather than handling these resources individually.
* Repeatedly deploy your solution through the development lifecycle and your resources are deployed in a consistent state.
* Manage your infrastructure through declarative templates rather than scripts.
* Define the dependencies between resources so they're deployed in the correct order.
* Apply access control to all services in your resource group because Role-Based Access Control (RBAC) is natively integrated into the management platform.
* Apply tags to resources to logically organize all the resources in your subscription.

### 3.1. ARM Template Deployment Using Azure Portal

1. **Click** the below **Git hub repo URL**.

**https://github.com/sysgain/whitegoods/tree/master**

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

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p1.png)
![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p2.png)

#### 3.1.1 Inputs

These parameter values enable you to customize the deployment by providing values. There parameters allow to choose the solution type, region and credentials to authenticate SQL Database and Virtual Machines.

**Parameters for Basic Solution:**

7. If you want to deploy the **Basic Solution** you have to enter the below parameters.

**Note:** For **basic solution**, select the geo-paired region for your template deployment and choose the values of **OMS Workspace Region, App Insights Location** or keep the default values as it is. It is not necessary to choose **High availability region** of **App Insights Location Dr**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/23.jpg)

**Parameters for Standard Solution:**

8. If you want to deploy the **standard solution** you have to enter the below parameters.

**Note:** For **standard solution**, select the geo-paired region for your template deployment and choose the values of **OMS Workspace Region, App Insights Location** and **High availability region** of **App Insights Location Dr** or keep the default values as it is.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/24.png)

**Parameters for Premium solution:**

9. If you want to deploy the **Premium solution** you have to enter the below parameters.

**Note**: For **Premium solution**, select the geo-paired region for your template deployment and choose the values of **OMS Workspace Region, App Insights Location** and **High availability region** of **App Insights Location Dr** or keep the default values as it is.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/25.png)

10. Once all the parameters are entered, select the **terms and conditions** check box and click **Purchase**.

11. After the successful deployment of the ARM template, the following **resources** are created in a **Resource Group**.

* 1 app services
* 1 application Insights
* 1 automation account
* 1 run book
* 1 azure cosmos DB
* 1 IoT Hub
* 1 log analytics
* 1 Notification Hub
* 1 Stream analytics job
* Traffic manager

The above resources deployed for **Basic Solution**.

12. Expect IoT Hub, Cosmos DB, OMS Log Analytics, Automation Account and Run Book, the rest of the resources are created additionally along with Traffic manager as disaster recovery for Standard and Premium Solution deployment.

13. Once the solution is deployed successfully, navigate to the resource group to view the list of resources that are created as shown below.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/26.png)
![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/27.png)

#### 3.1.2 Outputs

14. Go to **Resource group** -> click **deployments**. 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/28.png)

15. Click **Microsoft Template**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/29.png)

16. Click **outputs**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/30.png)

### 3.2 ARM Template Deployment Using Azure CLI

AAzure CLI is used to deploy your resources to Azure. The Resource Manager template you deploy, can either be a local file on your machine, or an external file that is in a repository like GitHub.  

Azure Cloud Shell is an interactive, browser-accessible shell for managing Azure resources. Cloud Shell enables access to a browser-based command-line experience built with Azure management tasks in mind. 

Deployment can proceed within the Azure Portal via Windows PowerShell.  

1. Download the master branch from the URL mentioned in section 3.1

2. Open Windows PowerShell and run the following command

    **az login**

3. It shows the device login URL and code to authenticate the device login after running the command.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/i1.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/32.png)

4. Past the code and click **Continue**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/33.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/34.png)

5. Download the main-template.parameters.json in your local system from the below  URL, paste it in  an editor , update the values and save it.

**https://github.com/sysgain/whitegoods/blob/master/main-template.parameters.json**

6. Update the following parameters in **main-template.parameters.json** file 

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

#### 3.2.1 Create Resource Group for White Goods Solution 

Use the **az group create** command to create a **Resource Group** in your region.

**Description:** To create a resource group, use **az group create** command, It uses the name parameter to specify the name for resource group (-n) and location parameter to specify the location (-l). 

**Syntax**: **az group create -n < resource group name> -l < location >**  

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/35.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/36.png)

#### 3.2.2 Execute the template deployment 

Use the **az group deployment create** command to deploy the ARM template.

**Description**: To deploy the ARM Template, you require two files: 

**main-template.json** – contains the resource & its dependency resources to be provisioned from the ARM template.

**main-template.parameters.json** –contains the input values that are required to provision respective SKU & Others details, for more details on the input parameter values navigate to Section 2 of this document. 

**Syntax**:  **az group deployment create --template-file './<main-template.json filename>' --parameters '@./<main-template.parameters.json filename>' -g < provide resource group name that created in the section 3.2.1> -n deploy >> <provide the outputs filename>** 

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/i2.png)

Deployment may take between 15-20 minutes depending on deployment size. 

After successful deployment you can see the deployment outputs as follows.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/39.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/40.png)

## 4 Post Deployment steps: Getting started with Azure Sphere

### 4.1 Set Up your Machine for Development Machine

To connect to a reference development board (RDB), your development machine requires the following: 

* Windows 10 Anniversary Update or later 
* Support for the Visual Studio 2017 System Requirements 
* A USB Port

#### 4.1.1 Connect the RDB

The RDB connects to a PC through a USB micro-connector. When plugged in, the RDB exposes three COM ports. 

The first time you plug in the board, the drivers should be automatically downloaded and installed. Installation can be slow; if the drivers are not installed automatically, right-click on the device name in Device Manager and select Update driver.

**Click** on **search Device Manager** in Windows Search Box to verify installation, open Device Manager and look for three COM ports:

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/41.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/42.png)

#### 4.1.2 Install the TAP Driver

The development board communicates with the PC over serial line internet protocol (SLIP). Tap-Windows provides a network interface driver for SLIP.

To install TAP and enable SLIP Communication

1. **Install TAP-Windows**, which came with your Azure Sphere software development kit (SDK). 

2. Download below zip file and extract it **Download** the **tap-windows-9.21.2.exe** file from below link and **Click** on **install tap-windows-9.21.2.exe**. 

**url: https://projectiot.blob.core.windows.net/whitegoods/Documents/CustomerDropTP4.0.1.zip**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/t1.png)

4. Click **Next**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p3.png)

5. Click **I Agree**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/45.png)

6. Choose **TAP Virtual Ethernet Adapter** and **TAP Utilities**, but not TAP SDK and click **Next**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/46.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/47.png)

7.	If you are asked to authorize installation of the driver, select **Install**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/48.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/49.png)

#### 4.1.3 Configure TAP Networking

1.	After you install the driver, configure TAP networking for the board.

2.	Go to **Control Panel**, search **View Network Connections** in the search box.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/t2.png)

3. Click to open the **View Network Connections**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p4.png)

4. Find the TAP-Windows Adapter V9.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p5.png)

5. Select **TAP-Windows Adapter v9** and rename it to **sl0** (lower case S, lower case L, the number zero):

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p6.png)

6. **Open Properties** for **sl0** and disable all services except **Internet Protocol Version 4 (TCP/IPv4)**:

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/54.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/55.png)

6.	Select Properties for TCP/IPv4 and configure it to use the **IP address 192.168.35.1**, **subnet mask 255.255.255.0** and **click OK**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/56.png)

7.	Now you can see the **sl0** is **enabled**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p7.png)

### 4.2 Install Visual Studio 

The Visual Studio Tools for Azure Sphere require Visual Studio Enterprise, Professional, or Community 2017 version 15.3 or later. To verify which version is installed, start the Visual Studio Installer and make sure that the version number is 15.3.0 or later.

If you have installed an earlier version of Visual Studio 2017 15.3.0 (preview 4 or later) from the Visual Studio Preview channel, you can continue to use that version. However, use of the Preview channel is no longer required; you can use the current version from the regular Visual Studio website. 

To install Visual Studio, click **Download Visual Studio**, select the edition to install, and then run the installer. You can choose to install any workloads, or none. The Visual Studio Tools for Azure Sphere installation procedure automatically installs the workloads that the SDK requires.

### 4.3 Install the Visual Studio Tools Preview for Azure Sphere

The Visual Studio Tools Preview for Azure Sphere includes: 

* A custom Azure Sphere Developer Command Prompt, which is available in the Start menu under Azure Sphere 
* The GDB debugger for use with the Azure Sphere development board 
* Device, cloud, and image utilities 
* Libraries for application development 
* Visual Studio extensions to support Azure Sphere development 

**Azure_Sphere_VS_Dev_Tools_Preview.exe** installs the complete Azure Sphere software development kit (SDK).

#### 4.3.1 To install the Visual Studio Tools Preview for Azure Sphere

1. Download below file and extract it, run **VS_Tools_Preview_for_Azure_Sphere.exe** it. 

**Url: : https://projectiot.blob.core.windows.net/whitegoods/Documents/CustomerDropTP4.0.1.zip**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/v1.png)

2. To install the developer tools. **Agree** to the license terms and **select Install**.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p8.png)

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p9.png)

3. If you have just installed Visual Studio for the first time, you might see the message, "No product to install SDK on." If this occurs, restart your PC and return to this step.

4. In the VSIX Installer window, confirm the Visual Studio version(s) for which to install the tools.

5. **Accept** the **elevation prompt**.

6. After installation starts, find the VSIX Installer window and bring it to the front. The installation process displays two installation windows: The Visual Studio Tools Preview for Azure Sphere window and the VSIX Installer window. The former reports progress and errors from the overall installation, and the latter reports information about the Visual Studio extension only. If the VSIX window becomes obscured during installation, you might not see error reports or requests for action.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/61.png)

7. When setup completes, select **Close** in both the VSIX Installer window and the Visual Studio Tools Preview for Azure Sphere setup window.

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/62.png)

8. If the installer returns errors, try uninstalling and then reinstalling the tools. To uninstall the tools, use **Add and Remove Programs in Control Panel**.

### 4.4 Update Device Software

#### 4.4.1 To Check the Software Version on your Device

1. Open an **Azure Sphere Developer Command Prompt**. To find the **Azure Sphere Developer Command Prompt**, click the Windows Start button and search Azure Sphere Developer Command Prompt Preview.  

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/a1.png)

2. Issue the following command: 

**dutil device sdkversion** 
 
![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/63.png)

3. The number that follows the + should be 107652 or greater. If you see a smaller number, you must update the software by following the instructions from the below link.

URL: **https://projectiot.blob.core.windows.net/whitegoods/Documents/DeviceUpdateToTP4.0.1.docx**
 

### 4.5 Claim your Device

After you install Visual Studio and the SDK, you must claim your device. Claim your device only once. 

Every device has a unique and immutable Azure Sphere device ID that the Azure Sphere security service uses to identify and authenticate it. Before you update the device software or develop applications for the device, you must claim the device. Claiming the device associates it with your Azure Sphere tenant.

Refer  **HowToCreateATenant.docx** for information about creating a tenant.

Before you claim a device, be sure that you are logged in to the tenant that you plan to use with Azure Sphere services.

Refer the cutil login command in the Command-line Utilities  for more information.

**To claim your device:**

1. Connect your board to the PC by USB. 

2. Open an Azure Sphere Developer Command Prompt. To find the Azure Sphere Developer Command Prompt, click the Start button and type Azure Sphere.

3. **Run** the **cutil cloud utility** with the device command as follows:

**cutil device claim --attached**

4.	This command reads the Azure Sphere device ID from the board and associates it with your current tenant. If you are prompted to log in to Microsoft Azure, do so using your Azure Sphere credentials.

### 4.6 Add to Device Group

Before you configure Wi-Fi on your device, you must add it to the Microsoft-created System Software device group. The group is named System Software and has the following device group ID:

**63bbe6ea-14be-4d1a-a6e7-03591d882b42**

Use the following command to add your device to the group:

**Cutil device setdg –attached –devicegroupid  63bbe6ea-14be-4d1a-a6e7-03591d882b42**

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/64.png)

### 4.7 Configure Wi-Fi

You must configure the device for Wi-Fi before it can communicate with the Azure IoT Hub or receive over-the-air (OTA) updates.

#### 4.7.1 To Set up Wi-Fi on your Device

1.	If your device is not connected to your PC, connect it now.

2.	**Open** an **Azure Sphere Developer Command Prompt** and issue a command in the following form:

**dutil wifi add –s <xxxxxxx> -k <XXXXXXXXX>**

3.	The **-s flag** specifies the network SSID, and the **-k flag** specifies the WPA2 key. Network SSIDs are case-sensitive and can include only ASCII characters. To add an open Wi-Fi access point, omit the -k flag.

4.	You should see:

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/65.png)

### 4.8 Connect to IoT Hub

#### 4.8.1 Set Up Microsoft Azure Credentials

To run this sample, you must have a Microsoft Azure subscription and an IoT Hub. 

After you set up the subscription, you can create a hub. Log into the Azure Portal and follow these instructions to set up your hub.

#### 4.8.2 Add your Device to the IoT Hub

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

![alt text](https://github.com/sysgain/whitegoods/raw/master/Images/p1.png)

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
