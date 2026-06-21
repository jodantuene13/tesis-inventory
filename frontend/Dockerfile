FROM node:22-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration production

FROM nginx:alpine AS final
COPY --from=build /app/dist/inventory-app/browser /usr/share/nginx/html

# nginx:alpine procesa automáticamente los *.template de esta carpeta con envsubst
# antes de arrancar, reemplazando ${PORT} con el valor real.
COPY nginx.conf /etc/nginx/templates/default.conf.template

ENV PORT=80
EXPOSE 80

# 1. Crea assets/ por si no existe
# 2. Escribe env.js con la API_URL del entorno en runtime
# 3. Delega a docker-entrypoint.sh de nginx (procesa templates y arranca nginx)
CMD ["/bin/sh", "-c", "mkdir -p /usr/share/nginx/html/assets && echo \"window.__env = { apiUrl: '${API_URL:-http://localhost:5139}' };\" > /usr/share/nginx/html/assets/env.js && /docker-entrypoint.sh nginx -g 'daemon off;'"]
