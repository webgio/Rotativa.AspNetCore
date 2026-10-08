#!/usr/bin/env bash
# Starts the demo app, runs the test suite against it, then stops the app.
# Run from the repository root. Expects the projects to be built already.
set -euo pipefail

url="${ROTATIVA_DEMO_URL:-http://localhost:5000}"

(cd Rotativa.AspNetCore.DemoApp && exec dotnet run --no-build --urls "$url") &
app_pid=$!
trap 'kill $app_pid 2>/dev/null || true' EXIT

for _ in $(seq 1 60); do
    curl -sf -o /dev/null "$url" && break
    sleep 1
done

dotnet test Rotativa.AspNetCore.Tests --no-build --logger "console;verbosity=normal"
