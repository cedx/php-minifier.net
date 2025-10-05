Write-Host "Building the project..."
$configuration = $release ? "Release" : "Debug"
dotnet build PhpMinifier.slnx --configuration=$configuration
