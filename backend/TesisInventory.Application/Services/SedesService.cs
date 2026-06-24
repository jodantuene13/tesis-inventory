using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Sedes;
using TesisInventory.Application.Exceptions;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class SedesService : ISedesService
    {
        private readonly ISedeRepository _sedeRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IStockRepository _stockRepository;
        private readonly ITransferenciaRepository _transferenciaRepository;
        private readonly ISolicitudCompraRepository _solicitudCompraRepository;
        private readonly IMovimientoRepository _movimientoRepository;

        private static readonly EstadoTransferencia[] EstadosTransferenciaActivos =
        [
            EstadoTransferencia.Solicitada,
            EstadoTransferencia.Aprobada,
            EstadoTransferencia.EnTransito,
            EstadoTransferencia.PendienteDevolucion
        ];

        public SedesService(
            ISedeRepository sedeRepository,
            IProductoRepository productoRepository,
            IStockRepository stockRepository,
            ITransferenciaRepository transferenciaRepository,
            ISolicitudCompraRepository solicitudCompraRepository,
            IMovimientoRepository movimientoRepository)
        {
            _sedeRepository = sedeRepository;
            _productoRepository = productoRepository;
            _stockRepository = stockRepository;
            _transferenciaRepository = transferenciaRepository;
            _solicitudCompraRepository = solicitudCompraRepository;
            _movimientoRepository = movimientoRepository;
        }

        public async Task<IEnumerable<SedeDto>> GetAllSedesAsync()
        {
            var sedes = await _sedeRepository.GetAllAsync();
            return sedes
                .Where(s => s.Activo)
                .Select(s => new SedeDto
                {
                    IdSede = s.IdSede,
                    NombreSede = s.NombreSede,
                    Direccion = s.Direccion
                });
        }

        public async Task<SedeDto?> GetSedeByIdAsync(int id)
        {
            var sede = await _sedeRepository.GetByIdAsync(id);
            if (sede == null) return null;

            return new SedeDto
            {
                IdSede = sede.IdSede,
                NombreSede = sede.NombreSede,
                Direccion = sede.Direccion
            };
        }

        public async Task<SedeDto> CreateSedeAsync(CreateSedeDto createDto)
        {
            var sede = new Sede
            {
                NombreSede = createDto.NombreSede,
                Direccion = createDto.Direccion,
                Activo = true
            };

            var createdSede = await _sedeRepository.AddAsync(sede);

            var productosExistentes = await _productoRepository.GetAllProductosAsync(true);
            foreach (var producto in productosExistentes)
            {
                var stockInicial = new Stock
                {
                    IdProducto = producto.IdProducto,
                    IdSede = createdSede.IdSede,
                    CantidadActual = 0,
                    PuntoReposicion = 10,
                    FechaActualizacion = DateTime.UtcNow
                };
                await _stockRepository.AddStockAsync(stockInicial);
            }

            return new SedeDto
            {
                IdSede = createdSede.IdSede,
                NombreSede = createdSede.NombreSede,
                Direccion = createdSede.Direccion
            };
        }

        public async Task<SedeDto?> UpdateSedeAsync(int id, UpdateSedeDto updateDto)
        {
            var sede = await _sedeRepository.GetByIdAsync(id);
            if (sede == null) return null;

            sede.NombreSede = updateDto.NombreSede;
            sede.Direccion = updateDto.Direccion;

            await _sedeRepository.UpdateAsync(sede);

            return new SedeDto
            {
                IdSede = sede.IdSede,
                NombreSede = sede.NombreSede,
                Direccion = sede.Direccion
            };
        }

        public async Task<bool> DeleteSedeAsync(int id)
        {
            var sede = await _sedeRepository.GetByIdAsync(id);
            if (sede == null) return false;

            var bloqueantes = new List<SedeDeleteBloqueante>();

            // Transferencias activas que involucran esta sede
            var entrantes = await _transferenciaRepository.GetEntrantesAsync(id);
            var salientes = await _transferenciaRepository.GetSalientesAsync(id);
            var transferenciasPendientes = entrantes.Concat(salientes)
                .Where(t => EstadosTransferenciaActivos.Contains(t.Estado))
                .ToList();
            if (transferenciasPendientes.Any())
            {
                bloqueantes.Add(new SedeDeleteBloqueante
                {
                    Tipo = "transferencias",
                    Mensaje = $"Hay {transferenciasPendientes.Count} transferencia(s) en curso. Resuélvalas antes de continuar.",
                    Cantidad = transferenciasPendientes.Count
                });
            }

            // Solicitudes de compra pendientes o aprobadas
            var solicitudes = await _solicitudCompraRepository.GetAllAsync(idSede: id);
            var solicitudesPendientes = solicitudes
                .Where(s => s.Estado == EstadoSolicitudCompra.Pendiente || s.Estado == EstadoSolicitudCompra.Aprobada)
                .ToList();
            if (solicitudesPendientes.Any())
            {
                bloqueantes.Add(new SedeDeleteBloqueante
                {
                    Tipo = "solicitudesCompra",
                    Mensaje = $"Hay {solicitudesPendientes.Count} solicitud(es) de compra pendiente(s). Resuélvalas antes de continuar.",
                    Cantidad = solicitudesPendientes.Count
                });
            }

            // Productos con stock > 0
            var stockConCantidad = (await _stockRepository.GetAllStocksAsync(idSede: id)).ToList();
            if (stockConCantidad.Any())
            {
                bloqueantes.Add(new SedeDeleteBloqueante
                {
                    Tipo = "stock",
                    Mensaje = $"Hay {stockConCantidad.Count} producto(s) con stock. Transfiéralos a otra sede antes de continuar.",
                    Cantidad = stockConCantidad.Count,
                    Items = stockConCantidad.Select(s => new StockItemDto
                    {
                        NombreProducto = s.Producto?.Nombre ?? "Desconocido",
                        Sku = s.Producto?.Sku ?? "",
                        Cantidad = s.CantidadActual
                    }).ToList()
                });
            }

            if (bloqueantes.Any())
                throw new SedeDeleteBloqueadaException(bloqueantes);

            await _sedeRepository.SoftDeleteAsync(id);
            return true;
        }

        public async Task TransferirStockAsync(int sedeOrigenId, int sedeDestinoId, int idUsuario)
        {
            var sedeDestino = await _sedeRepository.GetByIdAsync(sedeDestinoId);
            if (sedeDestino == null || !sedeDestino.Activo)
                throw new InvalidOperationException("La sede de destino no existe o está inactiva.");

            if (sedeOrigenId == sedeDestinoId)
                throw new InvalidOperationException("La sede de origen y destino no pueden ser la misma.");

            var stockItems = (await _stockRepository.GetAllStocksAsync(idSede: sedeOrigenId)).ToList();

            foreach (var item in stockItems)
            {
                var cantidad = item.CantidadActual;
                var idProducto = item.IdProducto;

                // Sumar al stock de destino
                var stockDestino = await _stockRepository.GetStockAsync(idProducto, sedeDestinoId);
                int nuevaCantidadDestino;
                if (stockDestino != null)
                {
                    nuevaCantidadDestino = stockDestino.CantidadActual + cantidad;
                    stockDestino.CantidadActual = nuevaCantidadDestino;
                    stockDestino.FechaActualizacion = DateTime.UtcNow;
                    await _stockRepository.UpdateStockAsync(stockDestino);
                }
                else
                {
                    nuevaCantidadDestino = cantidad;
                    await _stockRepository.AddStockAsync(new Stock
                    {
                        IdProducto = idProducto,
                        IdSede = sedeDestinoId,
                        CantidadActual = nuevaCantidadDestino,
                        PuntoReposicion = item.PuntoReposicion,
                        FechaActualizacion = DateTime.UtcNow
                    });
                }

                // Egreso en sede origen
                await _movimientoRepository.AddMovimientoAsync(new Movimiento
                {
                    IdProducto = idProducto,
                    IdSede = sedeOrigenId,
                    TipoMovimiento = TipoMovimiento.Egreso,
                    Cantidad = cantidad,
                    CantidadRestante = 0,
                    Fecha = DateTime.UtcNow,
                    Motivo = MotivoMovimiento.Transferencia,
                    IdUsuario = idUsuario,
                    Observaciones = $"Cierre de sede: transferencia automática a sede ID {sedeDestinoId}"
                });

                // Ingreso en sede destino
                await _movimientoRepository.AddMovimientoAsync(new Movimiento
                {
                    IdProducto = idProducto,
                    IdSede = sedeDestinoId,
                    TipoMovimiento = TipoMovimiento.Ingreso,
                    Cantidad = cantidad,
                    CantidadRestante = nuevaCantidadDestino,
                    Fecha = DateTime.UtcNow,
                    Motivo = MotivoMovimiento.Transferencia,
                    IdUsuario = idUsuario,
                    Observaciones = $"Cierre de sede ID {sedeOrigenId}: recepción automática de stock"
                });

                // Poner a 0 el stock en origen
                var stockOrigen = await _stockRepository.GetStockAsync(idProducto, sedeOrigenId);
                if (stockOrigen != null)
                {
                    stockOrigen.CantidadActual = 0;
                    stockOrigen.FechaActualizacion = DateTime.UtcNow;
                    await _stockRepository.UpdateStockAsync(stockOrigen);
                }
            }
        }
    }
}
