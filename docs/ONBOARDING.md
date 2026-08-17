# Onboarding & Setup

Welcome to the Shipwise engineering team! We use Docker Compose to ensure every developer has the exact same local environment, eliminating "it works on my machine" issues.

## Local Environment

To spin up the entire infrastructure (PostgreSQL, Redis, Mailpit, and MinIO), simply run the startup script from the root directory:

```powershell
.\start.ps1
```

This script will automatically generate your `.env` file if it's missing and boot all containers in the background.

To stop the environment, run:
```powershell
.\stop.ps1
```

## Test Accounts
*(Test accounts will be populated here once the Identity module is built).*
