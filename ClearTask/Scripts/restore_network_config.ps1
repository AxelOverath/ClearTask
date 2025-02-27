# Paths
$configPath = "Platforms/Android/Resources/xml/network_security_config.xml"
$backupPath = "$configPath.bak"

# Restore the backup if it exists
if (Test-Path $backupPath) {
    Write-Host "Restoring network_security_config.xml to original state..."
    Move-Item -Path $backupPath -Destination $configPath -Force
} else {
    Write-Host "No backup found. Nothing to restore."
}
