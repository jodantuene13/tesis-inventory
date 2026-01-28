# Guía de Pruebas - Autenticación con Google SSO

Esta guía describe los pasos necesarios para levantar el entorno de desarrollo y probar el flujo de inicio de sesión implementado.

## Prerequisitos
- XAMPP instalado y corriendo (MySQL).
- .NET SDK 9.0 instalado.
- Node.js instalado.
- Un `Client ID` de Google Cloud Console configurado (ver sección Configuración).

## 1. Configuración de Google Cloud Console
Para que el botón de "Iniciar sesión con Google" funcione, necesitas un Client ID real asociado a `http://localhost:4200`.

1. Ve a [Google Cloud Console](https://console.cloud.google.com/).
2. Crea un nuevo proyecto.
3. Configura la **Pantalla de consentimiento de OAuth** (Externo o Interno).
4. Crea Credenciales de tipo **ID de cliente de OAuth 2.0**.
    - Tipo de aplicación: Aplicación web.
    - Orígenes autorizados de JavaScript: `http://localhost:4200`
5. Copia el **ID de cliente**.

### Actualizar Código
1. **Frontend**: Abre `frontend/src/app/services/auth.ts` y reemplaza `YOUR_GOOGLE_CLIENT_ID...` con tu ID real.
2. **Backend**: Abre `backend/TesisInventory.API/appsettings.json` y reemplaza el valor en `Google:ClientId`.

## 2. Levantar el Entorno

### Base de Datos
Asegúrate de que MySQL esté corriendo en XAMPP.
Si no has creado el usuario de prueba, inserta uno manualmente en la base de datos para simular un usuario existente (ya que el sistema valida que el email exista):
```sql
USE tesis_inventory;
INSERT INTO Rol (nombreRol, descripcion) VALUES ('ADMIN', 'Administrador');
INSERT INTO Sede (nombreSede) VALUES ('Campus');
INSERT INTO Usuario (nombreUsuario, email, idRol, idSede, estado) 
VALUES ('Test User', 'tu_email@ucc.edu.ar', 1, 1, 1);
```
*(Reemplaza `tu_email@ucc.edu.ar` con el correo de Google que usarás para probar).*

### Backend (.NET)
Desde una terminal en `c:\xampp\htdocs\xampp\tesis-inventory\backend`:
```powershell
dotnet run --project TesisInventory.API
```
El backend iniciará en `https://localhost:7147` (o el puerto que indique la consola).

### Frontend (Angular)
Desde una terminal en `c:\xampp\htdocs\xampp\tesis-inventory\frontend`:
```powershell
npm install
npm start
```
La aplicación iniciará en `http://localhost:4200`.

## 3. Escenarios de Prueba

### Escenario A: Inicio de Sesión Exitoso
1. Abre `http://localhost:4200`. Deberías ser redirigido a `/login`.
2. Haz clic en el botón de Google e inicia sesión con el correo registrado en la BD (`@ucc.edu.ar`).
3. **Resultado Esperado**: Redirección a `/home`, se muestra tu nombre y email.

### Escenario B: Usuario No Registrado
1. Intenta iniciar sesión con un correo `@ucc.edu.ar` que **NO** esté en la tabla `Usuario`.
2. **Resultado Esperado**: Alerta "Usuario no registrado en el sistema".

### Escenario C: Dominio Inválido
1. Intenta iniciar sesión con un Gmail personal (`@gmail.com`).
2. **Resultado Esperado**: Alerta "El dominio del correo no está autorizado".

### Escenario D: Logout
1. Estando en `/home`, haz clic en "Cerrar Sesión".
2. **Resultado Esperado**: Se borra la sesión y redirige a `/login`. Intenta volver atrás en el navegador o ir a `/home` manualmente; el `AuthGuard` debería impedirlo.
