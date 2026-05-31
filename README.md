# SAR Coordination Platform

> A microservices-based platform for coordinating search and rescue operations, volunteers, and resources in real time.

![Build Status](https://img.shields.io/badge/build-passing-brightgreen)
![Version](https://img.shields.io/badge/version-0.1.0-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![React](https://img.shields.io/badge/React-19.1.0-61DAFB)

---

## Overview

The SAR Coordination Platform is a full-stack system that enables emergency coordinators and dispatchers to manage search and rescue operations from a single centralized hub. It handles the full operational lifecycle — from creating and approving missions to assigning volunteers, tracking resources, and generating post-operation efficiency reports. The platform is built for rescue organizations that need to coordinate distributed teams across multiple geographic districts under time pressure.

---

## Features

- **Operation lifecycle management** — create, approve, monitor, and close SAR events with status tracking
- **Role-based access control** — distinct roles for Coordinators, Dispatchers, Volunteers, and Administrators
- **Volunteer & group management** — register volunteers, form groups, assign to events, and generate QR identification codes
- **Resource tracking** — maintain an equipment catalog and allocate resources to active operations
- **Task assignment** — define operation tasks, assign workers, and track completion status
- **Interactive map** — visualize operation locations and pick coordinates via an embedded Leaflet map
- **Async messaging** — RabbitMQ-backed inter-service communication for real-time status propagation
- **Reporting** — generate PDF operation reports with computed efficiency coefficients

---

## Tech Stack

**Backend**
- C# 8 / .NET 8.0 (ASP.NET Core)
- Entity Framework Core 9 with PostgreSQL
- MediatR (CQRS pattern)
- JWT + ASP.NET Core Identity (authentication)
- RabbitMQ 3 (async messaging)
- Swagger / Swashbuckle (API docs)

**Frontend**
- TypeScript / React 19 / Next.js 15
- PrimeReact 10 + PrimeFlex (UI components)
- Leaflet 1.9 (map visualization)
- Axios (HTTP client)

**Infrastructure**
- Docker / Docker Compose
- PostgreSQL (containerized)

---

## Getting Started

### Prerequisites

| Tool | Minimum version |
|------|----------------|
| Docker Desktop | 4.x |
| Docker Compose | 2.x |
| Node.js *(frontend dev only)* | 18.x |
| .NET SDK *(backend dev only)* | 8.0 |

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/MrSampy/Platform-for-coordination-of-search-and-rescue-operations.git
cd Platform-for-coordination-of-search-and-rescue-operations
```

2. **Build and start all services**

```bash
docker-compose up --build
```

This starts PostgreSQL, RabbitMQ, all five backend microservices, and serves the frontend.

3. **Verify the stack is running**

| Service | URL |
|---------|-----|
| API Gateway | http://localhost:5065 |
| Auth Service | http://localhost:5107 |
| Utils Service | http://localhost:5031 |
| Operations Service | http://localhost:5160 |
| Volunteers Service | http://localhost:5083 |
| RabbitMQ Management | http://localhost:15672 |
| Frontend (dev) | http://localhost:3000 |

### Configuration

**Frontend** — create `Central Control System/frontend/.env`:

```env
REACT_APP_API_BASE_URL=http://localhost:5065/gateway.integration.api
```

**Backend** — each service reads from its own `appsettings.json`. Key values:

```jsonc
// Shared across services
"ConnectionStrings": {
  "PostgreSQLConnection": "Host=postgresql-server;Port=5432;Database=CSSDB;Username=sa;Password=P@ssw0rd123"
},
"JWTConfiguration": {
  "ValidAudience": "http://localhost:5107",
  "ValidIssuer": "http://localhost:5107",
  "Secret": "<your-jwt-secret>"
},
"OriginUrls": ["http://localhost:3000", "http://localhost:3001"]
```

RabbitMQ defaults: `guest / guest` on port `5672` (management UI on `15672`).

---

## Usage

### Create and manage an operation (via Gateway API)

```bash
# Authenticate and obtain a JWT token
curl -X POST http://localhost:5065/gateway.integration.api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email": "coordinator@example.com", "password": "P@ssw0rd123"}'

# Create a new SAR operation
curl -X POST http://localhost:5065/gateway.integration.api/events \
  -H "Authorization: Bearer <token>" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Mountain Search — Sector 4",
    "description": "Missing hiker, last seen at trail marker 12",
    "longitude": 24.031,
    "latitude": 49.842,
    "eventTypeId": 1
  }'
```

### Run the frontend in development mode

```bash
cd "Central Control System/frontend"
npm install
npm start        # http://localhost:3000
npm test         # run test suite
npm run build    # production build
```

---

## Project Structure

```
.
├── Central Control System/
│   ├── AuthService/            # User auth, JWT issuance, role management
│   ├── Gateway/                # API Gateway — routes requests, aggregates responses
│   ├── OperationsService/      # SAR events, tasks, assignments, messages (CQRS)
│   ├── UtilsService/           # Districts, resources, measurement units
│   ├── VolunteerService/       # Volunteer registration, groups, event participation
│   ├── frontend/               # React/Next.js single-page application
│   │   ├── src/
│   │   │   ├── components/     # Reusable UI components (map, forms, tables)
│   │   │   ├── pages/          # Route-level pages (Dashboard, Operations, Reports…)
│   │   │   └── services/       # Axios-based API service modules
│   │   └── package.json
│   ├── docker-compose.yml      # Full stack orchestration
│   └── sql.Dockerfile          # PostgreSQL image with schema init
├── LICENSE
└── README.md
```

Each backend service follows Clean Architecture layers: `Domain → Application (CQRS) → Infrastructure → API`.

---

## Contributing

1. Fork the repository and create a feature branch from `main`:

```bash
git checkout -b feature/my-feature
```

2. Make your changes. Keep commits small and focused:

```bash
git commit -m "Add volunteer QR export endpoint"
```

3. Ensure the project builds and existing tests pass before opening a PR:

```bash
# Backend (from a service directory)
dotnet build
dotnet test

# Frontend
npm test
```

4. Open a pull request against `main`. Describe *what* changed and *why*. Reference any related issues.

**Code style:**
- Backend: follow standard C# conventions; use `var` where the type is obvious
- Frontend: ESLint config is included; run `npm run lint` before pushing
- No commented-out code in PRs

---

## License

This project is licensed under the [MIT License](LICENSE).
