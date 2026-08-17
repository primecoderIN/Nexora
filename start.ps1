# start.ps1
# Bootstraps the Shipwise local development environment

Write-Host "🚀 Starting Shipwise Infrastructure..." -ForegroundColor Cyan

# Ensure .env exists
if (-not (Test-Path ".env")) {
    Write-Host "⚠️ No .env file found. Copying from .env.example..." -ForegroundColor Yellow
    Copy-Item ".env.example" -Destination ".env"
    Write-Host "✅ Created .env file. Please review it if you need custom passwords." -ForegroundColor Green
}

# Spin up docker-compose in detached mode
Write-Host "🐳 Starting Docker containers..." -ForegroundColor Cyan
docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n✅ Infrastructure is up and running!" -ForegroundColor Green
    Write-Host "----------------------------------------------------"
    Write-Host "📦 PostgreSQL : localhost:5432"
    Write-Host "🗄️ Redis       : localhost:6379"
    Write-Host "📧 Mailpit    : http://localhost:8025"
    Write-Host "🪣 MinIO      : http://localhost:9001 (API: 9000)"
    Write-Host "----------------------------------------------------"
    Write-Host "Run .\stop.ps1 to shut down the environment." -ForegroundColor Gray
} else {
    Write-Host "`n❌ Failed to start Docker containers. Make sure Docker Desktop is running." -ForegroundColor Red
}
