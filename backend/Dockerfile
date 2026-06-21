FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["TesisInventory.API/TesisInventory.API.csproj", "TesisInventory.API/"]
COPY ["TesisInventory.Application/TesisInventory.Application.csproj", "TesisInventory.Application/"]
COPY ["TesisInventory.Domain/TesisInventory.Domain.csproj", "TesisInventory.Domain/"]
COPY ["TesisInventory.Infrastructure/TesisInventory.Infrastructure.csproj", "TesisInventory.Infrastructure/"]

RUN dotnet restore "TesisInventory.API/TesisInventory.API.csproj"

COPY . .

RUN dotnet publish "TesisInventory.API/TesisInventory.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TesisInventory.API.dll"]
