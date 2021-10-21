FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WeatherMonitor/WeatherMonitor.csproj", "WeatherMonitor/"]
RUN dotnet restore "WeatherMonitor/WeatherMonitor.csproj"
COPY . .
WORKDIR "/src/WeatherMonitor"
RUN dotnet build "WeatherMonitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherMonitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherMonitor.dll"]
