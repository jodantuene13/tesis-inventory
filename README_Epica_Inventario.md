# Épica: Módulo Inventario

## Objetivo de la Épica
Construir y modernizar de raíz el módulo de Inventario reemplazando el antiguo modelo plano por un sistema de organización estructurado en dos niveles (Rubros y Familias) con la capacidad de definir atributos dinámicos obligatorios adaptables según la Familia del producto.
También incluye un sistema de autogeneración de SKU (Stock Keeping Unit).

## HU (Historias de Usuario) Incluidas
- **HU-INV-01**: ABM de Rubros.
- **HU-INV-02**: ABM de Familias asociadas a Rubros.
- **HU-INV-03**: Mantenimiento de Atributos del Sistema (Diccionario Global).
- **HU-INV-04**: Asociación y configuración de Atributos dentro de Familias de productos.
- **HU-INV-05**: Gestión Avanzada de Productos (con auto-SKU y captura dinámica de atributos).
- **HU-INV-06**: Adaptación de la relación Stock transaccional entre Sede y Producto.

## Alcance Funcional
- **Rubros y Familias**: Flujo de gestión a doble vista interdependiente. Un Rubro agrupa Familias, estas últimas engloban a su vez el catálogo real de productos. Todo soporta Baja Lógica.
- **Atributos**: Tipos de datos soportados (`STRING`, `NUMBER`, `DECIMAL`, `BOOLEAN`, `LIST`).
- **Productos**: El SKU asume la forma de concatenación del Código de Rubro + Código de Familia + Nro Correlativo. Al crearlo, la UI genera los controles visuales correspondientes basados en las normas estrictas impuestas por la asociación con su familia (FamiliaAtributo). El borrado del producto exige confirmación exacta validando su nombre.

## Capas Involucradas
- **Domain**: `Rubro.cs`, `Familia.cs`, `Atributo.cs`, `FamiliaAtributo.cs`, `AtributoOpcion.cs`, `Producto.cs`, `ProductoAtributoValor.cs`, `Stock.cs`, refactor de `Sede.cs`.
- **Infrastructure**: Implementación de RepositoryPattern (`RubroRepository`, etc.), inyección por Entity Framework y Migraciones (`InventorySchemaInitialization`).
- **Application**: DTOs completos de Lectura/Escritura y lógica pesada abstraída en `ProductosService.cs`, `AtributosService.cs` y análogos.
- **Presentation**: `*Controller.cs` exponiendo endpoints REST estándar.
- **Frontend (Angular)**: Módulo `inventory` implementando componentes (`rubros-familias`, `atributos`, `productos`) y el manejo de `ReactiveForms` (Angular) para el renderizado programático del ABM base de la aplicación.

## Flujo General
1. Usuario crea Rubro "Bazar".
2. Usuario crea Familia "Tazas" englobada en "Bazar".
3. Se crea el atributo maestro "Capacidad" (NUMBER, ml) y se asinga obligatoriamente a "Tazas".
4. Al ir a agregar un inventario, eligen rubro/familia, el formulario pide la Capacidad obligatoria. Luego, el servidor arroja el SKU auto-armado y persiste el catálogo.
