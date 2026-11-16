# Brownfield

This repository contains the initial application foundation. Application code
lives under `source\`, with the .NET API in `source\backend\` and the React
client in `source\frontend\`.

Run the commands below in PowerShell from the repository root.

## Prerequisites

- .NET 10 SDK
- Node.js 22 or newer and npm
- No separate SQLite server is required; SQLite is embedded.

## Restore and build

```powershell
dotnet restore source\backend\Brownfield.slnx
dotnet build source\backend\Brownfield.slnx --no-restore
npm ci --prefix source\frontend
npm run build --prefix source\frontend
```

## Test

```powershell
dotnet test source\backend\Brownfield.slnx
```

This runs an HTTP smoke test that verifies `/health` returns HTTP 200 with
`{"status":"ok"}`.

## Run locally

Start the API in one terminal:

```powershell
dotnet run --project source\backend\Brownfield.Api --urls http://localhost:5000
```

Start the client in another terminal:

```powershell
npm run dev --prefix source\frontend
```

Open the Vite URL shown in the terminal (normally `http://localhost:5173`).
The client uses the Vite development proxy to display the API health status,
so keep the backend running while using the client.
