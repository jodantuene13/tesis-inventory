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

**[2026-05-04] (Filtros en Catálogo de Productos)**
- **Descripción del cambio**: Se extendió `ProductoDto` para incluir la proyección de `IdRubro` y `NombreRubro`. Se implementó el componente de filtros avanzado tipo "Stock Local" dentro del Catálogo de Productos y se reemplazó la visualización de la familia por una Clasificación combinada (Rubro + Familia).
- **Motivo técnico**: Proveer una UX consistente entre módulos y permitir búsquedas refinadas a nivel de catálogo. La proyección de Rubro en el DTO evita llamadas adicionales y simplifica el filtrado en cliente que se usa en esta pantalla.
- **Impacto funcional**: El usuario ahora puede filtrar el listado global de productos por Nombre/SKU, Rubro, Familia, Estado y Unidad de Medida directamente desde el frontend. La grilla resulta más legible al mostrar la clasificación jerárquica completa.

**[2026-05-04] (Normalización de Interfaz en Tablas y Paginación)**
- **Descripción del cambio**: Se estandarizaron las vistas de tablas (Productos, Remitos / Op. Múltiples e Historial de Movimientos) adoptando la estructura CSS consolidada en `ds-table-wrapper`, `ds-table`, `ds-table-empty` y `ds-table-footer`. Además, se implementó paginación local en la pantalla de Productos.
- **Motivo técnico**: Mantener consistencia del Design System (DS) en todas las grillas principales del módulo de Inventario y estandarizar el comportamiento de la paginación.
- **Impacto funcional**: Todas las tablas del módulo ahora comparten la misma apariencia gráfica y controles de paginación idénticos (Página X de Y · Z resultados) que mejoran sustancialmente la legibilidad y UX en pantallas reducidas.

**[2026-05-04] (Refactorización de Solicitudes de Compra a modelo Cabecera-Detalle)**
- **Descripción del cambio**: Se migró la entidad `SolicitudCompra` a un esquema Cabecera-Detalle, agregando la nueva entidad `SolicitudCompraDetalle`. El frontend fue actualizado para incluir un carrito de productos que reutiliza el diseño de "Operaciones Múltiples".
- **Motivo técnico**: Proveer soporte para solicitar múltiples productos (con cantidades independientes) dentro de un único proceso de autorización y capturar metadatos organizacionales vitales, tales como la Orden de Trabajo (OT) y el Ticket de soporte relacionado.
- **Impacto funcional**: Los solicitantes ahora pueden armar un "carrito" de productos variados en una sola solicitud, justificando la compra en bloque. El Comprobante de impresión y el Visor de Detalles ahora presentan un remito estandarizado de la solicitud completa.

**[2026-05-12] (UI: Condicional de Orden de Compra en Operaciones Múltiples)**
- **Descripción del cambio**: Se añadió un control lógico en el modal de Operaciones Múltiples de Stock. Cuando se elige la operación "Ingresar Stock" por motivo "Por Compra", aparece un checkbox "Requiere OC". El campo de texto de Orden de Compra se encuentra oculto y deshabilitado por defecto hasta que se marque dicha casilla.
- **Motivo técnico**: Regla de negocio para mejorar la UX y limpiar la interfaz, forzando a que la captura de una Orden de Compra sea una acción explícita del usuario cuando genuinamente exista una.
- **Impacto funcional**: Evita la carga accidental de datos en el campo OC y reduce el ruido visual en ingresos de compras menores o remitos que no proceden de una OC formal.

**[2026-05-12] (Integración Solicitudes de Compra con Operaciones Múltiples de Stock y sistema de Etiquetas)**
- **Descripción del cambio**: Se vinculó el módulo de Solicitudes de Compra con Operaciones Múltiples de Stock. Al aprobar una solicitud, se habilita la acción "Impactar Stock", que instancia el modal de operaciones. La solicitud controla cumplimientos parciales y asume una **Etiqueta** logística (`ParcialmenteIngresada`, `IngresadaAlStock`, `NoConcretada`) paralela al estado de aprobación. Se eliminaron los estados híbridos ("Parcial" y "Completada") del workflow de aprobación principal.
- **Motivo técnico**: Requisito crítico para cerrar el ciclo de abastecimiento separando la responsabilidad de "Autorización" (Aprobada/Rechazada) de la "Ejecución" (Ingresada parcial o totalmente). Se añadió la posibilidad de marcar manualmente una Solicitud Aprobada como "No concretada" en caso de falla del proveedor.
- **Impacto funcional**: Al momento de la llegada de los insumos físicos, el usuario "Impacta Stock" precargando el listado original. Si recibe un pedido fraccionado, la solicitud obtendrá la etiqueta "Ingreso Parcial". Adicionalmente, el usuario puede cancelar la espera de stock marcando la orden como "No concretada", cerrando visualmente el proceso y desactivando la opción de seguir impactando inventario.

**[2026-05-19] (Corrección de diseño en paginación de tablas)**
- **Descripción del cambio**: Se actualizó la clase global CSS `.ds-table-wrapper` con propiedades flex (`flex-col`) y `.ds-table-footer` con `margin-top: auto`.
- **Motivo técnico**: Solucionar un problema de diseño en el que el pie de paginación de las tablas (ej. Remitos e Historial de Movimientos) se renderizaba en el medio del contenedor cuando la grilla no tenía registros (o no cubría la altura mínima del contenedor).
- **Impacto funcional**: La barra de paginación ahora se posiciona consistentemente y siempre en la base inferior del panel sin importar la cantidad de resultados de la búsqueda, ofreciendo una experiencia visual limpia.

