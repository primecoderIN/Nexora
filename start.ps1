# start.ps1
# Bootstraps the Nexora local development environment

Write-Host "[Start] Starting Nexora Infrastructure..." -ForegroundColor Cyan

# Ensure .env exists
if (-not (Test-Path ".env")) {
    Write-Host "[Warn] No .env file found. Copying from .env.example..." -ForegroundColor Yellow
    Copy-Item ".env.example" -Destination ".env"
    Write-Host "[OK] Created .env file. Please review it if you need custom passwords." -ForegroundColor Green
}

# Spin up docker-compose in detached mode
Write-Host "[Docker] Starting Docker containers..." -ForegroundColor Cyan
docker-compose up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n[OK] Infrastructure is up and running!" -ForegroundColor Green
    Write-Host "----------------------------------------------------"
    Write-Host "[DB] PostgreSQL : localhost:5432"
    Write-Host "[DB] pgAdmin    : http://localhost:5050 (admin@nexora.com / admin)"
    Write-Host "[Cache] Redis   : localhost:6379"
    Write-Host "[Mail] Mailpit  : http://localhost:8025"
    Write-Host "[S3] MinIO      : http://localhost:9001 (API: 9000)"
    Write-Host "[Auth] Keycloak : http://localhost:8080 (admin/admin)"
    Write-Host "[API] Nexora    : http://localhost:5110/swagger"
    Write-Host "----------------------------------------------------"
    Write-Host "Run .\stop.ps1 to shut down the environment." -ForegroundColor Gray
} else {
    Write-Host "`n[Error] Failed to start Docker containers. Make sure Docker Desktop is running." -ForegroundColor Red
}
