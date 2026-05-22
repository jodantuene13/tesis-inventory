# README - Épica: Módulo de Transferencias

## Objetivo
Implementar la funcionalidad completa para solicitar, gestionar y hacer seguimiento a las transferencias de stock entre distintas sedes del sistema "Tesis Inventory", garantizando la integridad de los datos y el respeto a la restricción física de las existencias.

## Historias de Usuario (HU)
- **HU-TRANS-01**: Como usuario de una sede, quiero poder solicitar stock a otra sede para reabastecer mi inventario (Transferencia Entrante).
- **HU-TRANS-02**: Como usuario de una sede, quiero ver las peticiones que otras sedes me han hecho y decidir si las Acepto o Rechazo (Transferencia Saliente / Pendiente).
- **HU-TRANS-03**: Como administrador de inventario, quiero enviar un producto a otra sede sin que me lo hayan solicitado (Transferencia Definitiva).
- **HU-TRANS-04**: Como usuario, quiero poder realizar transferencias en calidad de "Préstamo", para que el sistema mantenga constancia de devolución pendiente.
- **HU-TRANS-05**: Como usuario destino, quiero confirmar la recepción de la mercadería para que el stock se debite formalmente de la sede origen y se acredite en mi sede.

## Alcance Funcional
- Interfaz gráfica en Angular con diseño moderno (Listado de Salientes y Entrantes, Tarjetas de estado).
- Creación de Nueva Solicitud mediante Modal flotante interactivo y confirmadores Swal2.
- Selección de productos con buscador autocomplete (SKU + Nombre) pre-filtrado por stock en la sede activa.
- Lógica de negocio de Estados: Solicitada -> En Tránsito -> Recibida / Rechazada / Pendiente de Devolución -> Devuelta.
- Manejo dinámico del Contexto de Sede leyendo el JWT del usuario autenticado.

## Capas Involucradas (Onion Architecture)
- **Domain**: Entidades `Transferencia`, `HistorialTransferencia`, Enum `EstadoTransferencia`, `MotivoTransferencia`.
- **Application**: Servicios de lógica de negocio (`TransferenciaService`) y DTOs (`TransferenciaDto`, `CreateTransferenciaDto`).
- **Infrastructure**: `TransferenciaRepository`, configuración manual de DbContext, y Entity Framework Migrations.
- **API**: `TransferenciasController` para exponer los Endpoints.
- **Frontend**: Componente Standalone `TransferenciasListComponent`, modal integrado, y `TransferenciaService.ts` conectando al backend.

## Decisiones de Implementación y Ajustes

### [2026-03-27]
**Descripción del cambio:** Generación de Estructura Base y Servicios de Transferencias.
**Motivo técnico:** Construir los cimientos de Transferencias aislando la responsabilidad del DbContext a un Repository.  
**Impacto funcional:** Creación de las tablas `Transferencia` e `HistorialTransferencia`, inyectadas bajo la regla de Clean Architecture.

### [2026-03-28]
**Descripción del cambio:** Refactor a `SedeOrigen` y `SedeDestino`.
**Motivo técnico:** El modelo inicial poseía un solo campo `idSede`, lo cual impedía trazar el viaje de la mercadería. Se dividió en Origen y Destino.
**Impacto funcional:** Permite renderizar en las Tarjetas del Front-end de dónde salió y hacia dónde va exactamente la mercadería.

### [2026-03-28]
**Descripción del cambio:** Obtención dinámica del Sede de la sesión mediante JWT.
**Motivo técnico:** Evitar vulnerabilidades enviando el ID de Sede desde el frontend. 
**Impacto funcional:** El backend (`AuthController` y `TransferenciaController`) ahora lee el Claim `sede_id` del token para autorizar las solicitudes y fijar obligatoriamente la SedeOrigen/Destino según corresponda.

### [2026-03-28]
**Descripción del cambio:** Corrección del ruteo 500 al finalizar la Creación.
**Motivo técnico:** ASP.NET Core MVC explotaba con `InvalidOperationException` al intentar emitir la respuesta `CreatedAtAction` porque faltaba el Endpoint `GetById`. Se cambió la devolución a `Ok()`.
**Impacto funcional:** El Frontend ahora recibe una confirmación exitosa y cierra el modal Swal2 limpiamente en verde tras finalizar la transacción.

