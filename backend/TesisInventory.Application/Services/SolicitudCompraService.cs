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
            var producto = await _productoRepository.GetProductoByIdAsync(dto.IdProducto);
            if (producto == null)
                throw new KeyNotFoundException("El producto especificado no existe.");

            var nuevaSolicitud = new SolicitudCompra
            {
                IdProducto = dto.IdProducto,
                IdSede = idSede,
                IdUsuarioSolicitante = idUsuario,
                Cantidad = dto.Cantidad,
                Estado = EstadoSolicitudCompra.Pendiente,
                FechaSolicitud = DateTime.UtcNow,
                Observaciones = dto.Observaciones
            };

            await _solicitudRepository.AddAsync(nuevaSolicitud);

            return MapToDto(nuevaSolicitud);
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
                IdProducto = s.IdProducto,
                NombreProducto = s.Producto?.Nombre ?? "N/A",
                SkuProducto = s.Producto?.Sku ?? "N/A",
                IdSede = s.IdSede,
                NombreSede = s.Sede?.NombreSede ?? "N/A",
                IdUsuarioSolicitante = s.IdUsuarioSolicitante,
                NombreSolicitante = s.UsuarioSolicitante?.NombreUsuario ?? "N/A",
                IdUsuarioAprobador = s.IdUsuarioAprobador,
                NombreAprobador = s.UsuarioAprobador?.NombreUsuario,
                Cantidad = s.Cantidad,
                Estado = s.Estado,
                FechaSolicitud = s.FechaSolicitud,
                FechaDecision = s.FechaDecision,
                Observaciones = s.Observaciones,
                MotivoRechazo = s.MotivoRechazo
            };
        }
    }
}
