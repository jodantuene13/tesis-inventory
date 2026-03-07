6.X Épica: Configuración

────────────────────────────────────────
6.1 Descripción general de la épica
────────────────────────────────────────
Esta épica resuelve la necesidad de administrar el control de acceso y la estructura organizacional del sistema. Permite al Administrador gestionar los usuarios, sus roles (permisos) y su ubicación física (sedes).
- **Objetivo**: Proveer un módulo centralizado para el Alta, Baja y Modificación (ABM) de usuarios y parámetros globales.
- **Proceso del negocio**: Gestión de Recursos Humanos y Seguridad.
- **Relación con el problema**: Garantiza que solo personal autorizado acceda al sistema y que cada operación quede trazada a un responsable específico, mitigando la falta de control en los inventarios actuales.

────────────────────────────────────────
6.2 Alcance funcional de la épica
────────────────────────────────────────
**Funcionalidades incluidas:**
1.  Listado de usuarios existentes con paginación/scroll.
2.  Alta de nuevos usuarios asignando Rol y Sede.
3.  Edición de usuarios (cambio de Rol o Sede).
4.  Baja lógica de usuarios (Cambio de estado Activo/Inactivo).
5.  Visualización dinámica de Roles y Sedes disponibles desde base de datos.
6.  Eliminación física de usuarios (con confirmación).
7.  ABM Remoto completo de Sedes (Alta, Baja, Modificación con validación estricta de eliminación frente a Usuarios activos).

**Funcionalidades excluidas:**
- Auto-registro de usuarios (solo el Admin invita/crea).
- Gestión granular de permisos por vista (se maneja por Rol fijo).

**Suposiciones o restricciones:**
- El email debe ser institucional (`@ucc.edu.ar`).
- No se puede eliminar al propio usuario logueado.

────────────────────────────────────────
6.3 Relación con requerimientos y casos de uso
────────────────────────────────────────
| Requerimiento | Caso de Uso | Descripción |
| :--- | :--- | :--- |
| **REQ-SEG-01** | **CF01** | Registrar nuevo usuario con validación de correo. |
| **REQ-SEG-02** | **CF02** | Asignar roles (Administrador, Operador, etc.). |
| **REQ-SEG-03** | **CF03** | Modificar datos de usuario (Sede, Rol). |
| **REQ-SEG-04** | **CF04** | Cambiar estado de usuario (Baja lógica). |
| **REQ-SEG-05** | **CF06** | Consultar lista de usuarios con sus atributos. |
| **REQ-SEG-06** | **CF07** | Filtrar/Visualizar usuarios por Sede o Rol. |

────────────────────────────────────────
6.4 Historias de usuario asociadas
────────────────────────────────────────
- **HU-01**: Como Administrador quiero ver el listado de usuarios para conocer quiénes tienen acceso al sistema.
- **HU-02**: Como Administrador quiero registrar un nuevo usuario asignándole un rol y una sede para habilitar su operatividad.
- **HU-03**: Como Administrador quiero editar los datos de un usuario para corregir errores o actualizar su cargo.
- **HU-04**: Como Administrador quiero deshabilitar o eliminar un usuario para revocar su acceso cuando ya no pertenezca a la institución.

────────────────────────────────────────
6.5 Criterios de aceptación
────────────────────────────────────────

| Historia | Criterio de Aceptación |
| :--- | :--- |
| **HU-01** | **Dado** que soy Admin, **Cuando** ingreso a Configuración > Usuarios, **Entonces** veo una tabla con Nombre, Email, Rol, Sede y Estado. |
| **HU-02** | **Dado** que estoy en el formulario, **Cuando** despliego "Rol" o "Sede", **Entonces** veo las opciones cargadas desde la BD (no harcodeadas). |
| **HU-02** | **Dado** que completo el formulario, **Cuando** guardo, **Entonces** el usuario se crea y aparezco nuevamente en la lista actualizada. |
| **HU-04** | **Dado** un usuario en la lista, **Cuando** presiono "Eliminar" y confirmo, **Entonces** el usuario desaparece de la lista. |

