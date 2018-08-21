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
        $objectId,

        [Parameter(Mandatory=$true)]
        [string]
        $datapacketUri
    )

    InlineScript{
    
        $tenantId = $Using:tenantId
        $clientId = $Using:clientId
        $clientSecret = $Using:clientSecret
        $deviceManagementUri = $Using:deviceManagementUri
        $azureAccountName = $Using:azureAccountName
        $azurePassword = $Using:azurePassword   
        $objectId = $Using:objectId
        $datapacketUri=$Using:datapacketUri

    Set-ExecutionPolicy -ExecutionPolicy RemoteSigned  -Force
    $password = ConvertTo-SecureString $azurePassword -AsPlainText -Force
    $psCred = New-Object System.Management.Automation.PSCredential($azureAccountName, $password)
    start-Sleep -s 20
    Login-AzureRmAccount -TenantId $tenantId -Credential $psCred 
    start-Sleep -s 20

    #Update Azure AD applications reply urls
    Connect-AzureAd -TenantId $tenantId -Credential $psCred -InformationAction Ignore
    $datapacketHomeUri="https://"+$datapacketUri
    $datapacketUriOIDC="https://"+$datapacketUri+"/signin-oidc"      
    $deviceManagementUriOIDC=$deviceManagementUri+"/signin-oidc"   
    $replyURLList = @($datapacketUriOIDC,$deviceManagementUriOIDC);  
    Write-Host '', 'Configuring and setting the Azure AD reply URLs' -ForegroundColor Green
    Set-AzureADApplication -ObjectId $objectId -HomePage $datapacketHomeUri -ReplyUrls $replyURLList -Verbose
    Start-Sleep -s 30
}
}
