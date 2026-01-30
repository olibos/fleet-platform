# ----------------------
# BASE IMAGE (SDK + AOT deps)
# ----------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS base-aot

# Install Native AOT prerequisites
RUN apt-get update && apt-get install -y --no-install-recommends \
        clang \
        gcc \
        libc6-dev \
        libstdc++-14-dev \
        make \
    && rm -rf /var/lib/apt/lists/*

# Set WORKDIR for builds
WORKDIR /src

# ----------------------
# NUGET CACHE IMAGE
# ----------------------
FROM base-aot AS nuget-cache

# Copy csproj(s) only to restore packages
COPY ["CregTariffImporter/CregTariffImporter.csproj", ""]

RUN dotnet restore "CregTariffImporter.csproj"

# ----------------------
# 3. BUILD IMAGE
# ----------------------
FROM nuget-cache AS build

ARG BUILD_CONFIGURATION=Release

# Copy the rest of the source
COPY ./CregTariffImporter/ .

# Publish Native AOT
RUN dotnet publish "CregTariffImporter.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    -p:PublishAot=true \
    -p:StripSymbols=true \
    -p:UseAppHost=true \
    -p:EnableCompressionInSingleFile=true \
    -p:TrimMode=full

# -------- RUNTIME --------
FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-noble-chiseled-extra AS final

WORKDIR /app
COPY --from=build /app/publish/CregTariffImporter .

# If you use a non-root user in Container Apps
USER $APP_UID

# Native binary → no dotnet
ENTRYPOINT ["./CregTariffImporter"]
