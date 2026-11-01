# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY nuget.github.config nuget.config
COPY src/ src/
RUN --mount=type=secret,id=nuget_github,env=NuGetPackageSourceCredentials_github \
    dotnet publish src/MyCarDb.Web/MyCarDb.Web.csproj -c Release -o /app

FROM caddy:2-alpine AS runtime
COPY Caddyfile /etc/caddy/Caddyfile
COPY --from=build /app/wwwroot /usr/share/caddy
