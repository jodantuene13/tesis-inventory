namespace TesisInventory.Application.DTOs.Informes
{
    /// <summary>
    /// Punto de datos para el gráfico de evolución semanal de alertas.
    /// </summary>
    public class EvolucionSemanalDto
    {
        /// <summary>Etiqueta de la semana, ej: "2 Jun".</summary>
        public string Semana { get; set; } = string.Empty;

        /// <summary>Cantidad de alertas registradas en esa semana.</summary>
        public int Alertas { get; set; }
    }
}
