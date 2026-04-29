# Épica: Módulo Inventario

## Objetivo de la Épica
Construir y modernizar de raíz el módulo de Inventario reemplazando el antiguo modelo plano por un sistema de organización estructurado en dos niveles (Rubros y Familias) con la capacidad de definir atributos dinámicos obligatorios adaptables según la Familia del producto.
También incluye un sistema de autogeneración de SKU (Stock Keeping Unit).

## HU (Historias de Usuario) Incluidas
- **HU-INV-01**: ABM de Rubros.
- **HU-INV-02**: ABM de Familias asociadas a Rubros.
- **HU-INV-03**: Mantenimiento de Atributos del Sistema (Diccionario Global).
- **HU-INV-04**: Asociación y configuración de Atributos dentro de Familias de productos (Incluye la desvinculación selectiva con auditoría visual).
- **HU-INV-05**: Gestión Avanzada de Productos (con auto-SKU y captura dinámica de atributos).
- **HU-INV-06**: Adaptación de la relación Stock transaccional entre Sede y Producto.
- **HU-INV-07**: Implementación del Submódulo Stock: Control de Inventario por Sede, registro de Consumo/Egresos, Adición de Stock por Compra/Ajustes, y Transferencias Logísticas Inter-Sedes con Historial.
- **HU-INV-08**: Desarrollo de vista global de Historial de Movimientos de Stock, con panel de filtros avanzados y detalle de saldos restantes.

## Alcance Funcional
- **Rubros y Familias**: Flujo de gestión a doble vista interdependiente. Un Rubro agrupa Familias, estas últimas engloban a su vez el catálogo real de productos. Todo soporta Baja Lógica.
- **Atributos**: Tipos de datos soportados (`STRING`, `NUMBER`, `DECIMAL`, `BOOLEAN`, `LIST`).
- **Productos**: El SKU asume la forma de concatenación del Código de Rubro + Código de Familia + Nro Correlativo. Al crearlo, la UI genera los controles visuales correspondientes basados en las normas estrictas impuestas por la asociación con su familia (FamiliaAtributo). El borrado del producto exige confirmación exacta validando su nombre.
- **Stock**: Visualización detallada del inventario por sede. Permite registrar consumos/egresos (con motivos predefinidos), incrementar stock y solicitar transferencias hacia otras sedes, registrando una trazabilidad (Movimientos y Transferencias) completa.

## Capas Involucradas
- **Domain**: `Rubro.cs`, `Familia.cs`, `Atributo.cs`, `FamiliaAtributo.cs`, `AtributoOpcion.cs`, `Producto.cs`, `ProductoAtributoValor.cs`, `Stock.cs`, refactor de `Sede.cs`. Adición de `Movimiento.cs`, `Transferencia.cs`, `HistorialTransferencia.cs`.
- **Infrastructure**: Implementación de RepositoryPattern (`RubroRepository`, `StockRepository`, `MovimientoRepository`, `TransferenciaRepository`, etc.), inyección por Entity Framework y Migraciones (`InventorySchemaInitialization`, `AddStockModule`).
- **Application**: DTOs completos de Lectura/Escritura y lógica pesada abstraída en `ProductosService.cs`, `AtributosService.cs`, `StockService.cs` y análogos.
- **Presentation**: `*Controller.cs` exponiendo endpoints REST estándar (`StockController`).
- **Frontend (Angular)**: Módulo `inventory` implementando componentes (`rubros-familias`, `atributos`, `productos`, `stock`) y el manejo de modales integrados para transacciones logísticas y consultas del almacén.

## Flujo General
1. Usuario crea Rubro "Bazar".
2. Usuario crea Familia "Tazas" englobada en "Bazar".
3. Se crea el atributo maestro "Capacidad" (NUMBER, ml) y se asinga obligatoriamente a "Tazas".
4. Al ir a agregar un inventario, eligen rubro/familia, el formulario pide la Capacidad obligatoria. Luego, el servidor arroja el SKU auto-armado y persiste el catálogo.