**[2026-05-19] (Ajuste visual en grilla de Solicitudes de Compra)**
- **Descripción del cambio**: Se modificó la renderización de la tabla principal de Solicitudes de Compra. Se creó una nueva columna dedicada a "Etiquetas" separada de "Estado". Adicionalmente, se mejoró la lectura de productos resumiendo las cantidades (sumatoria total) y estandarizando la vista del primer producto para órdenes múltiples (mostrando "Producto X" y debajo "y N más").
- **Motivo técnico**: Proveer una UX más limpia donde la columna de estado no se sobrecargue con badges de estado de aprobación y etiquetas de recepción simultáneamente, y simplificar la lectura rápida de volúmenes de compra.
- **Impacto funcional**: Los usuarios ahora pueden identificar claramente si el estado "Aprobado" y el progreso de recepción logística ("Ingreso Parcial") operan por carriles separados. La sumatoria de unidades previene la ambigüedad del texto previo "VARIAS".

**[2026-05-19] (Normalización de UI en Rubros y Familias)**
- **Descripción del cambio**: Se refactorizaron las tablas de "Rubros" y "Familias" en `rubros-familias.component.html` para utilizar el sistema de diseño estándar `.ds-table-wrapper` y `.ds-table`.
- **Motivo técnico**: Las tablas no utilizaban el diseño estandarizado (Design System). Específicamente, en "Rubros" el atributo `sticky` de la cabecera, sumado al padding, estaba causando un gap visual por donde se veían los items hacer scroll.
- **Impacto funcional**: Mayor consistencia visual con el resto de los módulos del sistema y se soluciona la falla estética del scroll desfasado.
### [2026-05-19] Normalización visual en pantallas de inventario (Stock, Productos, Atributos, Remitos e Historial)
- **Descripción del cambio:** Se aplicó una estructura `flex` de pantalla completa a los componentes del inventario y se independizó el scroll de las tablas mediante `.ds-table-wrapper`. Los paginadores ahora quedan anclados al final de la pantalla y el encabezado de las tablas queda fijo (`sticky top-0`).
- **Motivo técnico:** Solucionar el problema de cabeceras flotantes (gap visual al hacer scroll) y estandarizar el comportamiento de la paginación globalmente.
- **Impacto funcional:** Mejor UX al leer tablas largas con paginación, logrando que el usuario no pierda contexto visual de las columnas. Se modificaron los archivos `stock.html`, `productos.component.html`, `atributos.component.html`, `remitos.component.html` e `historial-movimientos.component.html`.

### [2026-05-19] Reestructuración del Menú Lateral (Sidebar)
- **Descripción del cambio:** El titular de la sección se renombró a "Administración" y su menú desplegable operativo a "Configuración Admin". Se creó un nuevo grupo desplegable llamado "Paramétricas del Gestor" bajo este mismo titular, moviendo allí los submenús de "Rubros y Familias", "Atributos" y "Productos", que antes pertenecían a "Inventario".
- **Motivo técnico:** Mejorar la organización del menú lateral y agrupar las configuraciones paramétricas (entidades maestras del catálogo) de manera independiente para evitar sobrecargar el menú operativo del Inventario.
- **Impacto funcional:** Mayor claridad para el usuario a la hora de navegar entre operaciones diarias (Inventario/Remitos) y gestión de maestros paramétricos. Se modificaron los componentes `admin-layout.component.ts` y `admin-layout.component.html`.

### [2026-05-19] Corrección de doble renderizado en Ficha de Producto (Stock Local)
- **Descripción del cambio:** Se eliminó el modal de ficha antiguo (inline) de `stock.html` que convivía con el componente reutilizable `app-ficha-producto-modal`, provocando que ambos modales se abrieran superpuestos al dar click en "Ver Ficha". Adicionalmente, el formato base de `.modal-box` (del *Design System*) fue integrado internamente dentro del componente `app-ficha-producto-modal` para garantizar que la vista contenga la funcionalidad de "Atributos del producto" preservando los estilos estandarizados globales solicitados.
- **Motivo técnico:** Había una duplicación del estado `activeModal === 'detalle'` que gatillaba dos bloques de HTML distintos simultáneamente. Además el componente compartido no estaba aplicando la clase `.modal-box`.
- **Impacto funcional:** Solo se renderiza un único modal con la información consolidada (incluyendo atributos) y un fondo correcto, evitando la doble superposición. Modificados `stock.html` y `ficha-producto-modal.component.html`.

### [2026-05-19] Eliminación de botón "Inhabilitar" en Stock Local
- **Descripción del cambio:** Se retiró el botón de acción rápida "Dar de baja" (Inhabilitar) que aparecía en cada fila de la tabla del componente `stock.html`.
- **Motivo técnico:** Solicitud directa de simplificación de UI para la pantalla de Stock Local.
- **Impacto funcional:** Menor carga visual en las filas de stock. Se modificó únicamente el frontend en `stock.html`.