### [2026-03-28]
**Descripción del cambio:** Hard-Mapping de CamelCase en Tablas de MySQL.
**Motivo técnico:** Entity Framework Core genera sentencias en PascalCase (`IdSedeDestino`), lo cual chocaba con el entorno local de desarrollo MySQL del usuario que registraba en Base de Datos de manera estricta `idSedeDestino` (minúsculas). Se agregaron reglas `HasColumnName("id..")` directas en el `InventoryDbContext`. Se corrió un script limpio borrando las tablas mal mapeadas y se actualizó vía `dotnet ef database update`.
**Impacto funcional:** Solucionó los choques al registrar la Transferencia en MySQL, blindando la Base de Datos contra problemas de Case Sensitivity entre SO Linux/Windows.

### [2026-03-28]
**Descripción del cambio:** Cambio de Arquitectura a "Pull-Based Transfers" e integración con Catálogo Global de Productos.
**Motivo técnico:** El modelo original (`Push-Based`) asumía que el creador de la solicitud era quien enviaba la mercadería. Se detectó que el requerimiento de negocio exigía que el creador de la solicitud funcione como el *receptor* que le **pide** mercadería a otra sede. Se refactorizaron los Repositorios y Controladores para invertir la asignación de `IdSedeOrigen` e `IdSedeDestino`.
**Impacto funcional:** 
- Las bandejas de "Salientes" y "Entrantes" en Angular ahora reflejan correctamente el sentido del *Mensaje/Solicitud* en lugar del flujo físico primario (Saliente = Yo pido; Entrante = Me piden). 
- El Selector de productos ahora permite elegir y buscar sobre la lista maestra del `/api/productos`, combinando las cantidades físicas desde `/api/stock` correspondientes únicamente a la sede original seleccionada, resolviendo la restricción anterior de no poder pedir productos sin historial previo de stock.

### [2026-03-28]
**Descripción del cambio:** Refactor UX/UI (Tarjetas Horizontales, Nomenclatura, Modal de Detalles e Inteligencia de Stock).
**Motivo técnico:** Clarificar la experiencia de usuario y añadir capacidades de validación en tiempo de aprobación mediante consulta histórica y en vivo de Stock Sede. 
**Impacto funcional:** 
- Nomenclaturas pasaron a "Pedidos a Mi Sede (Recibidos)" y "Mis Pedidos Emitidos".
- Tarjetas `TransferenciaCardComponent` migraron de un diseño vertical a horizontal (panorámico).
- Integración del "Ojito" (Ver Detalles) con un sub-modal profundo que captura la columna `StockOrigenSnapshot` recién agregada en Entity Framework y dispara un HTTP Get pasivo al backend para arrojar una advertencia luminosa en color Ámbar si la Sede Origen no está capacitada físicamente para aprobar el pedido en la actualidad.

### [2026-04-16]
**Descripción del cambio:** Refactor de Base de Datos y Formulario hacia modelo Multiproducto (1 a N).
**Motivo técnico:** El diseño lógico anterior ("Una transferencia = un producto") era limitante y requería generar N solicitudes separadas manuales para un pedido completo. Se separó al responsable de la meta data `Transferencia.cs` y su mercadería interna `TransferenciaDetalle.cs`.
**Impacto funcional:**
- Se purgó el histórico preexistente de base de datos debido al hard-reset estructural del esquema.
- El Navbar Lateral agrupa ahora "Gestionar" (Órdenes activas) e "Histórico" en un dropdown de submenús.
- Al generar una nueva transferencia, el Interfaz provee un FormArray dinámico para ir encolando N productos con diferentes cantidades, todo empaquetado bajo un único Número de ID Transferencia visible globalmente.

### [2026-04-16]
**Descripción del cambio:** Generación dinámica de Código de Rastreo (TR) y rediseño a Doble Modal.
**Motivo técnico:** Proveer una experiencia de usuario que mejore el autocompletado sin deformar el UI, y dotar de un código TR legible. Se hizo mediante lógica calculada en el DTO sin necesidad de tocar la base de datos de Entity Framework.
**Impacto funcional:**
- Las transferencias se rigen visualmente por un código como `TR-160426-5` (donde 5 es el ID incremental, precedido por la fecha DDMMAA).
- El autocompletador de inventario ahora vive en un Modal Secundario que se interpone temporalmente, retornando automáticamente los productos clickeados hacia el carrito del Modal Principal.
# README - Épica: Módulo de Transferencias