────────────────────────────────────────
6.6 Diseño técnico asociado
────────────────────────────────────────
**Componentes de Frontend (Angular):**
- `UsersListComponent`: Tabla inteligente con iteración `*ngFor` y acciones.
- `UserFormComponent`: Formulario reactivo para Crear/Editar.
- `UserService`, `RoleService`, `SedeService`: Servicios HTTP para comunicación con API.

**Lógica de Backend (C# .NET):**
- `UsersController`: API REST (`GET`, `POST`, `PUT`, `DELETE`).
- `SedesController` y `RolesController`: Endpoints de lectura para poblar dropdowns.
- `UserRepository`: Implementa `Include` para traer relaciones (`Rol`, `Sede`).
- `DbInitializer`: Clase para seedear datos iniciales si la BD está vacía.

**Base de datos (MySQL):**
- Tabla `Usuario`: Clave foránea a `Rol` (`idRol`) y `Sede` (`idSede`).
- Tabla `Rol`: Catálogo de roles.
- Tabla `Sede`: Catálogo de ubicaciones.

────────────────────────────────────────
6.7 Implementación de la épica
────────────────────────────────────────
**Descripción de la solución:**
Se implementó una arquitectura Full Stack. En el Backend, se expusieron endpoints que devuelven DTOs enriquecidos (ej. `UserDto` incluye `NombreSede`). En el Frontend, se verificó la correspondencia de puertos y se diseñó una UI limpia usando Tailwind CSS.

**Tecnologías:** Angular 19, .NET 9, MySQL 8, Tailwind CSS.

**Fragmentos Clave:**

*Backend: UserDto con relaciones aplanadas*
```csharp
public class UserDto
{
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string Email { get; set; }
    public string NombreRol { get; set; } // Flattened from Rol entity
    public string NombreSede { get; set; } // Flattened from Sede entity
}
```

*Backend: Seeding Automático (DbInitializer)*
```csharp
if (!context.Usuario.Any())
{
    var adminUser = new Usuario { NombreUsuario = "Admin", IdRol = 1, ... };
    context.Usuario.Add(adminUser);
    context.SaveChanges();
}
```

*Frontend: Servicio con Puerto Configurado*
```typescript
@Injectable({ providedIn: 'root' })
export class UserService {
    private apiUrl = 'http://localhost:5139/api/users'; // Puerto correcto
    // ... métodos CRUD
}
```

────────────────────────────────────────
6.8 Pruebas realizadas
────────────────────────────────────────
| Historia | Prueba realizada | Resultado esperado | Resultado obtenido |
| :--- | :--- | :--- | :--- |
| **HU-01** | Acceso a ruta `/configuration/users` | Visualizar tabla con datos | **Éxito**: Tabla renderizada con estilos Tailwind. |
| **HU-02** | Click en "Nuevo Usuario" y verificar dropdowns | Dropdowns "Rol" y "Sede" con datos | **Éxito**: Se listan roles y sedes de la BD. |
| **HU-04** | Click en icono "Basura" (Eliminar) | Confirmación y desaparición del registro | **Éxito**: Registro eliminado y tabla refrescada. |
| **General**| Verificación de estilos UI | Diseño coincidente con mockup | **Éxito**: Columnas y botones alineados. |

────────────────────────────────────────
6.9 Resultado de la épica
────────────────────────────────────────
Se ha completado la Épica de Configuración de Usuarios.
- **Logros**: Sistema ABM funcional, integración real Database-Backend-Frontend, estética profesional.
- **Aporte**: Base fundamental para la seguridad, ya que define *quién* puede operar el sistema.
- **Limitaciones**: La gestión de permisos es por Rol completo, no por funcionalidad específica (RBAC simple).
- **Dependencias**: Esta épica es prerrequisito para todas las demás (Inventario, Movimientos) que requieren un usuario logueado y asociado a una Sede.
