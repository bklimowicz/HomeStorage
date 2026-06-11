# HomeStorage

A small home inventory app: track **products** (name, quantity, optional description/producer) and the
**locations** where you keep them (e.g. *"Drawer in the wardrobe"*). Every product must be assigned to a
location.

Built with **.NET 10**, orchestrated with **.NET Aspire** for local development, persisted in **PostgreSQL**.
Self-hosted for free on a **Raspberry Pi** (Docker Compose + Cloudflare Tunnel), with Azure Container Apps as
an optional alternative.

## Solution layout

| Project | Description |
|---|---|
| `src/HomeStorage.Core` | Domain (entities, value objects), EF Core `DbContext`, repositories, migrations |
| `src/HomeStorage.Api` | Minimal API — CRUD for `/products` and `/locations` |
| `src/HomeStorage.Web` | Blazor Server UI |
| `src/HomeStorage.AppHost` | Aspire orchestration (Postgres + API + Web) |
| `src/HomeStorage.ServiceDefaults` | Shared Aspire defaults (telemetry, health checks, service discovery) |
| `tests/HomeStorage.Tests` | xUnit + Shouldly unit tests |

## Run locally (development)

Prerequisites: [.NET 10 SDK](https://dotnet.microsoft.com/download) and a container runtime (Docker / Podman).

```bash
dotnet run --project src/HomeStorage.AppHost
```

This starts the Aspire dashboard and brings up PostgreSQL, the API, and the Web UI. The database schema is
created automatically on API startup (EF Core migrations). Open the Web endpoint shown in the dashboard,
create a location, then add a product to it.

## Test

```bash
dotnet test
```

## Deploy (production)

### Raspberry Pi — self-hosted, free (recommended)

Runs the whole stack as Docker containers behind a free Cloudflare Tunnel, with auto-restart and a hardware
watchdog so it stays up unattended. Images are built by GitHub Actions (multi-arch) and pushed to GHCR, so
the Pi only pulls them. See **[docs/DEPLOY-RaspberryPi.md](docs/DEPLOY-RaspberryPi.md)**.

```bash
cp infrastructure/.env.example infrastructure/.env   # POSTGRES_PASSWORD, TUNNEL_TOKEN, IMAGE_PREFIX
docker compose -f infrastructure/docker-compose.yaml pull
docker compose -f infrastructure/docker-compose.yaml up -d
```

### Azure Container Apps — optional alternative

`azd up` deploys PostgreSQL, the API, and the Web app as containers in a Container Apps environment.
Mostly cheap (apps scale to zero), but note an Azure Container Registry (~$5/mo) and an always-on database
container are **not** free — see the discussion in the deploy doc. Requires
[Azure Developer CLI (`azd`)](https://aka.ms/azd).

## Configuration

The DbContext connection is named `homestorage`. Locally it is provided by the Aspire AppHost; if you run the
API on its own it falls back to `ConnectionStrings:homestorage` in `appsettings.Development.json`.
