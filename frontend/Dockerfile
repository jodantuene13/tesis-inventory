# Etapa de build
FROM node:22-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build

# Etapa runtime
FROM caddy:2-alpine
WORKDIR /srv

# Ajustá esta ruta si el nombre real del proyecto en dist cambia
COPY --from=build /app/dist/inventory-app /usr/share/caddy

EXPOSE 80