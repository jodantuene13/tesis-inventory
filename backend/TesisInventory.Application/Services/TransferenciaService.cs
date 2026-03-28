using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Transferencias;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class TransferenciaService : ITransferenciaService
    {
        private readonly ITransferenciaRepository _transferenciaRepo;
        private readonly IStockRepository _stockRepo;
        private readonly IMovimientoRepository _movimientoRepo;

        public TransferenciaService(
            ITransferenciaRepository transferenciaRepo,
            IStockRepository stockRepo,
            IMovimientoRepository movimientoRepo)
        {
            _transferenciaRepo = transferenciaRepo;
            _stockRepo = stockRepo;
            _movimientoRepo = movimientoRepo;
        }

        public async Task<IEnumerable<TransferenciaDto>> GetEntrantesAsync(int idSede)
        {
            var transferencias = await _transferenciaRepo.GetEntrantesAsync(idSede);
            return transferencias.Select(MapToDto);
        }

        public async Task<IEnumerable<TransferenciaDto>> GetSalientesAsync(int idSede)
        {
            var transferencias = await _transferenciaRepo.GetSalientesAsync(idSede);
            return transferencias.Select(MapToDto);
        }

        public async Task<TransferenciaDto> CreateAsync(CreateTransferenciaDto createDto, int idUsuarioSolicita)
        {
            var stockOrigen = await _stockRepo.GetStockAsync(createDto.IdProducto, createDto.IdSedeOrigen);
            var stockOrigenActual = stockOrigen?.CantidadActual ?? 0;

            var transferencia = new Transferencia
            {
                IdProducto = createDto.IdProducto,
                IdSedeOrigen = createDto.IdSedeOrigen,
                IdSedeDestino = createDto.IdSedeDestino,
                StockOrigenSnapshot = stockOrigenActual,
                Cantidad = createDto.Cantidad,
                Motivo = createDto.Motivo,
                Observaciones = createDto.Observaciones,
                IdUsuarioSolicita = idUsuarioSolicita,
                FechaSolicitud = DateTime.UtcNow,
                Estado = EstadoTransferencia.Solicitada
            };

            await _transferenciaRepo.AddTransferenciaAsync(transferencia);
            await AddHistorialAsync(transferencia, null, EstadoTransferencia.Solicitada, idUsuarioSolicita, "Solicitud generada");
            
            var savedTransfer = await _transferenciaRepo.GetTransferenciaByIdAsync(transferencia.IdTransferencia);
            return MapToDto(savedTransfer!);
        }

        public async Task AceptarTransferenciaAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null)
        {
            var tx = await _transferenciaRepo.GetTransferenciaByIdAsync(idTransferencia) ?? throw new Exception("Transferencia no encontrada");

            if (tx.Estado != EstadoTransferencia.Solicitada)
                throw new Exception("Solo se pueden aceptar transferencias en estado Solicitada");

            var estadoAnterior = tx.Estado;
            tx.Estado = EstadoTransferencia.EnTransito;

            await _transferenciaRepo.UpdateTransferenciaAsync(tx);
            await AddHistorialAsync(tx, estadoAnterior, tx.Estado, idUsuarioAutenticado, observaciones ?? "Transferencia aceptada y en tránsito");
        }

        public async Task RechazarTransferenciaAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null)
        {
            var tx = await _transferenciaRepo.GetTransferenciaByIdAsync(idTransferencia) ?? throw new Exception("Transferencia no encontrada");

            if (tx.Estado != EstadoTransferencia.Solicitada)
                throw new Exception("Solo se pueden rechazar transferencias en estado Solicitada");

            var estadoAnterior = tx.Estado;
            tx.Estado = EstadoTransferencia.Rechazada;

            await _transferenciaRepo.UpdateTransferenciaAsync(tx);
            await AddHistorialAsync(tx, estadoAnterior, tx.Estado, idUsuarioAutenticado, observaciones ?? "Transferencia rechazada");
        }

        public async Task ConfirmarRecepcionAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null)
        {
            var tx = await _transferenciaRepo.GetTransferenciaByIdAsync(idTransferencia) ?? throw new Exception("Transferencia no encontrada");

            if (tx.Estado != EstadoTransferencia.EnTransito)
                throw new Exception("Solo se puede confirmar recepción de transferencias en tránsito");

            var estadoAnterior = tx.Estado;
            
            await HandleStockMovement(tx, isReverse: false, idUsuarioAutenticado);

            if (tx.Motivo == MotivoTransferencia.Prestamo)
            {
                tx.Estado = EstadoTransferencia.PendienteDevolucion;
            }
            else
            {
                tx.Estado = EstadoTransferencia.Recibida;
            }

            await _transferenciaRepo.UpdateTransferenciaAsync(tx);
            await AddHistorialAsync(tx, estadoAnterior, tx.Estado, idUsuarioAutenticado, observaciones ?? "Recepción confirmada");
        }

        public async Task DevolverPrestamoAsync(int idTransferencia, int idUsuarioAutenticado, string? observaciones = null)
        {
            var tx = await _transferenciaRepo.GetTransferenciaByIdAsync(idTransferencia) ?? throw new Exception("Transferencia no encontrada");

            if (tx.Estado != EstadoTransferencia.PendienteDevolucion)
                throw new Exception("Solo se puede devolver transferencias en estado Pendiente de Devolucion");

            await HandleStockMovement(tx, isReverse: true, idUsuarioAutenticado);

            var estadoAnterior = tx.Estado;
            tx.Estado = EstadoTransferencia.Devuelta;

            await _transferenciaRepo.UpdateTransferenciaAsync(tx);
            await AddHistorialAsync(tx, estadoAnterior, tx.Estado, idUsuarioAutenticado, observaciones ?? "Préstamo devuelto");
        }

        private async Task HandleStockMovement(Transferencia tx, bool isReverse, int userId)
        {
            var originSede = !isReverse ? tx.IdSedeOrigen : tx.IdSedeDestino;
            var destSede = !isReverse ? tx.IdSedeDestino : tx.IdSedeOrigen;

            var stockOrigen = await _stockRepo.GetStockAsync(tx.IdProducto, originSede);
            if (stockOrigen == null || stockOrigen.CantidadActual < tx.Cantidad)
            {
                throw new Exception("Stock insuficiente en la sede origen para confirmar la transferencia o devolución.");
            }

            stockOrigen.CantidadActual -= tx.Cantidad;
            await _stockRepo.UpdateStockAsync(stockOrigen);

            var stockDestino = await _stockRepo.GetStockAsync(tx.IdProducto, destSede);
            if (stockDestino == null)
            {
                stockDestino = new Stock
                {
                    IdProducto = tx.IdProducto,
                    IdSede = destSede,
                    CantidadActual = tx.Cantidad
                };
                await _stockRepo.AddStockAsync(stockDestino);
            }
            else
            {
                stockDestino.CantidadActual += tx.Cantidad;
                await _stockRepo.UpdateStockAsync(stockDestino);
            }

            await _movimientoRepo.AddMovimientoAsync(new Movimiento
            {
                IdProducto = tx.IdProducto,
                IdSede = originSede,
                TipoMovimiento = TipoMovimiento.Egreso,
                Cantidad = tx.Cantidad,
                Motivo = MotivoMovimiento.Transferencia,
                IdUsuario = userId
            });

            await _movimientoRepo.AddMovimientoAsync(new Movimiento
            {
                IdProducto = tx.IdProducto,
                IdSede = destSede,
                TipoMovimiento = TipoMovimiento.Ingreso,
                Cantidad = tx.Cantidad,
                Motivo = MotivoMovimiento.Transferencia,
                IdUsuario = userId
            });
        }

        private async Task AddHistorialAsync(Transferencia tx, EstadoTransferencia? estadoAnterior, EstadoTransferencia estadoNuevo, int idUsuario, string observaciones)
        {
            await _transferenciaRepo.AddHistorialTransferenciaAsync(new HistorialTransferencia
            {
                IdTransferencia = tx.IdTransferencia,
                EstadoAnterior = estadoAnterior ?? EstadoTransferencia.Solicitada,
                EstadoNuevo = estadoNuevo,
                IdUsuario = idUsuario,
                Observaciones = observaciones
            });
        }

        private static TransferenciaDto MapToDto(Transferencia t)
        {
            return new TransferenciaDto
            {
                IdTransferencia = t.IdTransferencia,
                IdProducto = t.IdProducto,
                NombreProducto = t.Producto?.Nombre ?? "Desconocido",
                IdSedeOrigen = t.IdSedeOrigen,
                NombreSedeOrigen = t.SedeOrigen?.NombreSede ?? "Desconocida",
                IdSedeDestino = t.IdSedeDestino,
                NombreSedeDestino = t.SedeDestino?.NombreSede ?? "Desconocida",
                Cantidad = t.Cantidad,
                StockOrigenSnapshot = t.StockOrigenSnapshot,
                FechaSolicitud = t.FechaSolicitud,
                Estado = t.Estado,
                Motivo = t.Motivo,
                IdUsuarioSolicita = t.IdUsuarioSolicita,
                NombreUsuarioSolicita = t.UsuarioSolicita?.NombreUsuario ?? "Desconocido",
                Observaciones = t.Observaciones
            };
        }
    }
}
