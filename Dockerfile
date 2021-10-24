FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./", "./"]

RUN dotnet restore "WeatherMonitor.Host/WeatherMonitor.Host.csproj"

WORKDIR "/src/WeatherMonitor.Host"
RUN dotnet build "WeatherMonitor.Host.csproj" -c Release -o /app/build

WORKDIR "/src/WeatherMonitor.Host"
RUN dotnet test ../WeatherMonitor.Core.Tests/

FROM build AS publish
RUN dotnet publish "WeatherMonitor.Host.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "WeatherMonitor.Host.dll"]
