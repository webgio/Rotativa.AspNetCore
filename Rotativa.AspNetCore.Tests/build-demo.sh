#!/usr/bin/env bash
# Builds the demo app and the test project. Run from the repository root.
# With UseRotativaPackage=true, packs the library and builds the demo app against the .nupkg instead of the project.
set -euo pipefail

if [ "${UseRotativaPackage:-false}" = "true" ]; then
    dotnet pack Rotativa.AspNetCore -c Release -o artifacts/packages -p:GeneratePackageOnBuild=false
    version=$(dotnet msbuild Rotativa.AspNetCore/Rotativa.AspNetCore.csproj -getProperty:Version | tr -d '\r')
    dotnet restore Rotativa.AspNetCore.DemoApp --configfile Rotativa.AspNetCore.Tests/package-test.nuget.config -p:RotativaPackageVersion="$version"
    dotnet build Rotativa.AspNetCore.DemoApp --no-restore -p:RotativaPackageVersion="$version"

    # Make sure the demo app really uses the package, not the project.
    deps=Rotativa.AspNetCore.DemoApp/bin/Debug/net10.0/Rotativa.AspNetCore.DemoApp.deps.json
    if ! grep -A1 "\"Rotativa.AspNetCore/$version\": {" "$deps" | grep -q '"type": "package"'; then
        echo "The demo app does not reference the Rotativa.AspNetCore $version package." >&2
        exit 1
    fi
else
    dotnet build Rotativa.AspNetCore.DemoApp
fi

dotnet build Rotativa.AspNetCore.Tests
