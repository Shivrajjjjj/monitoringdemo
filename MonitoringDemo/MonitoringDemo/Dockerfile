# ============================
# Stage 1: Base runtime image
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# ============================
# Stage 2: Build application
# ============================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only csproj first to leverage Docker caching
COPY MonitoringDemo.csproj ./MonitoringDemo/
WORKDIR /src/MonitoringDemo
RUN dotnet restore

# Copy everything else
COPY . ./

# Publish application
RUN dotnet publish -c Release -o /app/publish

# ============================
# Stage 3: Final runtime image
# ============================
FROM base AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_EnableDiagnostics=1

ENTRYPOINT ["dotnet", "MonitoringDemo.dll"]