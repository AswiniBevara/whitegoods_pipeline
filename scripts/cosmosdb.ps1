workflow  container{
    param(
       
        [Parameter(Mandatory=$true)]
        [string]
        $tenantId,

        [Parameter(Mandatory=$true)]
        [string]
        $clientId,

        [Parameter(Mandatory=$true)]
        [string]
        $clientSecret,

        [Parameter(Mandatory=$true)]
        [string]
        $deviceManagementUri,
        
        [Parameter(Mandatory=$true)]
        [string]
        $azureAccountName,

        [Parameter(Mandatory=$true)]
        [string]
        $azurePassword,
             
        [Parameter(Mandatory=$true)]
        [string]
        $cosmosDBAccountKey,

        [Parameter(Mandatory=$true)]
        [string]
        $cosmosDbAccountName,
        
        [Parameter(Mandatory=$true)]
        [string]
        $cosmosDbName,

        [Parameter(Mandatory=$true)]
        [string]
        $objectId
    )

    InlineScript{
    
        $tenantId = $Using:tenantId
        $clientId = $Using:clientId
        $clientSecret = $Using:clientSecret
        $deviceManagementUri = $Using:deviceManagementUri
        $azureAccountName = $Using:azureAccountName
        $azurePassword = $Using:azurePassword
        $cosmosDBAccountKey = $Using:cosmosDBAccountKey
        $cosmosDbAccountName = $Using:cosmosDbAccountName
        $cosmosDbName = $Using:cosmosDbName
        $objectId = $Using:objectId
        Set-ExecutionPolicy -ExecutionPolicy RemoteSigned  -Force
        $password = ConvertTo-SecureString $azurePassword -AsPlainText -Force
        $psCred = New-Object System.Management.Automation.PSCredential($azureAccountName, $password)
        Login-AzureRmAccount -Credential $psCred 
              
        $primaryKey = ConvertTo-SecureString -String $cosmosDBAccountKey -AsPlainText -Force
        $cosmosDbContext = New-CosmosDbContext -Account $cosmosDbAccountName -Database $cosmosDbName -Key $primaryKey
        start-Sleep -s 20
    
        # Create CosmosDB
        New-CosmosDbDatabase -Context $cosmosDbContext -Id $cosmosDbName
        start-Sleep -s 30
    
        # Create CosmosDB Collections
        New-CosmosDbCollection -Context $cosmosDbContext -Id 'Messages'  -OfferThroughput 400
        start-Sleep -s 20
        New-CosmosDbCollection -Context $cosmosDbContext -Id 'Templates' -OfferThroughput 400
        start-Sleep -s 20
        New-CosmosDbCollection -Context $cosmosDbContext -Id 'Groups' -OfferThroughput 400
        start-Sleep -s 30
        
            # Update Azure AD applications reply urls
            Connect-AzureAd -TenantId $tenantId -Credential $psCred -InformationAction Ignore
         
                 
            $deviceManagementUriOIDC=$deviceManagementUri+"/signin-oidc"   
            $replyURLList = @($deviceManagementUriOIDC);  
            Write-Host '', 'Configuring and setting the Azure AD reply URLs' -ForegroundColor Green
            Set-AzureADApplication -ObjectId $objectId -HomePage $deviceManagementUri -ReplyUrls $replyURLList -Verbose
            # Get Access token for calling API
            ##$accessToken=Get-AccessTokenAPI $tenantId $clientId $clientSecret
            ##Write-Host "Access token is " $accessToken -ForegroundColor Magenta
            #add sleep inorder to have API startup successfully
            ##Start-Sleep -s 10
            # Call API to initialize cosmos DB with default starter IoT templates
            # This templates will be loaded in the data packet designer
            ##$cosmosDBInitResult=CosmosDbInit $datapacketUri $accessToken  
            Start-Sleep -s 30
    }
}
