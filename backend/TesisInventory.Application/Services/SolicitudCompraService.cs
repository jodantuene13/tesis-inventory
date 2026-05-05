using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.SolicitudesCompra;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Enums;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class SolicitudCompraService : ISolicitudCompraService
    {
        private readonly ISolicitudCompraRepository _solicitudRepository;
        private readonly IProductoRepository _productoRepository;

        public SolicitudCompraService(
            ISolicitudCompraRepository solicitudRepository,
            IProductoRepository productoRepository)
        {
            _solicitudRepository = solicitudRepository;
            _productoRepository = productoRepository;
        }

        public async Task<SolicitudCompraDto> CreateSolicitudAsync(int idSede, int idUsuario, CreateSolicitudCompraDto dto)
        {
            if (dto.Detalles == null || !dto.Detalles.Any())
                throw new ArgumentException("La solicitud debe contener al menos un producto.");

            var nuevaSolicitud = new SolicitudCompra
            {
                IdSede = idSede,
                IdUsuarioSolicitante = idUsuario,
                Estado = EstadoSolicitudCompra.Pendiente,
                FechaSolicitud = DateTime.UtcNow,
                MotivoSolicitud = dto.MotivoSolicitud,
                OrdenTrabajo = dto.OrdenTrabajo,
                TicketSolicitud = dto.TicketSolicitud,
                TareaARealizar = dto.TareaARealizar,
                Observaciones = dto.Observaciones
            };

            foreach (var det in dto.Detalles)
            {
                var producto = await _productoRepository.GetProductoByIdAsync(det.IdProducto);
                if (producto == null)
                    throw new KeyNotFoundException($"El producto especificado (ID: {det.IdProducto}) no existe.");

                nuevaSolicitud.Detalles.Add(new SolicitudCompraDetalle
                {
                    IdProducto = det.IdProducto,
                    Cantidad = det.Cantidad
                });
            }

            await _solicitudRepository.AddAsync(nuevaSolicitud);

            var created = await _solicitudRepository.GetByIdAsync(nuevaSolicitud.IdSolicitudCompra);
            return MapToDto(created!);
        }

        public async Task<SolicitudCompraDto> UpdateEstadoAsync(int idSolicitud, int idUsuarioAprobador, UpdateSolicitudCompraEstadoDto dto)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(idSolicitud);
            if (solicitud == null)
                throw new KeyNotFoundException("La solicitud no existe.");

            if (solicitud.Estado != EstadoSolicitudCompra.Pendiente)
                throw new InvalidOperationException("Solo se pueden procesar solicitudes en estado Pendiente.");

            solicitud.Estado = dto.NuevoEstado;
            solicitud.IdUsuarioAprobador = idUsuarioAprobador;
            solicitud.FechaDecision = DateTime.UtcNow;
            solicitud.MotivoRechazo = dto.NuevoEstado == EstadoSolicitudCompra.Rechazada ? dto.MotivoRechazo : null;

            await _solicitudRepository.UpdateAsync(solicitud);

            // Refetch to get updated relations (like Aprobador name)
            var updated = await _solicitudRepository.GetByIdAsync(idSolicitud);
            return MapToDto(updated!);
        }

        public async Task<(IEnumerable<SolicitudCompraDto> Items, int TotalCount)> GetPagedSolicitudesAsync(
            int? idSede, string? search, int? estado, int page, int pageSize)
        {
            int skip = (page - 1) * pageSize;
            var (items, totalCount) = await _solicitudRepository.GetPagedAsync(idSede, search, estado, skip, pageSize);

            return (items.Select(MapToDto), totalCount);
        }

        public async Task<SolicitudCompraDto?> GetByIdAsync(int id)
        {
            var s = await _solicitudRepository.GetByIdAsync(id);
            return s != null ? MapToDto(s) : null;
        }

        private SolicitudCompraDto MapToDto(SolicitudCompra s)
        {
            return new SolicitudCompraDto
            {
                IdSolicitudCompra = s.IdSolicitudCompra,
                IdSede = s.IdSede,
                NombreSede = s.Sede?.NombreSede ?? "N/A",
                IdUsuarioSolicitante = s.IdUsuarioSolicitante,
                NombreSolicitante = s.UsuarioSolicitante?.NombreUsuario ?? "N/A",
                IdUsuarioAprobador = s.IdUsuarioAprobador,
                NombreAprobador = s.UsuarioAprobador?.NombreUsuario,
                
                MotivoSolicitud = s.MotivoSolicitud,
                OrdenTrabajo = s.OrdenTrabajo,
                TicketSolicitud = s.TicketSolicitud,
                TareaARealizar = s.TareaARealizar,

                Estado = s.Estado,
                FechaSolicitud = s.FechaSolicitud,
                FechaDecision = s.FechaDecision,
                Observaciones = s.Observaciones,
                MotivoRechazo = s.MotivoRechazo,
                
                Detalles = s.Detalles.Select(d => new SolicitudCompraDetalleDto
                {
                    IdProducto = d.IdProducto,
                    NombreProducto = d.Producto?.Nombre ?? "N/A",
                    SkuProducto = d.Producto?.Sku ?? "N/A",
                    Cantidad = d.Cantidad
                }).ToList()
            };
        }
    }
}