## Decisiones de Implementación y Ajustes
**[2026-04-14]**
- **Descripción del cambio**: Implementación de contexto global (interceptor HTTP y SedeContextService en UI) para el manejo dinámico de la sede seleccionada en componentes dependientes del inventario (Stock / Movimientos).
- **Motivo técnico**: Se optó por una arquitectura reactiva (RxJS BehaviorSubject) en sustitución de inyección manual de parámetros en Sede para asegurar la recarga automática (auto-refresco y refetching) de los grids de datos en toda la aplicación Inventario cuando el Administrador cambia la sede en el layout global.
- **Impacto funcional**: Mejora sustancial en la UX del Administrador al consultar inventarios sedes-afines. Los grids de datos listarán estricta y únicamente el stock y transferencias correspondientes a la sede actualmente dispuesta en la top-bar por el Admin, transparentando la operación de consulta en multi-sede.

**[2026-04-16]**
- **Descripción del cambio**: Hotfix de sensibilidad a mayúsculas (Case Sensitivity) en comprobación de Rol de Administrador en el Backend.
- **Motivo técnico**: El sistema comparaba de manera exacta `(roleClaim == "Admin")`. Debido a que los roles en la base de datos se habían registrado as `'ADMIN'`, la comparación resultaba falsa, denegando el privilegio de cambio de `Sede-Contexto` y forzando la visualización de la sede estática del usuario. Se reemplazó por `.Equals("Admin", StringComparison.OrdinalIgnoreCase)`.
- **Impacto funcional**: Al cambiar la `Sede-Contexto` en el Header del frontend, el backend ahora obedece exitosamente el contexto para el administrador, refrescando dinámicamente las grillas de Stock y Transferencias para mostrar la información correspondiente a la sede solicitada.

**[2026-04-16] (Sincronización de Sedes)**
- **Descripción del cambio**: Autocompletado de registros de Stock al instanciar una nueva Sede.
- **Motivo técnico**: El módulo de `SedesService` fue inyectado con `IProductoRepository` y `IStockRepository` para remediar una brecha de integridad. Previo a esto, la creación de una Sede nueva no poblaba la tabla intermedia de inventario para el catálogo preexistente, resultando en grids vacíos al consultar el stock.
- **Impacto funcional**: Al crear una Sede, ésta adopta inmediatamente todo el catálogo de productos registrados en el sistema, figurando en la grilla visual de la GUI con `CantidadActual = 0`. Mantiene consistencia de inventario multi-sucursal garantizando visibilidad inmediata.

**[2026-04-21] (Implementación de Operaciones Múltiples de Stock)**
- **Descripción del cambio**: Se añadió la capacidad de procesar ingresos y egresos de stock para múltiples productos de forma atómica en el backend y frontend.
- **Motivo técnico**: Requerimiento de negocio para optimizar escenarios reales donde un operario registra consumos (ej. varias herramientas en una OT) o ingresos (varios insumos por OC) en un solo lote. Se creó la entidad transaccional `OperacionStock` para agrupar bajo un remito lógico a los respectivos `Movimiento` individuales.
**[2026-04-21] (Implementación de Remitos / Visor de Operaciones)**
- **Descripción del cambio**: Se añadió el Sub-menú "Remitos / Op. Múltiples" que permite visualizar y filtrar el historial de las Operaciones agrupadas.
- **Motivo técnico**: Proveer una grilla de control unificada con capacidad de exportación e impresión basada en las transacciones generadas en "Operaciones Múltiples".
- **Impacto funcional**: Administradores pueden ubicar lotes de consumo/ingreso por fecha, tipo u OC/OT y visualizar con un solo click un formato nativo de Remito para imprimir o exportar a PDF de forma limpia desde el navegador (`@media print`).

**[2026-04-29] (Unificación de flujo de operaciones de stock)**
- **Descripción del cambio**: Los modales individuales de incremento y consumo de stock de la vista Stock Local fueron deprecados y se consolidó el flujo a través del componente de "Operaciones Múltiples".
- **Motivo técnico**: Centralizar la lógica transaccional de stock y reutilizar el componente avanzado, reduciendo el código y modales redundantes (DRY) que existían en el componente de Stock Local.
- **Impacto funcional**: Al hacer clic en los botones de "Registrar Consumo" o "Incrementar Stock" individuales de la tabla, se abrirá dinámicamente el modal de Operaciones Múltiples con el tipo de operación preseleccionado y el producto ya agregado automáticamente al "carrito" para que el usuario solo defina la cantidad.
