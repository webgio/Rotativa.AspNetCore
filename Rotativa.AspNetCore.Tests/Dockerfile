# Runs the full test suite (unit + integration) on Linux.
# Build from the repository root (add --build-arg USE_ROTATIVA_PACKAGE=true to test the packed .nupkg):
#   docker build -f Rotativa.AspNetCore.Tests/Dockerfile -t rotativa-tests .
#   docker run --rm rotativa-tests
FROM debian:bookworm-slim

ARG TARGETARCH
ARG WKHTMLTOX_VERSION=0.12.6.1-3

RUN apt-get update \
    && apt-get install -y --no-install-recommends ca-certificates curl libicu72 chromium chromium-driver \
    && curl -fsSL -o /tmp/wkhtmltox.deb \
        "https://github.com/wkhtmltopdf/packaging/releases/download/${WKHTMLTOX_VERSION}/wkhtmltox_${WKHTMLTOX_VERSION}.bookworm_${TARGETARCH}.deb" \
    && apt-get install -y --no-install-recommends /tmp/wkhtmltox.deb \
    && rm -rf /tmp/wkhtmltox.deb /var/lib/apt/lists/*

RUN curl -fsSL https://dot.net/v1/dotnet-install.sh -o /tmp/dotnet-install.sh \
    && bash /tmp/dotnet-install.sh --channel 10.0 --install-dir /usr/share/dotnet \
    && ln -s /usr/share/dotnet/dotnet /usr/local/bin/dotnet \
    && rm /tmp/dotnet-install.sh

# true: pack the library and run the demo app against the .nupkg instead of the project.
ARG USE_ROTATIVA_PACKAGE=false

ENV DOTNET_NOLOGO=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    CI=true \
    ROTATIVA_DEMO_URL=http://localhost:5000 \
    ROTATIVA_CHROME_BINARY=/usr/bin/chromium \
    UseRotativaPackage=${USE_ROTATIVA_PACKAGE}

WORKDIR /src
COPY . .

RUN if [ "$UseRotativaPackage" = "true" ]; then \
        dotnet pack Rotativa.AspNetCore -c Release -o artifacts/packages -p:GeneratePackageOnBuild=false \
        && version=$(dotnet msbuild Rotativa.AspNetCore/Rotativa.AspNetCore.csproj -getProperty:Version) \
        && dotnet restore Rotativa.AspNetCore.DemoApp --configfile Rotativa.AspNetCore.Tests/package-test.nuget.config -p:RotativaPackageVersion=$version \
        && dotnet build Rotativa.AspNetCore.DemoApp --no-restore -p:RotativaPackageVersion=$version \
        && grep -A1 "\"Rotativa.AspNetCore/$version\": {" Rotativa.AspNetCore.DemoApp/bin/Debug/net10.0/Rotativa.AspNetCore.DemoApp.deps.json \
            | grep -q '"type": "package"' ; \
    else \
        dotnet build Rotativa.AspNetCore.DemoApp ; \
    fi \
    && dotnet build Rotativa.AspNetCore.Tests \
    && ln -sf /usr/local/bin/wkhtmltopdf Rotativa.AspNetCore.DemoApp/wwwroot/Rotativa/wkhtmltopdf \
    && ln -sf /usr/local/bin/wkhtmltoimage Rotativa.AspNetCore.DemoApp/wwwroot/Rotativa/wkhtmltoimage

CMD ["bash", "Rotativa.AspNetCore.Tests/run-tests.sh"]