## Objetivo
Implementar la funcionalidad completa para solicitar, gestionar y hacer seguimiento a las transferencias de stock entre distintas sedes del sistema "Tesis Inventory", garantizando la integridad de los datos y el respeto a la restricción física de las existencias.

## Historias de Usuario (HU)
- **HU-TRANS-01**: Como usuario de una sede, quiero poder solicitar stock a otra sede para reabastecer mi inventario (Transferencia Entrante).
- **HU-TRANS-02**: Como usuario de una sede, quiero ver las peticiones que otras sedes me han hecho y decidir si las Acepto o Rechazo (Transferencia Saliente / Pendiente).
- **HU-TRANS-03**: Como administrador de inventario, quiero enviar un producto a otra sede sin que me lo hayan solicitado (Transferencia Definitiva).
- **HU-TRANS-04**: Como usuario, quiero poder realizar transferencias en calidad de "Préstamo", para que el sistema mantenga constancia de devolución pendiente.
- **HU-TRANS-05**: Como usuario destino, quiero confirmar la recepción de la mercadería para que el stock se debite formalmente de la sede origen y se acredite en mi sede.

## Alcance Funcional
- Interfaz gráfica en Angular con diseño moderno (Listado de Salientes y Entrantes, Tarjetas de estado).
- Creación de Nueva Solicitud mediante Modal flotante interactivo y confirmadores Swal2.
- Selección de productos con buscador autocomplete (SKU + Nombre) pre-filtrado por stock en la sede activa.
- Lógica de negocio de Estados: Solicitada -> En Tránsito -> Recibida / Rechazada / Pendiente de Devolución -> Devuelta.
- Manejo dinámico del Contexto de Sede leyendo el JWT del usuario autenticado.

## Capas Involucradas (Onion Architecture)
- **Domain**: Entidades `Transferencia`, `HistorialTransferencia`, Enum `EstadoTransferencia`, `MotivoTransferencia`.
- **Application**: Servicios de lógica de negocio (`TransferenciaService`) y DTOs (`TransferenciaDto`, `CreateTransferenciaDto`).
- **Infrastructure**: `TransferenciaRepository`, configuración manual de DbContext, y Entity Framework Migrations.
- **API**: `TransferenciasController` para exponer los Endpoints.
- **Frontend**: Componente Standalone `TransferenciasListComponent`, modal integrado, y `TransferenciaService.ts` conectando al backend.

## Decisiones de Implementación y Ajustes

### [2026-03-27]
**Descripción del cambio:** Generación de Estructura Base y Servicios de Transferencias.
**Motivo técnico:** Construir los cimientos de Transferencias aislando la responsabilidad del DbContext a un Repository.  
**Impacto funcional:** Creación de las tablas `Transferencia` e `HistorialTransferencia`, inyectadas bajo la regla de Clean Architecture.

### [2026-03-28]
**Descripción del cambio:** Refactor a `SedeOrigen` y `SedeDestino`.
**Motivo técnico:** El modelo inicial poseía un solo campo `idSede`, lo cual impedía trazar el viaje de la mercadería. Se dividió en Origen y Destino.
**Impacto funcional:** Permite renderizar en las Tarjetas del Front-end de dónde salió y hacia dónde va exactamente la mercadería.

### [2026-03-28]
**Descripción del cambio:** Obtención dinámica del Sede de la sesión mediante JWT.
**Motivo técnico:** Evitar vulnerabilidades enviando el ID de Sede desde el frontend. 
**Impacto funcional:** El backend (`AuthController` y `TransferenciaController`) ahora lee el Claim `sede_id` del token para autorizar las solicitudes y fijar obligatoriamente la SedeOrigen/Destino según corresponda.

