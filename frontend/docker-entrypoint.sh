#!/bin/sh
set -e

# Genera el archivo env.js con las variables de entorno del contenedor.
# Angular lo carga antes de iniciar, permitiendo configurar la API URL
# sin recompilar la aplicación.
cat > /usr/share/nginx/html/assets/env.js <<EOF
window.__env = {
  apiUrl: "${API_URL:-http://localhost:5139}"
};
EOF

exec nginx -g "daemon off;"
