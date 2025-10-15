FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/WeatherNow.Api/WeatherNow.Api.csproj", "src/WeatherNow.Api/"]
COPY ["src/WeatherNow.Application/WeatherNow.Application.csproj", "src/WeatherNow.Application/"]
COPY ["src/WeatherNow.Domain/WeatherNow.Domain.csproj", "src/WeatherNow.Domain/"]
COPY ["src/WeatherNow.Infrastructure/WeatherNow.Infrastructure.csproj", "src/WeatherNow.Infrastructure/"]


RUN dotnet restore "src/WeatherNow.Api/WeatherNow.Api.csproj"
COPY . .
WORKDIR "/src/src/WeatherNow.Api"
RUN dotnet build "./WeatherNow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WeatherNow.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherNow.Api.dll"]