### [2026-03-28]
**Descripción del cambio:** Corrección del ruteo 500 al finalizar la Creación.
**Motivo técnico:** ASP.NET Core MVC explotaba con `InvalidOperationException` al intentar emitir la respuesta `CreatedAtAction` porque faltaba el Endpoint `GetById`. Se cambió la devolución a `Ok()`.
**Impacto funcional:** El Frontend ahora recibe una confirmación exitosa y cierra el modal Swal2 limpiamente en verde tras finalizar la transacción.

### [2026-03-28]
**Descripción del cambio:** Hard-Mapping de CamelCase en Tablas de MySQL.
**Motivo técnico:** Entity Framework Core genera sentencias en PascalCase (`IdSedeDestino`), lo cual chocaba con el entorno local de desarrollo MySQL del usuario que registraba en Base de Datos de manera estricta `idSedeDestino` (minúsculas). Se agregaron reglas `HasColumnName("id..")` directas en el `InventoryDbContext`. Se corrió un script limpio borrando las tablas mal mapeadas y se actualizó vía `dotnet ef database update`.
**Impacto funcional:** Solucionó los choques al registrar la Transferencia en MySQL, blindando la Base de Datos contra problemas de Case Sensitivity entre SO Linux/Windows.

### [2026-03-28]
**Descripción del cambio:** Cambio de Arquitectura a "Pull-Based Transfers" e integración con Catálogo Global de Productos.
**Motivo técnico:** El modelo original (`Push-Based`) asumía que el creador de la solicitud era quien enviaba la mercadería. Se detectó que el requerimiento de negocio exigía que el creador de la solicitud funcione como el *receptor* que le **pide** mercadería a otra sede. Se refactorizaron los Repositorios y Controladores para invertir la asignación de `IdSedeOrigen` e `IdSedeDestino`.
**Impacto funcional:** 
- Las bandejas de "Salientes" y "Entrantes" en Angular ahora reflejan correctamente el sentido del *Mensaje/Solicitud* en lugar del flujo físico primario (Saliente = Yo pido; Entrante = Me piden). 
- El Selector de productos ahora permite elegir y buscar sobre la lista maestra del `/api/productos`, combinando las cantidades físicas desde `/api/stock` correspondientes únicamente a la sede original seleccionada, resolviendo la restricción anterior de no poder pedir productos sin historial previo de stock.

### [2026-03-28]
**Descripción del cambio:** Refactor UX/UI (Tarjetas Horizontales, Nomenclatura, Modal de Detalles e Inteligencia de Stock).
**Motivo técnico:** Clarificar la experiencia de usuario y añadir capacidades de validación en tiempo de aprobación mediante consulta histórica y en vivo de Stock Sede. 
**Impacto funcional:** 
- Nomenclaturas pasaron a "Pedidos a Mi Sede (Recibidos)" y "Mis Pedidos Emitidos".
- Tarjetas `TransferenciaCardComponent` migraron de un diseño vertical a horizontal (panorámico).
- Integración del "Ojito" (Ver Detalles) con un sub-modal profundo que captura la columna `StockOrigenSnapshot` recién agregada en Entity Framework y dispara un HTTP Get pasivo al backend para arrojar una advertencia luminosa en color Ámbar si la Sede Origen no está capacitada físicamente para aprobar el pedido en la actualidad.

### [2026-04-16]
**Descripción del cambio:** Refactor de Base de Datos y Formulario hacia modelo Multiproducto (1 a N).
**Motivo técnico:** El diseño lógico anterior ("Una transferencia = un producto") era limitante y requería generar N solicitudes separadas manuales para un pedido completo. Se separó al responsable de la meta data `Transferencia.cs` y su mercadería interna `TransferenciaDetalle.cs`.
**Impacto funcional:**
- Se purgó el histórico preexistente de base de datos debido al hard-reset estructural del esquema.
- El Navbar Lateral agrupa ahora "Gestionar" (Órdenes activas) e "Histórico" en un dropdown de submenús.
- Al generar una nueva transferencia, el Interfaz provee un FormArray dinámico para ir encolando N productos con diferentes cantidades, todo empaquetado bajo un único Número de ID Transferencia visible globalmente.

