# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["API/API.csproj", "API/"]
RUN dotnet restore "API/API.csproj"

COPY . .

# publicar dependencias
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish 

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
ENTRYPOINT ["dotnet", "API.dll"]