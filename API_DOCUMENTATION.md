# 📦 TesisInventory — Documentación de API REST

> **Base URL:** `http://localhost:5000`
> **Formato de datos:** `application/json`
> **Autenticación:** JWT Bearer Token (excepto endpoints de auth)
> **Versión:** v1.0 | Última actualización: 2026-06-06

---

## 🗂️ Índice

1. [🔐 Autenticación](#-autenticación)
2. [📁 Rubros](#-rubros)
3. [📂 Familias](#-familias)
4. [🏷️ Atributos](#-atributos)
5. [📦 Productos](#-productos)
6. [📊 Stock](#-stock)
7. [📋 Tipos de Datos y Enums](#-tipos-de-datos-y-enums)

---

## 🔐 Autenticación

> Todos los endpoints del sistema (excepto los de autenticación) requieren un **JWT Bearer Token** en el header `Authorization`.

### Esquema de Autenticación

```
Authorization: Bearer <token>
```

El token se obtiene mediante uno de los dos endpoints de login descritos a continuación.

---

### `POST /api/Auth/google-login`

Autentica al usuario usando un token de Google OAuth2. El correo debe pertenecer al dominio `@ucc.edu.ar` y el usuario debe estar registrado y activo en el sistema.

**🔓 Requiere autenticación:** No

**Headers:**

| Header | Valor |
|---|---|
| `Content-Type` | `application/json` |

**Body (Request):**

```json
{
  "token": "eyJhbGciOiJSUzI1NiIsImtpZCI6..."
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `token` | `string` | ✅ Sí | Token de ID de Google OAuth2 |

**✅ Respuesta exitosa — 200 OK:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "idUsuario": 1,
    "nombreUsuario": "Juan Pérez",
    "email": "jperez@ucc.edu.ar",
    "idRol": 1,
    "idSede": 1,
    "nombreRol": "Administrador",
    "nombreSede": "Sede Central",
    "todasLasSedes": true,
    "limitarOperacionSedePrimaria": false,
    "permisos": [
      "Inventario_Ver",
      "Inventario_StockLocal_Ver",
      "Parametricas_Ver"
    ],
    "sedesPermitidas": [1, 2, 3]
  }
}
```

**❌ Respuestas de error:**

| Código | Descripción |
|---|---|
| `401 Unauthorized` | Token inválido / dominio no autorizado / usuario no registrado / usuario inactivo |
| `500 Internal Server Error` | Error interno del servidor |

```json
{ "message": "Token de Google inválido." }
{ "message": "El dominio del correo no está autorizado (@ucc.edu.ar)." }
{ "message": "Usuario no registrado en el sistema." }
{ "message": "Usuario inactivo. Contacte al administrador." }
```

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Auth/google-login \
  -H "Content-Type: application/json" \
  -d '{"token": "TU_GOOGLE_ID_TOKEN"}'
```

---

### `POST /api/Auth/test-login`

Login simplificado para desarrollo/testing. No requiere Google OAuth, solo el email del usuario registrado.

> ⚠️ **Solo para uso en entorno de desarrollo.**

**🔓 Requiere autenticación:** No

**Headers:**

| Header | Valor |
|---|---|
| `Content-Type` | `application/json` |

**Body (Request):**

```json
{
  "email": "admin@ucc.edu.ar"
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `email` | `string` | ✅ Sí | Email del usuario registrado en el sistema |

**✅ Respuesta exitosa — 200 OK:**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "idUsuario": 1,
    "nombreUsuario": "Admin",
    "email": "admin@ucc.edu.ar",
    "idRol": 1,
    "idSede": 1,
    "nombreRol": "Administrador",
    "nombreSede": "Sede Central",
    "todasLasSedes": true,
    "limitarOperacionSedePrimaria": false,
    "permisos": ["Inventario_Ver", "Parametricas_Ver"],
    "sedesPermitidas": []
  }
}
```

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Auth/test-login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@ucc.edu.ar"}'
```

---

### Uso del Token JWT

Una vez obtenido el token, incluirlo en **todas** las solicitudes protegidas:

```bash
# Variable de entorno recomendada:
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Uso en cualquier endpoint:
curl -H "Authorization: Bearer $TOKEN" http://localhost:5000/api/Rubros
```

---

## 📁 Rubros

Los Rubros son la **clasificación de primer nivel** del inventario (ej: Electricidad, Iluminación, Sanitario).

**🔒 Requiere autenticación:** Sí — JWT Bearer Token

**Base path:** `/api/Rubros`

---

### `GET /api/Rubros`

Obtiene todos los rubros del sistema.

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `includeInactive` | `boolean` | No | `false` | Si `true`, incluye rubros inactivos |

**✅ Respuesta — 200 OK:**

```json
[
  {
    "idRubro": 1,
    "codigoRubro": "ELEC",
    "nombre": "Electricidad",
    "activo": true,
    "fechaCreacion": "2025-01-15T10:00:00Z",
    "fechaActualizacion": "2025-01-15T10:00:00Z"
  },
  {
    "idRubro": 2,
    "codigoRubro": "ILUM",
    "nombre": "Iluminación",
    "activo": true,
    "fechaCreacion": "2025-01-15T10:00:00Z",
    "fechaActualizacion": "2025-01-15T10:00:00Z"
  }
]
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Rubros?includeInactive=false" \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Rubros/{id}`

Obtiene un rubro por su ID.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | `integer` | ID del rubro |

**✅ Respuesta — 200 OK:**

```json
{
  "idRubro": 1,
  "codigoRubro": "ELEC",
  "nombre": "Electricidad",
  "activo": true,
  "fechaCreacion": "2025-01-15T10:00:00Z",
  "fechaActualizacion": "2025-01-15T10:00:00Z"
}
```

**❌ Respuestas de error:**

| Código | Descripción |
|---|---|
| `404 Not Found` | No existe un rubro con ese ID |

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Rubros/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `POST /api/Rubros`

Crea un nuevo rubro.

**Body (Request):**

```json
{
  "codigoRubro": "ELEC",
  "nombre": "Electricidad",
  "activo": true
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `codigoRubro` | `string` | ✅ Sí | Código único del rubro (ej: ELEC, ILUM) |
| `nombre` | `string` | ✅ Sí | Nombre descriptivo del rubro |
| `activo` | `boolean` | No | Estado activo/inactivo. Default: `true` |

**✅ Respuesta — 201 Created:**

```json
{
  "idRubro": 10,
  "codigoRubro": "ELEC",
  "nombre": "Electricidad",
  "activo": true,
  "fechaCreacion": "2025-06-06T18:00:00Z",
  "fechaActualizacion": "2025-06-06T18:00:00Z"
}
```

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Rubros \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "codigoRubro": "ELEC",
    "nombre": "Electricidad",
    "activo": true
  }'
```

---

### `PUT /api/Rubros/{id}`

Actualiza un rubro existente.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | `integer` | ID del rubro a actualizar |

**Body (Request):**

```json
{
  "codigoRubro": "ELEC",
  "nombre": "Electricidad General",
  "activo": true
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `codigoRubro` | `string` | ✅ Sí | Nuevo código del rubro |
| `nombre` | `string` | ✅ Sí | Nuevo nombre del rubro |
| `activo` | `boolean` | ✅ Sí | Estado activo/inactivo |

**✅ Respuesta — 200 OK:** Objeto `RubroDto` actualizado.

**Ejemplo cURL:**

```bash
curl -X PUT http://localhost:5000/api/Rubros/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "codigoRubro": "ELEC",
    "nombre": "Electricidad General",
    "activo": true
  }'
```

---

### `DELETE /api/Rubros/{id}`

Elimina (desactiva) un rubro.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | `integer` | ID del rubro a eliminar |

**✅ Respuesta — 204 No Content**

**Ejemplo cURL:**

```bash
curl -X DELETE http://localhost:5000/api/Rubros/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

## 📂 Familias

Las Familias son la **clasificación de segundo nivel**, siempre asociadas a un Rubro (ej: MTF = Mecanismos, tomas y fichas → bajo ELEC).

**🔒 Requiere autenticación:** Sí — JWT Bearer Token

**Base path:** `/api/Familias`

---

### `GET /api/Familias`

Obtiene todas las familias.

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `includeInactive` | `boolean` | No | `false` | Si `true`, incluye familias inactivas |

**✅ Respuesta — 200 OK:**

```json
[
  {
    "idFamilia": 1,
    "idRubro": 1,
    "nombreRubro": "Electricidad",
    "codigoFamilia": "MTF",
    "nombre": "Mecanismos, tomas y fichas",
    "activo": true,
    "fechaCreacion": "2025-01-15T10:00:00Z",
    "fechaActualizacion": "2025-01-15T10:00:00Z"
  }
]
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Familias" \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Familias/rubro/{idRubro}`

Obtiene todas las familias de un rubro específico.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `idRubro` | `integer` | ID del rubro |

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `includeInactive` | `boolean` | No | `false` | Incluir inactivas |

**✅ Respuesta — 200 OK:** Array de `FamiliaDto`.

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Familias/rubro/1?includeInactive=false" \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Familias/{id}`

Obtiene una familia por ID.

**✅ Respuesta — 200 OK:**

```json
{
  "idFamilia": 1,
  "idRubro": 1,
  "nombreRubro": "Electricidad",
  "codigoFamilia": "MTF",
  "nombre": "Mecanismos, tomas y fichas",
  "activo": true,
  "fechaCreacion": "2025-01-15T10:00:00Z",
  "fechaActualizacion": "2025-01-15T10:00:00Z"
}
```

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Familias/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Familias/{id}/asociaciones`

Obtiene las asociaciones de atributos de una familia (qué atributos tiene asignados, cuáles son obligatorios).

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | `integer` | ID de la familia |

**✅ Respuesta — 200 OK:**

```json
{
  "idFamilia": 1,
  "nombreFamilia": "Mecanismos, tomas y fichas",
  "atributos": [
    {
      "idAtributo": 1,
      "codigoAtributo": "ATR-001",
      "nombre": "Código interno",
      "obligatorio": true
    }
  ]
}
```

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Familias/1/asociaciones \
  -H "Authorization: Bearer $TOKEN"
```

---

### `POST /api/Familias`

Crea una nueva familia.

**Body (Request):**

```json
{
  "idRubro": 1,
  "codigoFamilia": "MTF",
  "nombre": "Mecanismos, tomas y fichas",
  "activo": true
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `idRubro` | `integer` | ✅ Sí | ID del rubro al que pertenece |
| `codigoFamilia` | `string` | ✅ Sí | Código único dentro del rubro |
| `nombre` | `string` | ✅ Sí | Nombre descriptivo de la familia |
| `activo` | `boolean` | No | Default: `true` |

**✅ Respuesta — 201 Created:** Objeto `FamiliaDto`.

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Familias \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idRubro": 1,
    "codigoFamilia": "MTF",
    "nombre": "Mecanismos, tomas y fichas",
    "activo": true
  }'
```

---

### `PUT /api/Familias/{id}`

Actualiza una familia existente.

**Body (Request):**

```json
{
  "idRubro": 1,
  "codigoFamilia": "MTF",
  "nombre": "Mecanismos, tomas y fichas (actualizado)",
  "activo": true
}
```

**✅ Respuesta — 200 OK:** Objeto `FamiliaDto` actualizado.

**Ejemplo cURL:**

```bash
curl -X PUT http://localhost:5000/api/Familias/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idRubro": 1,
    "codigoFamilia": "MTF",
    "nombre": "Mecanismos, tomas y fichas (actualizado)",
    "activo": true
  }'
```

---

### `DELETE /api/Familias/{id}`

Elimina (desactiva) una familia.

**✅ Respuesta — 204 No Content**

**Ejemplo cURL:**

```bash
curl -X DELETE http://localhost:5000/api/Familias/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

## 🏷️ Atributos

Los Atributos son **campos de clasificación** asignables a Familias. Cada Producto puede tener valores para los atributos de su familia.

**🔒 Requiere autenticación:** Sí — JWT Bearer Token

**Base path:** `/api/Atributos`

---

### `GET /api/Atributos`

Obtiene todos los atributos del catálogo maestro.

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `includeInactive` | `boolean` | No | `false` | Incluir atributos inactivos |

**✅ Respuesta — 200 OK:**

```json
[
  {
    "idAtributo": 1,
    "codigoAtributo": "ATR-001",
    "nombre": "Código interno",
    "tipoDato": "TEXT",
    "unidad": null,
    "descripcion": "Identificador del producto dentro del stock.",
    "activo": true,
    "fechaCreacion": "2025-01-15T10:00:00Z",
    "fechaActualizacion": "2025-01-15T10:00:00Z"
  },
  {
    "idAtributo": 3,
    "codigoAtributo": "ATR-003",
    "nombre": "Unidad de medida",
    "tipoDato": "LIST",
    "unidad": null,
    "descripcion": "Unidad con la que se controla el stock.",
    "activo": true,
    "fechaCreacion": "2025-01-15T10:00:00Z",
    "fechaActualizacion": "2025-01-15T10:00:00Z"
  }
]
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Atributos" \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Atributos/{id}`

Obtiene un atributo por ID.

**✅ Respuesta — 200 OK:** Objeto `AtributoDto`.

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Atributos/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `POST /api/Atributos`

Crea un nuevo atributo maestro.

**Body (Request):**

```json
{
  "codigoAtributo": "ATR-109",
  "nombre": "Peso",
  "tipoDato": "NUMBER",
  "unidad": "kg",
  "descripcion": "Peso del producto en kilogramos.",
  "activo": true
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `codigoAtributo` | `string` | ✅ Sí | Código único del atributo (ej: ATR-109) |
| `nombre` | `string` | ✅ Sí | Nombre del atributo |
| `tipoDato` | `string` | ✅ Sí | `TEXT` \| `NUMBER` \| `DECIMAL` \| `BOOL` \| `LIST` |
| `unidad` | `string` | No | Unidad de medida (ej: `V`, `A`, `W`, `mm`) |
| `descripcion` | `string` | No | Descripción del atributo |
| `activo` | `boolean` | No | Default: `true` |

**✅ Respuesta — 201 Created:** Objeto `AtributoDto`.

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Atributos \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "codigoAtributo": "ATR-109",
    "nombre": "Peso",
    "tipoDato": "NUMBER",
    "unidad": "kg",
    "descripcion": "Peso del producto en kilogramos.",
    "activo": true
  }'
```

---

### `PUT /api/Atributos/{id}`

Actualiza un atributo existente.

**Body (Request):**

```json
{
  "codigoAtributo": "ATR-001",
  "nombre": "Código interno (actualizado)",
  "tipoDato": "TEXT",
  "unidad": null,
  "descripcion": "Descripción actualizada.",
  "activo": true
}
```

**✅ Respuesta — 200 OK:** Objeto `AtributoDto` actualizado.

**Ejemplo cURL:**

```bash
curl -X PUT http://localhost:5000/api/Atributos/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "codigoAtributo": "ATR-001",
    "nombre": "Código interno",
    "tipoDato": "TEXT",
    "unidad": null,
    "descripcion": "Identificador del producto.",
    "activo": true
  }'
```

---

### `DELETE /api/Atributos/{id}`

Elimina un atributo maestro.

**✅ Respuesta — 204 No Content**

**Ejemplo cURL:**

```bash
curl -X DELETE http://localhost:5000/api/Atributos/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Atributos/{idAtributo}/opciones`

Obtiene las opciones de lista de un atributo tipo `LIST`.

**✅ Respuesta — 200 OK:**

```json
[
  {
    "idAtributoOpcion": 1,
    "idAtributo": 3,
    "codigoOpcion": "Ud",
    "valor": "Ud",
    "activo": true
  },
  {
    "idAtributoOpcion": 2,
    "idAtributo": 3,
    "codigoOpcion": "metro",
    "valor": "metro",
    "activo": true
  }
]
```

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Atributos/3/opciones \
  -H "Authorization: Bearer $TOKEN"
```

---

### `POST /api/Atributos/{idAtributo}/opciones`

Agrega una nueva opción a un atributo tipo `LIST`.

**Body (Request):**

```json
{
  "codigoOpcion": "envase",
  "valor": "envase",
  "activo": true
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `codigoOpcion` | `string` | ✅ Sí | Código de la opción |
| `valor` | `string` | ✅ Sí | Valor mostrado al usuario |
| `activo` | `boolean` | No | Default: `true` |

**✅ Respuesta — 200 OK:** Objeto `AtributoOpcionDto`.

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Atributos/3/opciones \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "codigoOpcion": "envase",
    "valor": "envase",
    "activo": true
  }'
```

---

### `DELETE /api/Atributos/opciones/{idOpcion}`

Elimina una opción de lista.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `idOpcion` | `integer` | ID de la opción a eliminar |

**✅ Respuesta — 204 No Content**

**Ejemplo cURL:**

```bash
curl -X DELETE http://localhost:5000/api/Atributos/opciones/5 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Atributos/familia/{idFamilia}`

Obtiene todos los atributos asignados a una familia, con su configuración de obligatoriedad.

**✅ Respuesta — 200 OK:**

```json
[
  {
    "idFamiliaAtributo": 1,
    "idFamilia": 1,
    "idAtributo": 1,
    "codigoAtributo": "ATR-001",
    "nombreAtributo": "Código interno",
    "tipoDato": "TEXT",
    "obligatorio": true,
    "activo": true
  }
]
```

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Atributos/familia/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `POST /api/Atributos/familia/{idFamilia}`

Asigna un atributo existente a una familia.

**Body (Request):**

```json
{
  "idAtributo": 9,
  "obligatorio": true,
  "activo": true
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `idAtributo` | `integer` | ✅ Sí | ID del atributo a asignar |
| `obligatorio` | `boolean` | No | Si el atributo es obligatorio para esa familia. Default: `false` |
| `activo` | `boolean` | No | Default: `true` |

**✅ Respuesta — 200 OK:** Objeto `FamiliaAtributoDto`.

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Atributos/familia/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idAtributo": 9,
    "obligatorio": true,
    "activo": true
  }'
```

---

### `DELETE /api/Atributos/familia/{idFamilia}/atributo/{idAtributo}`

Desvincula un atributo de una familia.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `idFamilia` | `integer` | ID de la familia |
| `idAtributo` | `integer` | ID del atributo |

**✅ Respuesta — 204 No Content**

**Ejemplo cURL:**

```bash
curl -X DELETE http://localhost:5000/api/Atributos/familia/1/atributo/9 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Atributos/{idAtributo}/familias`

Obtiene en qué familias está asignado un atributo.

**✅ Respuesta — 200 OK:** Array de `FamiliaAtributoDto`.

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Atributos/1/familias \
  -H "Authorization: Bearer $TOKEN"
```

---

## 📦 Productos

Los Productos son los artículos del inventario, clasificados por Rubro y Familia, con atributos de valor específicos para cada uno.

**🔒 Requiere autenticación:** Sí — JWT Bearer Token

**Base path:** `/api/Productos`

---

### `GET /api/Productos`

Obtiene todos los productos.

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `includeInactive` | `boolean` | No | `false` | Incluir productos inactivos |

**✅ Respuesta — 200 OK:**

```json
[
  {
    "idProducto": 1,
    "idFamilia": 1,
    "idRubro": 1,
    "nombreFamilia": "Mecanismos, tomas y fichas",
    "nombreRubro": "Electricidad",
    "sku": "PROD-0001",
    "nombre": "BASTIDOR SCHNEIDER RODA",
    "unidadMedida": "Ud",
    "activo": true,
    "fechaCreacion": "2025-01-15T10:00:00Z",
    "fechaActualizacion": "2025-01-15T10:00:00Z",
    "atributos": [
      {
        "idAtributo": 1,
        "codigoAtributo": "ATR-001",
        "nombreAtributo": "Código interno",
        "tipoDatoAtributo": "TEXT",
        "valorTexto": "PROD-0001",
        "valorNumero": null,
        "valorDecimal": null,
        "valorBool": null,
        "valorLista": null
      },
      {
        "idAtributo": 4,
        "codigoAtributo": "ATR-004",
        "nombreAtributo": "Marca",
        "tipoDatoAtributo": "TEXT",
        "valorTexto": "Schneider",
        "valorNumero": null,
        "valorDecimal": null,
        "valorBool": null,
        "valorLista": null
      }
    ]
  }
]
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Productos?includeInactive=false" \
  -H "Authorization: Bearer $TOKEN"
```

---

### `GET /api/Productos/{id}`

Obtiene un producto por ID, incluyendo todos sus atributos.

**✅ Respuesta — 200 OK:** Objeto `ProductoDto` completo con array `atributos`.

**❌ Respuestas de error:**

| Código | Descripción |
|---|---|
| `404 Not Found` | No existe el producto |

**Ejemplo cURL:**

```bash
curl -X GET http://localhost:5000/api/Productos/1 \
  -H "Authorization: Bearer $TOKEN"
```

---

### `POST /api/Productos`

Crea un nuevo producto con sus valores de atributos.

**Body (Request):**

```json
{
  "idRubro": 1,
  "idFamilia": 1,
  "nombre": "BASTIDOR SCHNEIDER RODA",
  "unidadMedida": "Ud",
  "activo": true,
  "puntoReposicion": 5,
  "atributos": [
    {
      "idAtributo": 1,
      "valorTexto": "PROD-CUSTOM-001",
      "valorNumero": null,
      "valorDecimal": null,
      "valorBool": null,
      "valorLista": null
    },
    {
      "idAtributo": 2,
      "valorTexto": "Bastidor Schneider Roda",
      "valorNumero": null,
      "valorDecimal": null,
      "valorBool": null,
      "valorLista": null
    },
    {
      "idAtributo": 3,
      "valorTexto": null,
      "valorNumero": null,
      "valorDecimal": null,
      "valorBool": null,
      "valorLista": "Ud"
    },
    {
      "idAtributo": 4,
      "valorTexto": "Schneider",
      "valorNumero": null,
      "valorDecimal": null,
      "valorBool": null,
      "valorLista": null
    }
  ]
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `idRubro` | `integer` | ✅ Sí | ID del rubro |
| `idFamilia` | `integer` | ✅ Sí | ID de la familia |
| `nombre` | `string` | ✅ Sí | Nombre del producto |
| `unidadMedida` | `string` | ✅ Sí | Unidad de medida (ej: `Ud`, `metro`, `kg`) |
| `activo` | `boolean` | No | Default: `true` |
| `puntoReposicion` | `integer` | No | Umbral de alerta de stock mínimo. Default: `0` |
| `atributos` | `array` | No | Lista de valores de atributos |
| `atributos[].idAtributo` | `integer` | ✅ (en cada ítem) | ID del atributo |
| `atributos[].valorTexto` | `string\|null` | Condicional | Para atributos tipo `TEXT` |
| `atributos[].valorNumero` | `integer\|null` | Condicional | Para atributos tipo `NUMBER` |
| `atributos[].valorDecimal` | `decimal\|null` | Condicional | Para atributos tipo `DECIMAL` |
| `atributos[].valorBool` | `boolean\|null` | Condicional | Para atributos tipo `BOOL` |
| `atributos[].valorLista` | `string\|null` | Condicional | Para atributos tipo `LIST` (debe coincidir con una opción válida) |

> **Regla de valores:** Completar **solo** el campo que corresponde al `tipoDato` del atributo. Los demás deben ser `null`.

**✅ Respuesta — 201 Created:** Objeto `ProductoDto` completo.

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Productos \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "idRubro": 1,
    "idFamilia": 1,
    "nombre": "BASTIDOR SCHNEIDER RODA",
    "unidadMedida": "Ud",
    "activo": true,
    "puntoReposicion": 5,
    "atributos": [
      {"idAtributo": 1, "valorTexto": "PROD-CUSTOM-001"},
      {"idAtributo": 2, "valorTexto": "Bastidor Schneider Roda"},
      {"idAtributo": 3, "valorLista": "Ud"},
      {"idAtributo": 4, "valorTexto": "Schneider"}
    ]
  }'
```

---

### `PUT /api/Productos/{id}`

Actualiza un producto existente.

**Body (Request):**

```json
{
  "nombre": "BASTIDOR SCHNEIDER RODA (actualizado)",
  "unidadMedida": "Ud",
  "activo": true
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `nombre` | `string` | ✅ Sí | Nuevo nombre |
| `unidadMedida` | `string` | ✅ Sí | Nueva unidad |
| `activo` | `boolean` | No | Estado |

**✅ Respuesta — 200 OK:** Objeto `ProductoDto` actualizado.

**Ejemplo cURL:**

```bash
curl -X PUT http://localhost:5000/api/Productos/1 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "BASTIDOR SCHNEIDER RODA (actualizado)",
    "unidadMedida": "Ud",
    "activo": true
  }'
```

---

### `DELETE /api/Productos/{id}`

Elimina un producto. Requiere confirmación con el nombre exacto del producto.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `id` | `integer` | ID del producto |

**Query Parameters:**

| Parámetro | Tipo | Requerido | Descripción |
|---|---|---|---|
| `confirmacionNombre` | `string` | ✅ Sí | Nombre exacto del producto para confirmar la eliminación |

**✅ Respuesta — 204 No Content**

**Ejemplo cURL:**

```bash
curl -X DELETE "http://localhost:5000/api/Productos/1?confirmacionNombre=BASTIDOR+SCHNEIDER+RODA" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 📊 Stock

El módulo de Stock gestiona cantidades, movimientos, historial y operaciones múltiples por sede.

**🔒 Requiere autenticación:** Sí — JWT Bearer Token

**Headers especiales requeridos:**

| Header | Tipo | Descripción |
|---|---|---|
| `Authorization` | `string` | `Bearer <token>` |
| `Sede-Contexto` | `integer` | ID de la sede sobre la cual se opera. Los Administradores pueden cambiar este valor; los Operadores solo pueden usar su sede asignada. |

**Base path:** `/api/Stock`

---

### `GET /api/Stock/sede`

Obtiene el stock de la sede en contexto con soporte de filtros y paginación.

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `search` | `string` | No | — | Búsqueda por nombre o SKU |
| `idRubro` | `integer` | No | — | Filtrar por rubro |
| `idFamilia` | `integer` | No | — | Filtrar por familia |
| `estado` | `boolean` | No | — | `true` = solo activos, `false` = solo inactivos |
| `bajoStock` | `boolean` | No | — | `true` = solo productos con stock ≤ punto de reposición |
| `idSedeQuery` | `integer` | No | — | Sobreescribe la sede del header (solo Admin) |
| `page` | `integer` | No | `1` | Número de página |
| `pageSize` | `integer` | No | `50` | Registros por página |

**✅ Respuesta — 200 OK:**

```json
{
  "data": [
    {
      "idStock": 1,
      "idProducto": 1,
      "sku": "PROD-0001",
      "nombreProducto": "BASTIDOR SCHNEIDER RODA",
      "unidadMedida": "Ud",
      "rubroProducto": "Electricidad",
      "familiaProducto": "Mecanismos, tomas y fichas",
      "estadoProducto": true,
      "idSede": 1,
      "cantidadActual": 15,
      "puntoReposicion": 5,
      "fechaActualizacion": "2025-06-01T08:00:00Z",
      "conBajoStock": false,
      "atributos": [
        {
          "idAtributo": 4,
          "codigoAtributo": "ATR-004",
          "nombreAtributo": "Marca",
          "tipoDatoAtributo": "TEXT",
          "valorTexto": "Schneider",
          "valorNumero": null,
          "valorDecimal": null,
          "valorBool": null,
          "valorLista": null
        }
      ]
    }
  ],
  "totalCount": 290,
  "page": 1,
  "pageSize": 50,
  "totalPages": 6
}
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Stock/sede?search=schneider&idRubro=1&page=1&pageSize=20" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Sede-Contexto: 1"
```

---

### `POST /api/Stock/incremento`

Registra un ingreso de stock (entrada de mercadería por compra, ajuste, etc.).

**Body (Request):**

```json
{
  "idProducto": 1,
  "cantidad": 10,
  "motivo": "PorCompra",
  "observaciones": "Orden de compra OC-2025-001"
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `idProducto` | `integer` | ✅ Sí | ID del producto |
| `cantidad` | `integer` | ✅ Sí | Cantidad a incrementar (≥ 1) |
| `motivo` | `MotivoMovimiento` | ✅ Sí | Ver enum. Valores: `PorCompra`, `AjustesVarios` |
| `observaciones` | `string` | No | Texto libre de observación |

> **Motivos válidos para incremento:** `PorCompra`, `AjustesVarios`

**✅ Respuesta — 200 OK:**

```json
{
  "message": "Stock incrementado con éxito",
  "data": {
    "idStock": 1,
    "idProducto": 1,
    "idSede": 1,
    "cantidadActual": 25,
    "puntoReposicion": 5,
    "fechaActualizacion": "2025-06-06T18:00:00Z"
  }
}
```

**❌ Respuestas de error:**

| Código | Descripción |
|---|---|
| `400 Bad Request` | Validación fallida o producto no encontrado |

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Stock/incremento \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -H "Sede-Contexto: 1" \
  -d '{
    "idProducto": 1,
    "cantidad": 10,
    "motivo": "PorCompra",
    "observaciones": "Orden de compra OC-2025-001"
  }'
```

---

### `POST /api/Stock/consumo`

Registra un egreso de stock (consumo, descarte por daño, vencimiento, etc.).

**Body (Request):**

```json
{
  "idProducto": 1,
  "cantidad": 3,
  "motivo": "Consumo"
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `idProducto` | `integer` | ✅ Sí | ID del producto |
| `cantidad` | `integer` | ✅ Sí | Cantidad a descontar (≥ 1) |
| `motivo` | `MotivoMovimiento` | ✅ Sí | Ver enum. Valores: `Consumo`, `EgresoPorVencimiento`, `EgresoPorDano`, `AjustesVarios` |

> **Motivos válidos para consumo:** `Consumo`, `EgresoPorVencimiento`, `EgresoPorDano`, `AjustesVarios`

**✅ Respuesta — 200 OK:**

```json
{
  "message": "Consumo registrado con éxito",
  "data": {
    "idStock": 1,
    "idProducto": 1,
    "idSede": 1,
    "cantidadActual": 12,
    "puntoReposicion": 5,
    "fechaActualizacion": "2025-06-06T18:05:00Z"
  }
}
```

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Stock/consumo \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -H "Sede-Contexto: 1" \
  -d '{
    "idProducto": 1,
    "cantidad": 3,
    "motivo": "Consumo"
  }'
```

---

### `POST /api/Stock/operacion-multiple`

Procesa una operación de stock con múltiples productos en un solo request (ideal para remitos).

**Body (Request):**

```json
{
  "tipoOperacion": "Egreso",
  "motivo": "Consumo",
  "ordenTrabajo": "OT-2025-042",
  "ordenCompra": null,
  "ticketSolicitud": "SOL-008",
  "observaciones": "Mantenimiento eléctrico Aula 3",
  "idSolicitudCompraAsociada": null,
  "detalles": [
    {
      "idProducto": 1,
      "cantidad": 2
    },
    {
      "idProducto": 7,
      "cantidad": 5
    },
    {
      "idProducto": 15,
      "cantidad": 1
    }
  ]
}
```

| Campo | Tipo | Requerido | Descripción |
|---|---|---|---|
| `tipoOperacion` | `TipoMovimiento` | ✅ Sí | `Ingreso` o `Egreso` |
| `motivo` | `MotivoMovimiento` | ✅ Sí | Ver enum |
| `ordenTrabajo` | `string` | No | Referencia a orden de trabajo |
| `ordenCompra` | `string` | No | Referencia a orden de compra |
| `ticketSolicitud` | `string` | No | Referencia a ticket de solicitud |
| `observaciones` | `string` | No | Texto libre |
| `idSolicitudCompraAsociada` | `integer\|null` | No | ID de solicitud de compra vinculada |
| `detalles` | `array` | ✅ Sí | Mínimo 1 producto |
| `detalles[].idProducto` | `integer` | ✅ Sí | ID del producto |
| `detalles[].cantidad` | `integer` | ✅ Sí | Cantidad (≥ 1) |

**✅ Respuesta — 200 OK:**

```json
{
  "message": "Operación múltiple procesada con éxito",
  "data": {
    "idOperacion": 42,
    "idSede": 1,
    "idUsuario": 1,
    "tipoOperacion": "Egreso",
    "fecha": "2025-06-06T18:10:00Z",
    "motivo": "Consumo",
    "ordenTrabajo": "OT-2025-042",
    "movimientos": [
      { "idMovimiento": 101, "idProducto": 1, "cantidad": 2, "tipoMovimiento": "Egreso" },
      { "idMovimiento": 102, "idProducto": 7, "cantidad": 5, "tipoMovimiento": "Egreso" }
    ]
  }
}
```

**Ejemplo cURL:**

```bash
curl -X POST http://localhost:5000/api/Stock/operacion-multiple \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -H "Sede-Contexto: 1" \
  -d '{
    "tipoOperacion": "Egreso",
    "motivo": "Consumo",
    "ordenTrabajo": "OT-2025-042",
    "observaciones": "Mantenimiento Aula 3",
    "detalles": [
      {"idProducto": 1, "cantidad": 2},
      {"idProducto": 7, "cantidad": 5}
    ]
  }'
```

---

### `GET /api/Stock/operaciones-multiples`

Obtiene el historial de operaciones múltiples procesadas en la sede.

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `search` | `string` | No | — | Búsqueda por referencia |
| `tipoOperacion` | `string` | No | — | `Ingreso` o `Egreso` |
| `idUsuario` | `integer` | No | — | Filtrar por usuario |
| `fechaDesde` | `string` | No | — | Fecha inicio (ISO 8601: `2025-01-01`) |
| `fechaHasta` | `string` | No | — | Fecha fin (ISO 8601: `2025-12-31`) |
| `skip` | `integer` | No | `0` | Registros a saltar |
| `take` | `integer` | No | `50` | Registros a tomar |

**✅ Respuesta — 200 OK:**

```json
{
  "data": [
    {
      "idOperacion": 42,
      "tipoOperacion": "Egreso",
      "fecha": "2025-06-06T18:10:00Z",
      "motivo": "Consumo",
      "ordenTrabajo": "OT-2025-042",
      "observaciones": "Mantenimiento Aula 3",
      "idSede": 1,
      "idUsuario": 1,
      "cantidadItems": 2
    }
  ],
  "totalCount": 15,
  "skip": 0,
  "take": 50
}
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Stock/operaciones-multiples?tipoOperacion=Egreso&fechaDesde=2025-01-01&fechaHasta=2025-12-31" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Sede-Contexto: 1"
```

---

### `GET /api/Stock/{idProducto}/movimientos`

Obtiene el historial de movimientos de un producto específico en la sede.

**Path Parameters:**

| Parámetro | Tipo | Descripción |
|---|---|---|
| `idProducto` | `integer` | ID del producto |

**Query Parameters:**

| Parámetro | Tipo | Requerido | Descripción |
|---|---|---|---|
| `tipoMovimiento` | `string` | No | `Ingreso` o `Egreso` |
| `fechaDesde` | `string` | No | Fecha inicio ISO 8601 |
| `fechaHasta` | `string` | No | Fecha fin ISO 8601 |

**✅ Respuesta — 200 OK:**

```json
[
  {
    "idMovimiento": 101,
    "idProducto": 1,
    "nombreProducto": "BASTIDOR SCHNEIDER RODA",
    "sku": "PROD-0001",
    "idSede": 1,
    "idUsuario": 1,
    "nombreUsuario": "Admin",
    "tipoMovimiento": "Egreso",
    "cantidad": 2,
    "motivo": "Consumo",
    "observaciones": "Mantenimiento Aula 3",
    "fecha": "2025-06-06T18:10:00Z"
  }
]
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Stock/1/movimientos?tipoMovimiento=Egreso&fechaDesde=2025-01-01" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Sede-Contexto: 1"
```

---

### `GET /api/Stock/movimientos/historial`

Obtiene el historial global de movimientos de todos los productos en la sede, con filtros avanzados y paginación.

**Query Parameters:**

| Parámetro | Tipo | Requerido | Default | Descripción |
|---|---|---|---|---|
| `search` | `string` | No | — | Búsqueda por nombre o SKU |
| `idRubro` | `integer` | No | — | Filtrar por rubro |
| `idFamilia` | `integer` | No | — | Filtrar por familia |
| `tipoMovimiento` | `string` | No | — | `Ingreso` o `Egreso` |
| `idUsuario` | `integer` | No | — | Filtrar por usuario |
| `fechaDesde` | `string` | No | — | Fecha inicio ISO 8601 |
| `fechaHasta` | `string` | No | — | Fecha fin ISO 8601 |
| `page` | `integer` | No | `1` | Página |
| `pageSize` | `integer` | No | `50` | Registros por página |

**✅ Respuesta — 200 OK:**

```json
{
  "data": [ /* array de MovimientoDto */ ],
  "totalCount": 458,
  "page": 1,
  "pageSize": 50,
  "totalPages": 10
}
```

**Ejemplo cURL:**

```bash
curl -X GET "http://localhost:5000/api/Stock/movimientos/historial?idRubro=1&tipoMovimiento=Egreso&page=1&pageSize=25" \
  -H "Authorization: Bearer $TOKEN" \
  -H "Sede-Contexto: 1"
```

---

## 📋 Tipos de Datos y Enums

### TipoDato (Atributos)

| Valor | Descripción | Campo de valor en PAV |
|---|---|---|
| `TEXT` | Texto libre | `valorTexto` |
| `NUMBER` | Número entero | `valorNumero` |
| `DECIMAL` | Número decimal | `valorDecimal` |
| `BOOL` | Booleano (Sí/No) | `valorBool` |
| `LIST` | Opción de lista predefinida | `valorLista` |

---

### MotivoMovimiento (Enum)

| Valor numérico | Nombre | Uso |
|---|---|---|
| `0` | `Consumo` | Egreso por uso/consumo normal |
| `1` | `EgresoPorVencimiento` | Egreso por producto vencido |
| `2` | `EgresoPorDano` | Egreso por producto dañado |
| `3` | `PorCompra` | Ingreso por compra |
| `4` | `AjustesVarios` | Ajuste manual de inventario |
| `5` | `Transferencia` | Movimiento entre sedes |

> **En el body del request:** enviar el **nombre** del enum (string), no el número. Ej: `"motivo": "Consumo"`.

---

### TipoMovimiento (Enum)

| Valor numérico | Nombre | Descripción |
|---|---|---|
| `0` | `Ingreso` | Entrada de stock |
| `1` | `Egreso` | Salida de stock |

> **En el body del request:** enviar el **nombre** (string). Ej: `"tipoOperacion": "Egreso"`.

---

## 🛡️ Códigos de Estado HTTP

| Código | Significado |
|---|---|
| `200 OK` | Solicitud exitosa con respuesta |
| `201 Created` | Recurso creado exitosamente |
| `204 No Content` | Operación exitosa sin cuerpo de respuesta (DELETE) |
| `400 Bad Request` | Error de validación o datos incorrectos |
| `401 Unauthorized` | Token inválido, expirado o ausente |
| `404 Not Found` | Recurso no encontrado |
| `500 Internal Server Error` | Error interno del servidor |

---

## 🔧 Variables de Entorno Sugeridas para Testing

```bash
# Base URL
BASE_URL="http://localhost:5000"

# Obtener token
TOKEN=$(curl -s -X POST $BASE_URL/api/Auth/test-login \
  -H "Content-Type: application/json" \
  -d '{"email": "admin@ucc.edu.ar"}' | grep -o '"token":"[^"]*"' | cut -d'"' -f4)

echo "Token obtenido: $TOKEN"

# Ejemplo de uso
curl -X GET "$BASE_URL/api/Rubros" \
  -H "Authorization: Bearer $TOKEN"
```

---

## 📝 Notas de Implementación

> **Sede-Contexto:** El header `Sede-Contexto` es fundamental para todos los endpoints de Stock. El sistema verifica que el usuario tenga acceso a la sede solicitada. Un usuario con rol `Administrador` puede consultar cualquier sede; un `Operador` solo puede consultar su sede asignada.

> **SKU:** Los productos tienen un SKU generado automáticamente por el sistema al crearse. No es necesario enviarlo en el request de creación.

> **Cascada de eliminación:** Eliminar un Rubro no elimina Familias ni Productos directamente (OnDelete: Restrict). Se recomienda desactivar (`activo: false`) en lugar de eliminar en entornos productivos.

> **Atributos y valores:** Solo se puede tener **un valor por atributo por producto** (constraint único en `ProductoAtributoValor`). Al actualizar, se sobreescribe el valor existente.
