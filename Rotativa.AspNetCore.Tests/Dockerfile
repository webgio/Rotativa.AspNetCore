# Runs the full test suite (unit + integration) on Linux.
# Build from the repository root:
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

ENV DOTNET_NOLOGO=1 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1 \
    CI=true \
    ROTATIVA_DEMO_URL=http://localhost:5000 \
    ROTATIVA_CHROME_BINARY=/usr/bin/chromium

WORKDIR /src
COPY . .

RUN dotnet build Rotativa.AspNetCore.DemoApp \
    && dotnet build Rotativa.AspNetCore.Tests \
    && ln -sf /usr/local/bin/wkhtmltopdf Rotativa.AspNetCore.DemoApp/wwwroot/Rotativa/wkhtmltopdf \
    && ln -sf /usr/local/bin/wkhtmltoimage Rotativa.AspNetCore.DemoApp/wwwroot/Rotativa/wkhtmltoimage

CMD ["bash", "Rotativa.AspNetCore.Tests/run-tests.sh"]
