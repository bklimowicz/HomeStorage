# Deploying HomeStorage on a Raspberry Pi

Self-hosted, $0/month, and unattended. The stack runs as Docker containers
(`postgres` + `api` + `web` + `cloudflared`) defined in
[`infrastructure/docker-compose.yaml`](../infrastructure/docker-compose.yaml).
Public access is provided by a free **Cloudflare Tunnel** — no port forwarding,
no static IP, TLS handled by Cloudflare.

```
Internet ──https──> Cloudflare ──tunnel──> cloudflared ─┐ (Docker network)
                                                        ├─> web  (Blazor)  :8080
                                                        └─> web ──> api ──> postgres
```

## 1. Prerequisites on the Pi

- Raspberry Pi OS **64-bit** (arm64) recommended — Pi 3/4/5.
- Docker + Compose plugin:
  ```bash
  curl -fsSL https://get.docker.com | sh
  sudo usermod -aG docker $USER   # then log out/in
  sudo systemctl enable docker    # start Docker on boot
  ```

## 2. Get the code and configure secrets

```bash
git clone <your-repo-url> homestorage
cd homestorage
cp infrastructure/.env.example infrastructure/.env
# edit infrastructure/.env -> set a strong POSTGRES_PASSWORD and the TUNNEL_TOKEN (next step)
```

## 3. Create the Cloudflare Tunnel (one-time, free)

You need a domain on Cloudflare (a cheap domain works; Cloudflare itself is free).

1. Cloudflare **Zero Trust** dashboard → **Networks → Tunnels → Create a tunnel** → *Cloudflared*.
2. Name it, **Save**, then copy the **token** shown in the install command into
   `TUNNEL_TOKEN` in `infrastructure/.env`.
3. Open the tunnel → **Public Hostnames → Add a public hostname**:
   - Subdomain/domain: e.g. `storage.yourdomain.com`
   - Service: **HTTP** → `web:8080`
4. (Later, for the iOS app) add a second hostname, e.g. `api.yourdomain.com` → **HTTP** → `api:8080`.

The hostname → service routing lives in the dashboard; the Pi only needs the token.

## 4. Images: built by GitHub Actions, pulled on the Pi

You don’t compile on the Pi. The workflow
[`.github/workflows/build-images.yml`](../.github/workflows/build-images.yml) builds **multi-arch
(amd64 + arm64)** images for the API and Web on every push to `master` (and on manual dispatch) and pushes
them to **GitHub Container Registry (GHCR)** as:

- `ghcr.io/<owner>/homestorage-api`
- `ghcr.io/<owner>/homestorage-web`

One-time setup:

1. Set `IMAGE_PREFIX=ghcr.io/<owner>` in `infrastructure/.env` (lowercase GitHub username/org).
2. Make the Pi able to pull. Either:
   - **Make the packages public** (GitHub → your profile → Packages → each package → *Package settings* →
     *Change visibility* → Public), or
   - **Authenticate on the Pi** with a token that has `read:packages`:
     ```bash
     echo <YOUR_PAT> | docker login ghcr.io -u <owner> --password-stdin
     ```

## 5. Launch

```bash
docker compose -f infrastructure/docker-compose.yaml pull
docker compose -f infrastructure/docker-compose.yaml up -d
```

After it’s up:

- The API applies EF Core migrations automatically on startup (creates the schema).
- Visit `https://storage.yourdomain.com` — create a location, then a product.

> First push to `master` hasn’t run yet / no images published? You can build on the Pi instead:
> ```bash
> docker compose -f infrastructure/docker-compose.yaml -f infrastructure/docker-compose.build.yaml up -d --build
> ```

Check status / logs (health shows up in `ps` via the containers’ `/alive` probe):
```bash
docker compose -f infrastructure/docker-compose.yaml ps
docker compose -f infrastructure/docker-compose.yaml logs -f api
```

## 6. Make it survive crashes and freezes (unattended)

**Container/-reboot recovery — already handled:** every service uses
`restart: always`, and `systemctl enable docker` brings them all back after a
power loss or reboot. You never start anything by hand.

**Whole-Pi freeze recovery — hardware watchdog.** This directly fixes the
"every now and then the Pi needs a restart / I can’t reach it" problem: if the
kernel hangs, the watchdog reboots the Pi, and Docker brings the app back.

```bash
# 1) Enable the hardware watchdog
echo 'dtparam=watchdog=on' | sudo tee -a /boot/firmware/config.txt   # older OS: /boot/config.txt
sudo apt-get update && sudo apt-get install -y watchdog

# 2) Configure it
sudo tee -a /etc/watchdog.conf >/dev/null <<'EOF'
watchdog-device = /dev/watchdog
watchdog-timeout = 15
max-load-1 = 24
EOF

# 3) Enable on boot
sudo systemctl enable --now watchdog
```

Now a frozen Pi auto-reboots within ~15s and the stack restarts itself.

## 7. Updating the app

Push to `master` → GitHub Actions builds and publishes new images. On the Pi:

```bash
git pull   # only needed if compose/env files changed
docker compose -f infrastructure/docker-compose.yaml pull
docker compose -f infrastructure/docker-compose.yaml up -d
```

Data lives in the `pgdata` Docker volume and is preserved across updates.

## 8. Notes

- **Data persistence:** the `pgdata` volume keeps your data; don’t run
  `docker compose down -v` unless you want to wipe the database.
- **Backups:** occasionally dump the DB —
  `docker exec homestorage-postgres pg_dump -U postgres homestorage > backup.sql`.
- **Blazor Server + mobile:** the website needs a live connection (SignalR). It
  works fine over the tunnel when online; the planned native iOS app will talk to
  the API directly (`api.yourdomain.com`) and can cache/operate offline.
- **Remaining risks** are only home power/internet outages — outside the app’s control.
