# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore ./TesisInventory.API/TesisInventory.API.csproj
RUN dotnet publish ./TesisInventory.API/TesisInventory.API.csproj -c Release -o /app/publish

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080


ENTRYPOINT ["dotnet", "TesisInventory.API.dll"]
