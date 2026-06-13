FROM node:22-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci

COPY . .
RUN npm run build -- --configuration production

FROM nginx:alpine AS final
COPY --from=build /app/dist/inventory-app/browser /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

# Escribe env.js con la API_URL del entorno en runtime, luego inicia Nginx.
# Evita el script .sh separado para no tener problemas de CRLF en Windows.
CMD ["/bin/sh", "-c", "printf 'window.__env = { apiUrl: \"%s\" };\\n' \"${API_URL:-http://localhost:5139}\" > /usr/share/nginx/html/assets/env.js && nginx -g 'daemon off;'"]
