# Build context is the repository root: docker build -f src/HomeStorage.Web/Dockerfile .
# Base images are multi-arch, so building on the Raspberry Pi (arm64) just works.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore (copy only project files first for better layer caching)
COPY HomeStorage.sln ./
COPY src/HomeStorage.ServiceDefaults/HomeStorage.ServiceDefaults.csproj src/HomeStorage.ServiceDefaults/
COPY src/HomeStorage.Web/HomeStorage.Web.csproj src/HomeStorage.Web/
RUN dotnet restore src/HomeStorage.Web/HomeStorage.Web.csproj

# Publish
COPY src/ src/
RUN dotnet publish src/HomeStorage.Web/HomeStorage.Web.csproj -c Release -o /app /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*
COPY --from=build /app .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=3 \
    CMD curl -f http://localhost:8080/alive || exit 1
ENTRYPOINT ["dotnet", "HomeStorage.Web.dll"]
