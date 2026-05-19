# Stage 1: Build Frontend (Angular)
FROM node:20 AS frontend-build
WORKDIR /app/frontend

# Copy dependencies manifest and install
COPY frontend/package*.json ./
RUN npm install

# Copy source files and build for production
COPY frontend/ ./
RUN npm run build -- --configuration production

# Stage 2: Build Backend (.NET 9.0)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS backend-build
WORKDIR /app/backend

# Copy csproj files for layer caching
COPY backend/TesisInventory.API/TesisInventory.API.csproj TesisInventory.API/
COPY backend/TesisInventory.Application/TesisInventory.Application.csproj TesisInventory.Application/
COPY backend/TesisInventory.Domain/TesisInventory.Domain.csproj TesisInventory.Domain/
COPY backend/TesisInventory.Infrastructure/TesisInventory.Infrastructure.csproj TesisInventory.Infrastructure/

# Restore dependencies
RUN dotnet restore TesisInventory.API/TesisInventory.API.csproj

# Copy the rest of the backend source and publish
COPY backend/ ./
RUN dotnet publish TesisInventory.API/TesisInventory.API.csproj -c Release -o /app/publish

# Stage 3: Final Runtime Image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Set timezone / locale (optional, good for LatAm apps)
ENV TZ=America/Argentina/Buenos_Aires

# Copy the published backend app
COPY --from=backend-build /app/publish .

# Copy the built Angular app into the wwwroot folder so ASP.NET can serve it
COPY --from=frontend-build /app/frontend/dist/inventory-app/browser ./wwwroot

# ASP.NET Core 8.0/9.0 defaults to port 8080
# Railway will detect this EXPOSE directive
EXPOSE 8080

ENTRYPOINT ["dotnet", "TesisInventory.API.dll"]
