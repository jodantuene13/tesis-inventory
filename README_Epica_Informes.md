# Épica: Módulo de Informes

## Objetivo

Proveer al sistema Tesis Inventory un módulo de análisis e informes que permita a usuarios operativos y de gestión obtener datos estratégicos y operativos del inventario para la toma de decisiones.

El módulo se integra como una sección independiente dentro del layout principal del sistema, accesible desde el menú lateral bajo el ítem **Informes**.

---

## Historias de Usuario

| ID           | Historia                                                                                      | Estado     |
|--------------|-----------------------------------------------------------------------------------------------|------------|
| HU-INF-01    | Ver pantalla de informe "Stock bajo y alertas"                                                | ✅ Implementada |
| HU-INF-02    | Filtrar resultados por sede, familia, período, estado y top N                                 | ✅ Implementada |
| HU-INF-03    | Visualizar KPIs resumidos del informe (4 tarjetas por sección)                               | ✅ Implementada |
| HU-INF-04    | Ver gráficos dinámicos interactivos (barras horizontales, línea temporal)                     | ✅ Implementada |
| HU-INF-05    | Ver tabla de datos base del informe con ordenamiento y búsqueda                               | ✅ Implementada |
| HU-INF-06    | Exportar datos a CSV                                                                          | ✅ Implementada |
| HU-INF-07    | Ver más registros más allá del Top N seleccionado                                             | ✅ Implementada |
| HU-INF-08    | Ver pantalla de informe "Rotación de productos" con tres sub-vistas integradas               | ✅ Implementada |
| HU-INF-09    | Filtrar rotación por período (30d / 90d / personalizado), sede, familia y top N              | ✅ Implementada |
| HU-INF-10    | Ver ranking principal por índice de rotación (Egresos / Stock ponderado)                     | ✅ Implementada |
| HU-INF-11    | Ver desagregado de productos que más ingresaron por período y sede                            | ✅ Implementada |
| HU-INF-12    | Ver desagregado de productos que más egresaron por período y sede                            | ✅ Implementada |

---

## Alcance funcional

El módulo cubre inicialmente una pantalla de informe que combina dos análisis relacionados:

### Informe 1: Productos en alerta bajo stock (Top N por menor stock disponible)

Muestra un ranking de productos con bajo stock, ordenados de menor a mayor según unidades disponibles.

**KPIs:**
- Productos en alerta actualmente
- Producto con menor stock disponible
- Promedio de unidades disponibles
- Días promedio en alerta

**Visualización:** Gráfico de barras horizontales (Chart.js). Colores: rojo (Alta), naranja (Media), verde (Baja).

**Tabla:** Detalle de productos con columnas ordenables (producto, familia, sede, stock actual, stock mínimo, diferencia, días en alerta, última alerta, criticidad).

---

### Informe 2: Productos con mayor índice de recurrencia de alertas

Muestra un ranking de productos con mayor cantidad de eventos de alerta registrados en el período seleccionado.

**KPIs:**
- Producto con mayor recurrencia
- Total de alertas en el período
- Promedio de alertas por producto
- Familia con mayor recurrencia

**Visualización principal:** Gráfico de barras horizontales (Chart.js).

**Visualización complementaria:** Gráfico de línea temporal con evolución semanal de alertas.

**Tabla:** Detalle con columnas ordenables (producto, familia, sede, cantidad de alertas, días acumulados, stock actual, stock mínimo, última alerta, estado actual, criticidad).

---

## Estructura de archivos implementados

```
frontend/src/app/
├── pages/
│   └── informes/
│       └── alertas-stock/
│           ├── alertas-stock.component.ts   ← Lógica + llamadas HTTP reales
│           ├── alertas-stock.component.html ← Template actualizado (loading/error/filtros reales)
│           └── alertas-stock.component.css  ← Agrega loading-state y error-state
├── services/
│   └── informes.service.ts              ← [NUEVO] Servicio HTTP de informes
├── layout/
│   └── admin-layout/
│       ├── admin-layout.component.html
│       └── admin-layout.component.ts
└── app.routes.ts

backend/
├── TesisInventory.Domain/
│   ├── Entities/AlertaStock.cs              ← [NUEVO] Entidad histórica de alertas
│   ├── Enums/EstadoAlerta.cs               ← [NUEVO] Enum Activa/Resuelta
│   └── Interfaces/IAlertaStockRepository.cs ← [NUEVO] Contrato del repositorio
├── TesisInventory.Application/
│   ├── DTOs/Informes/                       ← [NUEVO] DTOs del informe
│   ├── Interfaces/IAlertaStockService.cs   ← [NUEVO] Interface del servicio de alertas
│   ├── Interfaces/IInformesService.cs      ← [NUEVO] Interface del servicio de informes
│   ├── Services/AlertaStockService.cs      ← [NUEVO] Lógica de detección de alertas
│   ├── Services/InformesService.cs         ← [NUEVO] Orquestación del informe
│   └── Services/StockService.cs            ← [MODIFICADO] Inyecta IAlertaStockService
├── TesisInventory.Infrastructure/
│   ├── Repositories/AlertaStockRepository.cs  ← [NUEVO] Implementación del repositorio
│   └── Persistence/InventoryDbContext.cs      ← [MODIFICADO] DbSet + configuración
└── TesisInventory.API/
    ├── Controllers/InformesController.cs      ← [NUEVO] GET /api/informes/alertas-stock
    └── Program.cs                             ← [MODIFICADO] Registro DI
```

---

## Capas involucradas

