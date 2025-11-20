# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Motos/Motos.csproj", "Motos/"]
RUN dotnet restore "Motos/Motos.csproj"

COPY . .

# publicar dependencias
RUN dotnet publish "Motos/Motos.csproj" -c Release -o /app/publish 

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:7050 \
    DOTNET_RUNNING_IN_CONTAINER=true \
    ASPNETCORE_ENVIRONMENT=Development

COPY --from=build /app/publish .

# porta api
EXPOSE 7050

# comando init
ENTRYPOINT ["dotnet", "Motos.dll"]