### [2026-04-16]
**Descripción del cambio:** Generación dinámica de Código de Rastreo (TR) y rediseño a Doble Modal.
**Motivo técnico:** Proveer una experiencia de usuario que mejore el autocompletado sin deformar el UI, y dotar de un código TR legible. Se hizo mediante lógica calculada en el DTO sin necesidad de tocar la base de datos de Entity Framework.
**Impacto funcional:**
- Las transferencias se rigen visualmente por un código como `TR-160426-5` (donde 5 es el ID incremental, precedido por la fecha DDMMAA).
- El autocompletador de inventario ahora vive en un Modal Secundario que se interpone temporalmente, retornando automáticamente los productos clickeados hacia el carrito del Modal Principal.
- Refactorización transversal: Se extirpó la "Ficha de Producto" que vivía harcodeada adentro de `stock.html` y se la convirtió en un `Shared Component` reutilizable en `/shared/components/ficha-producto-modal/`. Esto permitió invocarla en Transferencias mediante un nuevo botón "Ver" durante la selección de stock (con respeto al patrón DRY).

### [2026-05-21]
**Descripción del cambio:** Corrección de la validación de Sede de Origen y Destino iguales.
**Motivo técnico:** Desajuste entre el frontend (que enviaba la Sede en Contexto como Origen) y el backend (que al forzar la Sede Destino a la Sede en Contexto con el nuevo paradigma, provocaba que ambas sedes fuesen iguales). Además, simplificación de GetCurrentUserSedeId() para confiar en el header Sede-Contexto y alinear la arquitectura.
**Impacto funcional:** 
- El frontend mapea correctamente la Sede Seleccionada a IdSedeOrigen y la Sede en Contexto a IdSedeDestino.
- El backend en TransferenciasController fue unificado para leer el header inyectado por el interceptor, resolviendo el bloqueo de validación al crear transferencias.

### [2026-05-21] (Ajuste de BD)
**Descripción del cambio:** Corrección de la falta de `AUTO_INCREMENT` en la tabla `HistorialTransferencia`.
**Motivo técnico:** Se detectó un error `Duplicate entry '0' for key 'PRIMARY'` al intentar registrar el historial de las transferencias. Esto se debía a que Entity Framework Core mapeaba la clave primaria como auto-incremental, pero la tabla física en MySQL carecía de dicho atributo, provocando que MySQL insertara el valor por defecto `0` e impidiera subsiguientes registros.
**Impacto funcional:** 
- Alteración directa mediante script SQL a la tabla `HistorialTransferencia` para añadir `AUTO_INCREMENT` a `idHistorialTransferencia`.
- El sistema ahora puede guardar múltiples registros de histórico sin colisionar en la clave primaria.

### [2026-05-21] (Refactor UI/UX)
**Descripción del cambio:** Sustitución de los diálogos nativos del navegador por modales de confirmación con SweetAlert2.
**Motivo técnico:** Mejorar la experiencia de usuario y mantener la consistencia estética con el resto del proyecto. Los métodos `window.confirm` y `window.prompt` interrumpían el flujo visual de la aplicación.
- Las acciones de "Aceptar", "Rechazar", "Recibir" y "Devolver" en el panel de transferencias ahora utilizan interfaces ricas de `Swal.fire` configuradas con íconos, botones coloreados y (en el caso de rechazo) un campo de texto con validación integrada.

### [2026-05-21] (Histórico Global y Filtro Multi-Select)
**Descripción del cambio:** 
- Se agregó el endpoint global `[HttpGet("all")]` en la API y el repositorio para obtener la lista irrestricta de transferencias.
- El frontend `historico-transferencias.component.ts` se desvinculó de la Sede en Contexto para el panel Histórico.
- Se implementó un filtro Multi-Select moderno para productos (con lógica estricta AND) que evita saturar la BD usando los productos extraídos dinámicamente.
**Motivo técnico:** Separar el concepto operativo local ("Gestionar") del panel de consulta cruzada ("Histórico"). El Histórico es un registro auditor que sirve visualmente a todas las sedes por igual, independientemente de qué sede esté seleccionada en el Layout general.
**Impacto funcional:** 
- Si un usuario cambia la "Sede:" en el sidebar, el Histórico no se limpia ni re-filtra, sino que permanece global.
- Ahora es posible realizar búsquedas en la tabla combinando múltiples productos, donde el motor filtrará solo aquellas transferencias que incluyan el carrito completo de productos consultados.
