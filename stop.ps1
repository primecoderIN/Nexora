# stop.ps1
# Shuts down the Shipwise local development environment

Write-Host "🛑 Stopping Shipwise Infrastructure..." -ForegroundColor Yellow

docker-compose down

Write-Host "✅ Infrastructure safely stopped." -ForegroundColor Green
