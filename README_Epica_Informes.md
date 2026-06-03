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
│           ├── alertas-stock.component.ts   ← Lógica + datos mockeados
│           ├── alertas-stock.component.html ← Template con sidebar + main
│           └── alertas-stock.component.css  ← Estilos del módulo
├── layout/
│   └── admin-layout/
│       ├── admin-layout.component.html      ← Modificado: menú Informes
│       └── admin-layout.component.ts        ← Modificado: isInformesMenuOpen, toggleInformesMenu()
└── app.routes.ts                            ← Modificado: ruta /informes/stock-alertas
```

---

## Capas involucradas

- **Frontend (Angular)**: Standalone components con lazy load.
- **Backend**: No modificado. Se utilizan datos mockeados en el componente.
- **Base de datos**: No modificada.

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

## Pendientes / Próximos pasos

- [ ] Conectar con endpoints reales del backend cuando estén disponibles.
- [ ] Agregar permiso `Informes_Ver` al sistema de roles para control de acceso granular.
- [ ] Implementar informe de transferencias.
- [ ] Implementar informe de solicitudes de compra.
- [ ] Considerar cache de resultados para grandes volúmenes de datos.
