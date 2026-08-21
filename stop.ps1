# stop.ps1
# Shuts down the Nexora local development environment

Write-Host "🛑 Stopping Nexora Infrastructure..." -ForegroundColor Yellow

docker-compose down

Write-Host "✅ Infrastructure safely stopped." -ForegroundColor Green
