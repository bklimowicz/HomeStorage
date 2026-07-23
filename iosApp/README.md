# HomeStorage — iOS app

A native **SwiftUI** client for the HomeStorage API, styled to match the Blazor web app
(dark deep-blue theme). It manages **products** and **locations** and is the foundation for the
"check it while I'm in the shop" use case — later it can cache data / work offline.

## Screens

- **Products** — grid of cards (name, quantity pill, description, producer, location tag) with create / edit / delete.
- **Locations** — list with create / edit / delete (a location in use can't be deleted — the API's 409 is surfaced).
- **Settings** — set the API base URL (persisted).

## Requirements

- Xcode 16+ (the project targets iOS 17).
- [XcodeGen](https://github.com/yonatk/XcodeGen) to generate the Xcode project from `project.yml`:
  ```bash
  brew install xcodegen
  ```

## Generate & run

```bash
cd iosApp
xcodegen generate
open HomeStorage.xcodeproj
```

Then pick a simulator and run (⌘R).

## Pointing at the API

The API base URL is configurable in the **Settings** tab (persisted in `UserDefaults`):

- **Simulator → API on your Mac:** run the API (`dotnet run --project src/HomeStorage.Api`, listens on
  `http://localhost:5080`) and keep the default `http://localhost:5080`.
- **Real device / production:** use your Cloudflare Tunnel hostname, e.g. `https://api.yourdomain.com`
  (add an `api` public hostname → `http://api:8080` in the tunnel, per `docs/DEPLOY-RaspberryPi.md`).

> Plain HTTP is allowed only for local networking (ATS `NSAllowsLocalNetworking`). Public access goes over
> HTTPS via the tunnel.

## Project layout

```
HomeStorage/
  App/         HomeStorageApp.swift, RootView.swift (TabView)
  Design/      Theme.swift (palette + components ported from app.css)
  Models/      Product, Location, request payloads (Codable)
  Networking/  API.swift (async/await URLSession client, configurable base URL)
  Features/    Products/, Locations/, Settings/
  Support/     small extensions
  Assets.xcassets
project.yml    XcodeGen spec (source of truth for the Xcode project)
```

The `.xcodeproj` and the generated `Info.plist` are git-ignored — regenerate with `xcodegen generate`.
