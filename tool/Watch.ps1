Write-Host "Watching for file changes..."
Push-Location src
dotnet watch build
Pop-Location
