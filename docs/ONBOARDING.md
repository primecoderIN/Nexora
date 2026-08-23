# Onboarding & Setup

Welcome to the Nexora engineering team! We use Docker Compose to ensure every developer has the exact same local environment, eliminating "it works on my machine" issues.

## Local Environment

To spin up the entire infrastructure (PostgreSQL, Redis, Mailpit, MinIO, Keycloak, pgAdmin, and the API itself), simply run the startup script from the root directory:

```powershell
.\start.ps1
```

This script will automatically generate your `.env` file if it's missing (by copying `.env.example`) and boot all containers in the background. 

> **Security Note:** We do not hardcode credentials in `docker-compose.yml`. All passwords, database connections, and URLs are injected dynamically from your local `.env` file.

To stop the environment, run:
```powershell
.\stop.ps1
```

## Service URLs

Once running, the following services are available:

| Service | URL | Notes |
|---------|-----|-------|
| **Nexora API** | http://localhost:5110/swagger | Swagger UI for testing all endpoints |
| **API Health** | http://localhost:5110/api/health | Live health checks for all infrastructure |
| **Keycloak** | http://localhost:8080 | Auth server admin UI (admin / admin) |
| **pgAdmin** | http://localhost:5050 | PostgreSQL Web GUI (admin@nexora.com / admin) |
| **Mailpit** | http://localhost:8025 | Catch-all email inbox for dev |
| **MinIO** | http://localhost:9001 | Object storage console |
| **PostgreSQL** | localhost:5432 | Database (can also use DBeaver) |
| **Redis** | localhost:6379 | Cache (use RedisInsight) |

## Keycloak Setup (First Time Only)

Before the API can authenticate any user, you must create a Keycloak realm:

1. Open http://localhost:8080 and log in with `admin` / `admin`.
2. Click **Create Realm** and set the name to exactly `nexora`.
3. Create a test user inside the `nexora` realm with a verified email.
4. Use the Swagger UI at http://localhost:5110/swagger to test endpoints.

## Test Accounts
*(Test accounts will be populated here once the Identity module is built).*
