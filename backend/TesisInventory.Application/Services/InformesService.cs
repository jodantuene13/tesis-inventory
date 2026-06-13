using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Informes;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class InformesService : IInformesService
    {
        private readonly IAlertaStockRepository _alertaRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ITransferenciaRepository _transferenciaRepository;

        public InformesService(
            IAlertaStockRepository alertaRepository,
            IStockRepository stockRepository,
            IMovimientoRepository movimientoRepository,
            ITransferenciaRepository transferenciaRepository)
        {
            _alertaRepository = alertaRepository;
            _stockRepository = stockRepository;
            _movimientoRepository = movimientoRepository;
            _transferenciaRepository = transferenciaRepository;
        }

        // ── Informe 1: Alertas de Stock ───────────────────────────────────────

        public async Task<InformeAlertasStockDto> GetAlertasStockAsync(
            int? idSede = null,
            int? idFamilia = null,
            int semanas = 5)
        {
            var alertasActivas = await _alertaRepository.GetAlertasActivasAsync(idSede, idFamilia);

            var bajoStock = alertasActivas.Select(a => new ProductoAlertaStockDto
            {
                IdAlertaStock = a.IdAlertaStock,
                IdProducto    = a.IdProducto,
                Producto      = a.Producto?.Nombre ?? string.Empty,
                Familia       = a.Producto?.Familia?.Nombre ?? string.Empty,
                Sede          = a.Sede?.NombreSede ?? string.Empty,
                StockActual   = a.StockAlMomento,
                StockMinimo   = a.PuntoReposicion,
                Diferencia    = a.StockAlMomento - a.PuntoReposicion,
                DiasEnAlerta  = (int)(DateTime.UtcNow - a.FechaCreacion).TotalDays,
                UltimaAlerta  = a.FechaUltimaAlerta,
                Criticidad    = CalcularCriticidad(a.StockAlMomento, a.PuntoReposicion)
            }).ToList();

            var fechaDesde = DateTime.UtcNow.AddDays(-(semanas * 7));
            var historial  = await _alertaRepository.GetHistorialAlertasAsync(idSede, idFamilia, fechaDesde, null);

            var recurrencia = historial
                .GroupBy(a => new { a.IdProducto, a.IdSede })
                .Select(g =>
                {
                    var ultimo = g.OrderByDescending(a => a.FechaUltimaAlerta).First();
                    int diasAcumulados = g.Sum(a =>
                    {
                        var fin = a.Estado == EstadoAlerta.Resuelta ? a.FechaUltimaAlerta : DateTime.UtcNow;
                        return Math.Max(0, (int)(fin - a.FechaCreacion).TotalDays);
                    });

                    return new ProductoRecurrenciaDto
                    {
                        IdProducto      = g.Key.IdProducto,
                        Producto        = ultimo.Producto?.Nombre ?? string.Empty,
                        Familia         = ultimo.Producto?.Familia?.Nombre ?? string.Empty,
                        Sede            = ultimo.Sede?.NombreSede ?? string.Empty,
                        CantidadAlertas = g.Count(),
                        DiasAcumulados  = diasAcumulados,
                        StockActual     = ultimo.StockAlMomento,
                        StockMinimo     = ultimo.PuntoReposicion,
                        UltimaAlerta    = ultimo.FechaUltimaAlerta,
                        EstadoActual    = ultimo.Estado == EstadoAlerta.Activa ? "Bajo stock" : "Resuelto",
                        Criticidad      = CalcularCriticidad(ultimo.StockAlMomento, ultimo.PuntoReposicion)
                    };
                })
                .OrderByDescending(r => r.CantidadAlertas)
                .ToList();

            var evolucionRaw = await _alertaRepository.GetEvolucionSemanalAsync(idSede, semanas);

            var evolucionSemanal = evolucionRaw.Select(e => new EvolucionSemanalDto
            {
                Semana  = e.InicioSemana.ToString("d MMM"),
                Alertas = e.CantidadAlertas
            }).ToList();

            return new InformeAlertasStockDto
            {
                BajoStock        = bajoStock,
                Recurrencia      = recurrencia,
                EvolucionSemanal = evolucionSemanal
            };
        }

        // ── Informe 2: Rotación de Productos ─────────────────────────────────

        public async Task<InformeRotacionDto> GetRotacionProductosAsync(
            int? idSede, int? idFamilia,
            DateTime fechaDesde, DateTime fechaHasta, int topN = 10)
        {
            var movimientos = (await _movimientoRepository.GetDatosRotacionAsync(
                idSede, idFamilia, fechaDesde, fechaHasta)).ToList();

            int totalDias = Math.Max(1, (int)(fechaHasta - fechaDesde).TotalDays);

            var grupos = movimientos
                .GroupBy(m => new { m.IdProducto, m.IdSede })
                .ToList();

            var rotacionList = new List<ProductoRotacionDto>();

            foreach (var grupo in grupos)
            {
                var movGrupo = grupo.OrderBy(m => m.Fecha).ToList();

                string nombreProducto = movGrupo.First().Producto?.Nombre ?? string.Empty;
                string sku            = movGrupo.First().Producto?.Sku ?? string.Empty;
                string familia        = movGrupo.First().Producto?.Familia?.Nombre ?? string.Empty;
                string sede           = movGrupo.First().Sede?.NombreSede ?? string.Empty;

                int totalIngresos = movGrupo
                    .Where(m => m.TipoMovimiento == TipoMovimiento.Ingreso)
                    .Sum(m => m.Cantidad);

                int totalEgresos = movGrupo
                    .Where(m => m.TipoMovimiento == TipoMovimiento.Egreso)
                    .Sum(m => m.Cantidad);

                double stockPonderado = CalcularStockPonderado(movGrupo, fechaDesde, fechaHasta, totalDias);

                double indice = stockPonderado > 0
                    ? Math.Round((double)totalEgresos / stockPonderado, 2)
                    : totalEgresos;

                var ultimoIngreso = movGrupo
                    .Where(m => m.TipoMovimiento == TipoMovimiento.Ingreso)
                    .OrderByDescending(m => m.Fecha)
                    .FirstOrDefault()?.Fecha;

                var ultimoEgreso = movGrupo
                    .Where(m => m.TipoMovimiento == TipoMovimiento.Egreso)
                    .OrderByDescending(m => m.Fecha)
                    .FirstOrDefault()?.Fecha;

                rotacionList.Add(new ProductoRotacionDto
                {
                    IdProducto             = grupo.Key.IdProducto,
                    Producto               = nombreProducto,
                    Sku                    = sku,
                    Familia                = familia,
                    Sede                   = sede,
                    TotalIngresos          = totalIngresos,
                    TotalEgresos           = totalEgresos,
                    StockPromedioPonderado = Math.Round(stockPonderado, 2),
                    IndiceRotacion         = indice,
                    UltimoIngreso          = ultimoIngreso,
                    UltimoEgreso           = ultimoEgreso,
                });
            }

            double rotacionPromedio = rotacionList.Any()
                ? rotacionList.Average(r => r.IndiceRotacion)
                : 0;

            foreach (var r in rotacionList)
            {
                r.Tendencia = r.IndiceRotacion >= rotacionPromedio * 1.2 ? "Alta" :
                              r.IndiceRotacion >= rotacionPromedio * 0.8 ? "Media" : "Baja";
            }

            rotacionList = rotacionList.OrderByDescending(r => r.IndiceRotacion).ToList();

            var ingresosRanking = movimientos
                .Where(m => m.TipoMovimiento == TipoMovimiento.Ingreso)
                .GroupBy(m => new { m.IdProducto, m.IdSede })
                .Select(g => new ProductoMovimientoDto
                {
                    IdProducto          = g.Key.IdProducto,
                    Producto            = g.First().Producto?.Nombre ?? string.Empty,
                    Sku                 = g.First().Producto?.Sku ?? string.Empty,
                    Familia             = g.First().Producto?.Familia?.Nombre ?? string.Empty,
                    Sede                = g.First().Sede?.NombreSede ?? string.Empty,
                    TotalUnidades       = g.Sum(m => m.Cantidad),
                    CantidadOperaciones = g.Count(),
                    UltimaFecha         = g.Max(m => m.Fecha)
                })
                .OrderByDescending(d => d.TotalUnidades)
                .ToList();

            var egresosRanking = movimientos
                .Where(m => m.TipoMovimiento == TipoMovimiento.Egreso)
                .GroupBy(m => new { m.IdProducto, m.IdSede })
                .Select(g => new ProductoMovimientoDto
                {
                    IdProducto          = g.Key.IdProducto,
                    Producto            = g.First().Producto?.Nombre ?? string.Empty,
                    Sku                 = g.First().Producto?.Sku ?? string.Empty,
                    Familia             = g.First().Producto?.Familia?.Nombre ?? string.Empty,
                    Sede                = g.First().Sede?.NombreSede ?? string.Empty,
                    TotalUnidades       = g.Sum(m => m.Cantidad),
                    CantidadOperaciones = g.Count(),
                    UltimaFecha         = g.Max(m => m.Fecha)
                })
                .OrderByDescending(d => d.TotalUnidades)
                .ToList();

            int saldoNeto = movimientos.Sum(m =>
                m.TipoMovimiento == TipoMovimiento.Ingreso ? m.Cantidad : -m.Cantidad);

            return new InformeRotacionDto
            {
                Rotacion         = rotacionList,
                MayorIngreso     = ingresosRanking,
                MayorEgreso      = egresosRanking,
                FechaDesde       = fechaDesde.ToString("yyyy-MM-dd"),
                FechaHasta       = fechaHasta.ToString("yyyy-MM-dd"),
                TotalDiasPeriodo = totalDias,
                RotacionPromedio = rotacionList.Any() ? Math.Round(rotacionPromedio, 2) : 0,
                SaldoNetoTotal   = saldoNeto
            };
        }

        // ── Informe 3: Transferencias y Préstamos ────────────────────────────

        public async Task<InformeTransferenciaDto> GetInformeTransferenciasAsync(
            int? idSedeOrigen, int? idSedeDestino, int? idFamilia,
            MotivoTransferencia? motivo, EstadoTransferencia? estado,
            DateTime fechaDesde, DateTime fechaHasta, int topN = 10)
        {
            var transferencias = (await _transferenciaRepository.GetDatosInformeAsync(
                idSedeOrigen, idSedeDestino, idFamilia, motivo, estado, fechaDesde, fechaHasta)).ToList();

            var informe = new InformeTransferenciaDto();

            // 1. KPIs
            int totalTf = transferencias.Count(t => t.Motivo == MotivoTransferencia.Definitiva);
            int totalPr = transferencias.Count(t => t.Motivo == MotivoTransferencia.Prestamo);
            int totalOp = transferencias.Count;

            int rechazadas = transferencias.Count(t => t.Estado == EstadoTransferencia.Rechazada);

            var prestamosConDevolucion = transferencias
                .Where(t => t.Motivo == MotivoTransferencia.Prestamo && t.FechaDevolucionReal.HasValue)
                .ToList();
            
            double tiempoPromedioDias = prestamosConDevolucion.Any()
                ? prestamosConDevolucion.Average(t => (t.FechaDevolucionReal!.Value - t.FechaSolicitud).TotalDays)
                : 0;

            var sedeOrigenGroup = transferencias
                .Where(t => t.SedeOrigen != null)
                .GroupBy(t => t.SedeOrigen!.NombreSede)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            informe.Kpis = new KpiTransferenciaDto
            {
                TotalTransferencias = totalTf,
                TotalPrestamos = totalPr,
                PorcentajePrestamos = totalOp > 0 ? Math.Round((double)totalPr / totalOp * 100, 1) : 0,
                TasaRechazo = totalOp > 0 ? Math.Round((double)rechazadas / totalOp * 100, 1) : 0,
                TiempoPromedioPrestamoDias = Math.Round(tiempoPromedioDias, 1),
                SedeMasActiva = sedeOrigenGroup?.Key ?? "N/A",
                SedeMasActivaCantidad = sedeOrigenGroup?.Count() ?? 0
            };

            // 2. Habituales por Sede (Top N)
            informe.HabitualesPorSede = transferencias
                .Where(t => t.SedeOrigen != null)
                .GroupBy(t => t.SedeOrigen!.NombreSede)
                .Select(g => new TransferenciaHabitualDto
                {
                    Nombre = g.Key,
                    Cantidad = g.Count(),
                    Porcentaje = totalOp > 0 ? Math.Round((double)g.Count() / totalOp * 100, 1) : 0
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(topN)
                .ToList();

            // 3. Habituales por Producto (Top N)
            // Flatten detalles to get product level count
            var productosFlat = transferencias.SelectMany(t => t.Detalles).ToList();
            int totalProds = productosFlat.Count;

            informe.HabitualesPorProducto = productosFlat
                .Where(d => d.Producto != null)
                .GroupBy(d => d.Producto!.IdProducto)
                .Select(g => new TransferenciaHabitualDto
                {
                    Nombre = g.First().Producto!.Nombre,
                    Sku = g.First().Producto!.Sku,
                    Familia = g.First().Producto!.Familia?.Nombre ?? string.Empty,
                    Cantidad = g.Count(), // Cantidad de veces que aparece en transferencias
                    Porcentaje = totalProds > 0 ? Math.Round((double)g.Count() / totalProds * 100, 1) : 0
                })
                .OrderByDescending(x => x.Cantidad)
                .Take(topN)
                .ToList();

            // 4. Préstamos Activos y Curva Diaria
            var todosLosPrestamos = transferencias.Where(t => t.Motivo == MotivoTransferencia.Prestamo).ToList();
            
            informe.PrestamosActivos = todosLosPrestamos
                .Where(t => t.Estado == EstadoTransferencia.Aprobada || 
                            t.Estado == EstadoTransferencia.EnTransito || 
                            t.Estado == EstadoTransferencia.Recibida || 
                            t.Estado == EstadoTransferencia.PendienteDevolucion)
                .Select(t => new PrestamoActivoDto
                {
                    IdTransferencia = t.IdTransferencia,
                    Productos = string.Join(", ", t.Detalles.Select(d => d.Producto?.Nombre)),
                    SedeOrigen = t.SedeOrigen?.NombreSede ?? "N/A",
                    SedeDestino = t.SedeDestino?.NombreSede ?? "N/A",
                    FechaSolicitud = t.FechaSolicitud,
                    FechaDevolucionEsperada = t.FechaDevolucionEsperada,
                    DiasTranscurridos = (int)(DateTime.UtcNow - t.FechaSolicitud).TotalDays,
                    Estado = t.Estado.ToString()
                })
                .OrderByDescending(p => p.DiasTranscurridos)
                .ToList();

            // Generar curva diaria iterando los días
            var fechaIterador = fechaDesde.Date;
            var fechaFinCurva = DateTime.UtcNow.Date < fechaHasta.Date ? DateTime.UtcNow.Date : fechaHasta.Date;
            
            while (fechaIterador <= fechaFinCurva)
            {
                int activosEseDia = todosLosPrestamos.Count(t => 
                    t.FechaSolicitud.Date <= fechaIterador && 
                    (!t.FechaDevolucionReal.HasValue || t.FechaDevolucionReal.Value.Date > fechaIterador));
                
                informe.PrestamosPorDia.Add(new PrestamoPorDiaDto
                {
                    Fecha = fechaIterador,
                    CantidadActivos = activosEseDia
                });

                fechaIterador = fechaIterador.AddDays(1);
            }

            // 5. Rechazos por Producto
            var rechazosGrupo = productosFlat
                .GroupBy(d => d.Producto!.IdProducto)
                .Select(g => 
                {
                    var transferenciasDelProd = g.Select(d => d.Transferencia).Where(t => t != null).Distinct().ToList();
                    int totalTfProd = transferenciasDelProd.Count;
                    int rechazadasProd = transferenciasDelProd.Count(t => t!.Estado == EstadoTransferencia.Rechazada);
                    
                    string motivo = "N/A";
                    if (rechazadasProd > 0)
                    {
                        // Buscar el motivo de rechazo en el historial
                        var motivos = transferenciasDelProd
                            .Where(t => t!.Estado == EstadoTransferencia.Rechazada)
                            .SelectMany(t => t!.HistorialTransferencias)
                            .Where(h => h.EstadoNuevo == EstadoTransferencia.Rechazada && !string.IsNullOrEmpty(h.Observaciones))
                            .Select(h => h.Observaciones)
                            .ToList();
                        
                        if (motivos.Any())
                        {
                            // Tomar la moda o el último
                            motivo = motivos.GroupBy(m => m).OrderByDescending(gm => gm.Count()).First().Key!;
                        }
                    }

                    return new ProductoRechazoDto
                    {
                        Producto = g.First().Producto!.Nombre,
                        Sku = g.First().Producto!.Sku,
                        Familia = g.First().Producto!.Familia?.Nombre ?? string.Empty,
                        Rechazadas = rechazadasProd,
                        Total = totalTfProd,
                        Indice = totalTfProd > 0 ? Math.Round((double)rechazadasProd / totalTfProd * 100, 1) : 0,
                        MotivoPrincipal = motivo
                    };
                })
                .Where(r => r.Rechazadas > 0)
                .OrderByDescending(r => r.Indice)
                .Take(topN)
                .ToList();

            informe.RechazosPorProducto = rechazosGrupo;

            // 6. Detalle (Tabla paginada en frontend)
            informe.DetalleMovimientos = transferencias.Select(t => new TransferenciaDetalleDto
            {
                IdTransferencia = t.IdTransferencia,
                FechaSolicitud = t.FechaSolicitud,
                Tipo = t.Motivo == MotivoTransferencia.Prestamo ? "Préstamo" : "Transferencia",
                Productos = string.Join(", ", t.Detalles.Select(d => d.Producto?.Nombre)),
                SedeOrigen = t.SedeOrigen?.NombreSede ?? "N/A",
                SedeDestino = t.SedeDestino?.NombreSede ?? "N/A",
                CantidadTotal = t.Detalles.Sum(d => d.Cantidad),
                Estado = t.Estado.ToString(),
                Usuario = t.UsuarioSolicita?.NombreUsuario ?? "N/A"
            }).ToList();

            return informe;
        }

        // ── Informe 4: Stock Inmovilizado ─────────────────────────────────────────

        public async Task<IEnumerable<ProductoInmovilizadoDto>> GetStockInmovilizadoAsync(
            int? idSede, int? idFamilia,
            DateTime fechaDesde, DateTime fechaHasta)
        {
            // Productos que tuvieron EGRESOS en el período (los excluimos)
            var movimientosPeriodo = (await _movimientoRepository.GetDatosRotacionAsync(
                idSede, idFamilia, fechaDesde, fechaHasta)).ToList();

            var conEgresoPeriodo = movimientosPeriodo
                .Where(m => m.TipoMovimiento == TipoMovimiento.Egreso)
                .Select(m => (m.IdProducto, m.IdSede))
                .ToHashSet();

            // Todos los stocks activos (CantidadActual > 0) para la sede/familia
            var todosLosStocks = (await _stockRepository.GetAllStocksAsync(idSede, idFamilia)).ToList();

            // Último ingreso histórico por (producto, sede) para mostrar como dato
            var todosLosIngresos = (await _movimientoRepository.GetIngresosAsync(idSede, idFamilia)).ToList();

            var ultimosIngresos = todosLosIngresos
                .GroupBy(m => (m.IdProducto, m.IdSede))
                .ToDictionary(g => g.Key, g => g.First().Fecha); // vienen ordenados desc

            var hoy = DateTime.UtcNow.Date;
            int diasSinEgreso = Math.Max(1, (int)(hoy - fechaDesde.Date).TotalDays);

            var resultado = todosLosStocks
                .Where(s => !conEgresoPeriodo.Contains((s.IdProducto, s.IdSede)))
                .Select(s =>
                {
                    ultimosIngresos.TryGetValue((s.IdProducto, s.IdSede), out var ultimoIngreso);
                    return new ProductoInmovilizadoDto
                    {
                        IdProducto    = s.IdProducto,
                        Producto      = s.Producto?.Nombre ?? string.Empty,
                        Sku           = s.Producto?.Sku ?? string.Empty,
                        Familia       = s.Producto?.Familia?.Nombre ?? string.Empty,
                        Sede          = s.Sede?.NombreSede ?? string.Empty,
                        StockActual   = s.CantidadActual,
                        UltimoIngreso = ultimoIngreso == default ? null : (DateTime?)ultimoIngreso,
                        DiasSinEgreso = diasSinEgreso
                    };
                })
                .OrderByDescending(p => p.StockActual)
                .ToList();

            return resultado;
        }

        // ── Informe 5: Familias que más se consumen ───────────────────────────────

        public async Task<IEnumerable<FamiliaConsumoDto>> GetFamiliasConsumoAsync(
            int? idSede, int? idFamilia,
            DateTime fechaDesde, DateTime fechaHasta)
        {
            var movimientos = (await _movimientoRepository.GetDatosRotacionAsync(
                idSede, idFamilia, fechaDesde, fechaHasta)).ToList();

            var resultado = movimientos
                .Where(m => !string.IsNullOrEmpty(m.Producto?.Familia?.Nombre))
                .GroupBy(m => m.Producto!.Familia!.Nombre)
                .Select(g =>
                {
                    int ingresos  = g.Where(m => m.TipoMovimiento == TipoMovimiento.Ingreso).Sum(m => m.Cantidad);
                    int egresos   = g.Where(m => m.TipoMovimiento == TipoMovimiento.Egreso).Sum(m => m.Cantidad);
                    int productos = g.Select(m => m.IdProducto).Distinct().Count();
                    double ratio  = ingresos > 0
                        ? Math.Round((double)egresos / ingresos, 2)
                        : (egresos > 0 ? 99.0 : 0.0);

                    return new FamiliaConsumoDto
                    {
                        Familia           = g.Key,
                        TotalIngresos     = ingresos,
                        TotalEgresos      = egresos,
                        RatioConsumo      = ratio,
                        CantidadProductos = productos
                    };
                })
                .OrderByDescending(f => f.RatioConsumo)
                .ToList();

            return resultado;
        }

        // ── Helpers ───────────────────────────────────────────────────────────────

        private static string CalcularCriticidad(int stockActual, int puntoReposicion)
        {
            if (stockActual == 0) return "Alta";
            if (puntoReposicion > 0 && stockActual <= puntoReposicion / 2) return "Media";
            return "Baja";
        }

        /// <summary>
        /// Reconstruye el stock promedio ponderado por tiempo usando CantidadRestante
        /// (stock después del movimiento) como punto de control de la curva de stock.
        /// Σ(Stockᵢ × Díasᵢ) / totalDías
        /// </summary>
        private static double CalcularStockPonderado(
            List<Domain.Entities.Movimiento> movimientos,
            DateTime fechaDesde,
            DateTime fechaHasta,
            int totalDias)
        {
            if (!movimientos.Any()) return 0;

            double acumulado = 0;
            DateTime puntoAnterior = fechaDesde;

            // Stock estimado antes del primer movimiento del período
            int stockAnterior = movimientos.First().CantidadRestante +
                                (movimientos.First().TipoMovimiento == TipoMovimiento.Egreso
                                    ? movimientos.First().Cantidad
                                    : -movimientos.First().Cantidad);

            foreach (var mov in movimientos)
            {
                double dias = (mov.Fecha - puntoAnterior).TotalDays;
                if (dias > 0)
                    acumulado += stockAnterior * dias;

                stockAnterior = mov.CantidadRestante;
                puntoAnterior = mov.Fecha;
            }

            // Tramo final hasta el cierre del período
            double diasFinales = (fechaHasta - puntoAnterior).TotalDays;
            if (diasFinales > 0)
                acumulado += stockAnterior * diasFinales;

            return totalDias > 0 ? acumulado / totalDias : 0;
        }
    }
}
