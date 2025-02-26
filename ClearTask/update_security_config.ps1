# Path to the config file
$configPath = "Platforms/Android/Resources/xml/network_security_config.xml"
$appSettingsPath = "appsettings.json"

# Read the JSON config
$json = Get-Content $appSettingsPath | ConvertFrom-Json

# Extract the domain value
$domain = $json.NetworkSecurity.Domain

# Ensure the domain is valid
if (-not $domain -or $domain -eq "") {
    Write-Host "Error: No valid domain found in appsettings.json"
    exit 1
}

Write-Host "Replacing {{DOMAIN}} with: $domain"

# Replace placeholder in network_security_config.xml
(Get-Content $configPath) -replace "{{DOMAIN}}", $domain | Set-Content $configPath