- **Frontend (Angular)**: Standalone components con lazy load.
- **Backend**: Modificado. Nuevo `InformesController`, `InformesService`, `AlertaStockService`.
- **Base de datos**: Modificada. Nueva tabla `AlertaStock` para registro histórico de alertas.

---

## Componentes y patrones reutilizables utilizados

| Elemento          | Clase/referencia               |
|-------------------|-------------------------------|
| KPI Cards         | `.kpi-card`, `.kpi-grid`       |
| Data table        | `.ds-table`, `.informe-table`  |
| Badges criticidad | `.badge-criticidad`, `.badge-crit-alta/media/baja` |
| Gráficos          | Chart.js (ya instalado en el proyecto) |
| Filtros sidebar   | `.report-sidebar`, `.filter-section` |
| Layout principal  | `.report-page`, `.report-main` |
| Tabs              | `.report-tabs`, `.tab-btn`     |

---

## Navegación

El módulo es accesible desde el menú lateral bajo:

```
Informes
  └── Stock bajo y alertas → /informes/stock-alertas
```

El submenú se activa con toggle (igual patrón que Inventario, Transferencias y Configuración).

---

## Decisiones de implementación y ajustes

### [2026-05-29]

**Cambio:** Implementación inicial del módulo de Informes – Épica completa.

**Motivo técnico:**
- Se utilizaron datos mockeados realistas (10 productos de ejemplo) dado que no existen endpoints en el backend para este módulo aún.
- Se eligió Chart.js (ya disponible como dependencia extraneous v4.5.1) para las visualizaciones gráficas por su facilidad de integración con Angular sin necesidad de wrappers adicionales.
- Se optó por tabs internas (no rutas separadas) para mantener una única URL y contexto de filtros compartido entre los dos informes de la misma pantalla.
- El panel de filtros se posicionó como sidebar sticky para maximizar el espacio de visualización y mantener los filtros visibles al hacer scroll.
- Los gráficos se destruyen y recrean en cada cambio de tab y aplicación de filtros para evitar conflictos de canvas en Chart.js.

**Impacto funcional:**
- Nueva sección "Informes" visible en el sidebar para todos los usuarios autenticados.
- La pantalla responde correctamente a filtros de sede, familia, estado, período y top N.
- Exportación a CSV funcional para ambos informes.
- Tablas con ordenamiento por columnas y búsqueda por nombre de producto.

---

### [06-06-2026] Informe Transferencias y Préstamos
- **Motivo técnico:** Se requirió agregar `FechaDevolucionEsperada` y `FechaDevolucionReal` en la entidad `Transferencia` mediante migración de EF Core para poder calcular correctamente la curva de préstamos activos en el tiempo.
- **Impacto funcional:** Los préstamos ahora pueden trackearse en el tiempo en un gráfico de líneas, y permite calcular métricas como el tiempo promedio de devolución.
- **Cálculo de rechazos:** Se utiliza el campo `Observaciones` en el historial de transferencia cuando el estado cambia a `Rechazada` como "motivo principal".

### [06-06-2026] Informe Rotación de productos

**Cambio:** Implementación del informe "Rotación de productos" — HU-INF-08 a HU-INF-12.

**Motivo técnico:**
- La fórmula de rotación acordada es: `Índice = Egresos / Stock Promedio Ponderado por Tiempo`, donde el stock promedio ponderado = Σ(Stockᵢ × Díasᵢ) / días_totales. Se reconstruye la curva usando `CantidadRestante` de cada movimiento como punto de control.
- No se creó ninguna tabla nueva en la base de datos. Los datos se calculan en tiempo real desde `Movimiento` (join con `Producto`, `Familia`, `Sede`).
- Se agregó el método `GetDatosRotacionAsync` a `IMovimientoRepository` / `MovimientoRepository` con filtros por sede, familia y rango de fechas.
- Se amplió `IInformesService` / `InformesService` con `GetRotacionProductosAsync` que produce tres listas: rotación, mayorIngreso, mayorEgreso.
- Se solucionó de paso un error de compilación pre-existente: `GetStocksEnBajoStockAsync` estaba declarado en `IStockRepository` pero no implementado en `StockRepository`.
- El frontend usa el mismo sistema de diseño (clases CSS, chips, KPI grid, Chart.js barras horizontales) que `alertas-stock` para consistencia visual.
- El período "Personalizado" muestra inputs de fecha cuando se selecciona; los presets aplican y recargan automáticamente.
- Tendencia (Alta/Media/Baja) se calcula comparando el índice del producto contra el promedio del período × factores 1.2 / 0.8.

**Impacto funcional:**
- Nuevo submenú "Rotación de productos" bajo Informes → `/informes/rotacion-productos`.
- Tres tabs integradas: Rotación / Ingresos / Egresos con gráficos independientes y tablas exportables a CSV.
- Top N aplica solo al ranking de rotación; ingresos y egresos muestran todos con paginación tipo "Ver más".
- Misma lógica de sede que el resto del sistema (Admin ve todas, Depósito ve solo la propia).

---

## Pendientes / Próximos pasos

- [x] Conectar con endpoints reales del backend cuando estén disponibles. (**✅ Implementado**)
- [ ] Aplicar migración EF Core (`dotnet ef migrations add AddAlertaStockTable`) y actualizar BD.
- [ ] Agregar permiso `Informes_Ver` al sistema de roles para control de acceso granular.
- [x] Implementar informe de Rotación de productos. (**✅ Implementado**)
- [ ] Implementar informe de transferencias.
- [ ] Implementar informe de solicitudes de compra.
- [ ] Considerar cache de resultados para grandes volúmenes de datos.

