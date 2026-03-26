using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Stock;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository _stockRepository;
        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ITransferenciaRepository _transferenciaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly ISedeRepository _sedeRepository;

        public StockService(
            IStockRepository stockRepository,
            IMovimientoRepository movimientoRepository,
            ITransferenciaRepository transferenciaRepository,
            IProductoRepository productoRepository,
            ISedeRepository sedeRepository)
        {
            _stockRepository = stockRepository;
            _movimientoRepository = movimientoRepository;
            _transferenciaRepository = transferenciaRepository;
            _productoRepository = productoRepository;
            _sedeRepository = sedeRepository;
        }

        public async Task<(IEnumerable<StockDto> Items, int TotalCount)> GetStockAsync(
            int idSede,
            string? searchSkuOrName = null,
            int? idRubro = null,
            int? idFamilia = null,
            bool? estado = null,
            bool? bajoStock = null,
            int skip = 0,
            int take = 50)
        {
            var totalCount = await _stockRepository.GetTotalStockBySedeAsync(idSede, searchSkuOrName, idRubro, idFamilia, estado, bajoStock);
            var stocks = await _stockRepository.GetStockBySedeAsync(idSede, searchSkuOrName, idRubro, idFamilia, estado, bajoStock, skip, take);

            var items = stocks.Select(s => new StockDto
            {
                IdStock = s.IdStock,
                IdProducto = s.IdProducto,
                Sku = s.Producto!.Sku,
                NombreProducto = s.Producto.Nombre,
                UnidadMedida = s.Producto.UnidadMedida,
                RubroProducto = s.Producto.Familia?.Rubro?.Nombre ?? "",
                FamiliaProducto = s.Producto.Familia?.Nombre ?? "",
                EstadoProducto = s.Producto.Activo,
                IdSede = s.IdSede,
                CantidadActual = s.CantidadActual,
                PuntoReposicion = s.PuntoReposicion,
                FechaActualizacion = s.FechaActualizacion
            });

            return (items, totalCount);
        }

        public async Task<StockDto> IncrementarStockAsync(int idSede, int idUsuario, IncrementarStockDto dto)
        {
            var stock = await _stockRepository.GetStockAsync(dto.IdProducto, idSede);
            
            if (stock == null)
            {
                stock = new Stock
                {
                    IdProducto = dto.IdProducto,
                    IdSede = idSede,
                    CantidadActual = 0,
                    PuntoReposicion = 10,
                    FechaActualizacion = DateTime.UtcNow
                };
                await _stockRepository.AddStockAsync(stock);
            }

            stock.CantidadActual += dto.Cantidad;
            stock.FechaActualizacion = DateTime.UtcNow;

            await _stockRepository.UpdateStockAsync(stock);

            var movimiento = new Movimiento
            {
                IdProducto = dto.IdProducto,
                IdSede = idSede,
                TipoMovimiento = TipoMovimiento.Ingreso,
                Cantidad = dto.Cantidad,
                Motivo = dto.Motivo,
                IdUsuario = idUsuario,
                Observaciones = dto.Observaciones,
                Fecha = DateTime.UtcNow
            };

            await _movimientoRepository.AddMovimientoAsync(movimiento);

            return new StockDto
            {
                IdStock = stock.IdStock,
                IdProducto = stock.IdProducto,
                IdSede = stock.IdSede,
                CantidadActual = stock.CantidadActual,
                PuntoReposicion = stock.PuntoReposicion,
                FechaActualizacion = stock.FechaActualizacion
            };
        }

        public async Task<StockDto> RegistrarConsumoAsync(int idSede, int idUsuario, RegistrarConsumoDto dto)
        {
            var stock = await _stockRepository.GetStockAsync(dto.IdProducto, idSede);
            
            if (stock == null || stock.CantidadActual < dto.Cantidad)
            {
                throw new InvalidOperationException("No hay stock suficiente para realizar este consumo.");
            }

            stock.CantidadActual -= dto.Cantidad;
            stock.FechaActualizacion = DateTime.UtcNow;

            await _stockRepository.UpdateStockAsync(stock);

            var movimiento = new Movimiento
            {
                IdProducto = dto.IdProducto,
                IdSede = idSede,
                TipoMovimiento = TipoMovimiento.Egreso,
                Cantidad = dto.Cantidad,
                Motivo = dto.Motivo,
                IdUsuario = idUsuario,
                Observaciones = "Consumo registrado",
                Fecha = DateTime.UtcNow
            };

            await _movimientoRepository.AddMovimientoAsync(movimiento);

            return new StockDto
            {
                IdStock = stock.IdStock,
                IdProducto = stock.IdProducto,
                IdSede = stock.IdSede,
                CantidadActual = stock.CantidadActual,
                PuntoReposicion = stock.PuntoReposicion,
                FechaActualizacion = stock.FechaActualizacion
            };
        }

        public async Task<TransferenciaDto> RegistrarTransferenciaAsync(int idSedeOrigen, int idUsuario, RegistrarTransferenciaDto dto)
        {
            if (idSedeOrigen == dto.IdSedeDestino)
            {
                throw new InvalidOperationException("La sede destino no puede ser la misma que la sede origen.");
            }
            
            var stockOrigen = await _stockRepository.GetStockAsync(dto.IdProducto, idSedeOrigen);
            
            if (stockOrigen == null || stockOrigen.CantidadActual < dto.Cantidad)
            {
                throw new InvalidOperationException("No hay stock suficiente para realizar la transferencia.");
            }

            var transferencia = new Transferencia
            {
                IdProducto = dto.IdProducto,
                IdSedeOrigen = idSedeOrigen,
                IdSedeDestino = dto.IdSedeDestino,
                Cantidad = dto.Cantidad,
                FechaSolicitud = DateTime.UtcNow,
                Estado = EstadoTransferencia.Solicitada,
                IdUsuarioSolicita = idUsuario,
                Observaciones = dto.Observaciones
            };

            transferencia = await _transferenciaRepository.AddTransferenciaAsync(transferencia);

            var historial = new HistorialTransferencia
            {
                IdTransferencia = transferencia.IdTransferencia,
                EstadoAnterior = EstadoTransferencia.Solicitada,
                EstadoNuevo = EstadoTransferencia.Solicitada,
                Fecha = DateTime.UtcNow,
                IdUsuario = idUsuario,
                Observaciones = "Transferencia solicitada"
            };

            await _transferenciaRepository.AddHistorialTransferenciaAsync(historial);

            return new TransferenciaDto
            {
                IdTransferencia = transferencia.IdTransferencia,
                IdProducto = transferencia.IdProducto,
                IdSedeOrigen = transferencia.IdSedeOrigen,
                IdSedeDestino = transferencia.IdSedeDestino,
                Cantidad = transferencia.Cantidad,
                Estado = transferencia.Estado,
                FechaSolicitud = transferencia.FechaSolicitud,
                IdUsuarioSolicita = transferencia.IdUsuarioSolicita,
                Observaciones = transferencia.Observaciones
            };
        }

        public async Task<IEnumerable<MovimientoDto>> GetHistorialMovimientosAsync(int idProducto, int idSede, string? tipoMovimiento = null, string? fechaDesde = null, string? fechaHasta = null)
        {
            var movimientos = await _movimientoRepository.GetMovimientosAsync(idProducto, idSede, tipoMovimiento, fechaDesde, fechaHasta);
            
            return movimientos.Select(m => new MovimientoDto
            {
                IdMovimiento = m.IdMovimiento,
                IdProducto = m.IdProducto,
                IdSede = m.IdSede,
                TipoMovimiento = m.TipoMovimiento,
                Cantidad = m.Cantidad,
                Fecha = m.Fecha,
                Motivo = m.Motivo,
                IdUsuario = m.IdUsuario,
                NombreUsuario = m.Usuario?.NombreUsuario ?? "Usuario Desconocido",
                Observaciones = m.Observaciones
            });
        }

        public Task<IEnumerable<TransferenciaDto>> GetTransferenciasSedeAsync(int idSede)
        {
            throw new NotImplementedException();
        }
    }
}
