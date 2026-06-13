// Este archivo es sobreescrito por docker-entrypoint.sh al iniciar el contenedor.
// En desarrollo local apunta a localhost.
window.__env = {
  apiUrl: 'http://localhost:5139'
};
