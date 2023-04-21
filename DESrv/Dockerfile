#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DESrv/DESrv.csproj", "DESrv/"]
COPY ["DESrv.Configuration/DESrv.Configuration.csproj", "DESrv.Configuration/"]
COPY ["DESrv.Logging/DESrv.Logging.csproj", "DESrv.Logging/"]
COPY ["DESrv.Localization/DESrv.Localization.csproj", "DESrv.Localization/"]
COPY ["DESrv.PDK/DESrv.PDK.csproj", "DESrv.PDK/"]
COPY ["DESrv.Updater/DESrv.Updater.csproj", "DESrv.Updater/"]
RUN dotnet restore "DESrv/DESrv.csproj"
COPY . .
WORKDIR "/src/DESrv"
RUN dotnet build "DESrv.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DESrv.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .