#Requires -Version 3.0

Param(
    [string] [Parameter(Mandatory=$true)] $Stage,
    [string] [Parameter(Mandatory=$true)] $ResourceGroupName,
    [string] [Parameter(Mandatory=$true)] $TenantId,
    [string] [Parameter(Mandatory=$true)] $SubscriptionId,
    [string] $ResourceGroupLocation = "West Europe",
    [string] $TemplateFile = 'azuredeploy.json'
)

try 
{
    [Microsoft.Azure.Common.Authentication.AzureSession]::ClientFactory.AddUserAgent("VSAzureTools-$UI$($host.name)".replace(' ','_'), '3.0.0')
} 
catch 
{
}

Set-StrictMode -Version 3

$TemplateFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateFile))
$TemplateParametersFile = 'azuredeploy.parameters.' + $Stage + '.json'
$TemplateParametersFile = [System.IO.Path]::GetFullPath([System.IO.Path]::Combine($PSScriptRoot, $TemplateParametersFile))

Write-Output 'Setting Azure context...'
Set-AzContext -Subscription $SubscriptionId -Tenant $TenantId | Out-Null
Write-Output 'Azure context has been set.'

# Create the resource group only when it doesn't already exist

if ($null -eq (Get-AzResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation -Verbose -ErrorAction SilentlyContinue)) {
    Write-Output "Creating resource group $ResourceGroupName..."
    New-AzResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation -Tag @{ "purpose"="demo" } -Verbose -Force -ErrorAction Stop
    Write-Output "Resource group created."

    Write-Output "Set No-Delete-Lock for $ResourceGroupName..."
    New-AzResourceLock -LockName "no-delete" -LockLevel CanNotDelete -ResourceGroupName $ResourceGroupName -Force -ErrorAction Stop
    Write-Output "No-Delete Lock is set."    
} 
else {
    Write-Output "Resource group $ResourceGroupName already exists."
}

Write-Output "Starting template deployment with template $TemplateParametersFile ..."
New-AzResourceGroupDeployment -Name ((Get-ChildItem $TemplateFile).BaseName + '-' + ((Get-Date).ToUniversalTime()).ToString('MMdd-HHmm')) `
                                    -ResourceGroupName $ResourceGroupName `
                                    -TemplateFile $TemplateFile `
                                    -TemplateParameterFile $TemplateParametersFile `
                                    -Force -Verbose `
                                    -ErrorVariable ErrorMessages
if ($ErrorMessages) {
    Write-Output '', 'Template deployment returned the following errors:', @(@($ErrorMessages) | ForEach-Object { $_.Exception.Message.TrimEnd("`r`n") })
}