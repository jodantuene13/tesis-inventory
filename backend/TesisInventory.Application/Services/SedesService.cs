using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Sedes;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class SedesService : ISedesService
    {
        private readonly ISedeRepository _sedeRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IStockRepository _stockRepository;

        public SedesService(
            ISedeRepository sedeRepository, 
            IUserRepository userRepository,
            IProductoRepository productoRepository,
            IStockRepository stockRepository)
        {
            _sedeRepository = sedeRepository;
            _userRepository = userRepository;
            _productoRepository = productoRepository;
            _stockRepository = stockRepository;
        }

        public async Task<IEnumerable<SedeDto>> GetAllSedesAsync()
        {
            var sedes = await _sedeRepository.GetAllAsync();
            return sedes.Select(s => new SedeDto
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
                Direccion = createDto.Direccion
            };

            var createdSede = await _sedeRepository.AddAsync(sede);

            // Inicializar Stock en 0 para todos los productos existentes en esta nueva Sede
            var productosExistentes = await _productoRepository.GetAllProductosAsync(true);
            foreach (var producto in productosExistentes)
            {
                var stockInicial = new Stock
                {
                    IdProducto = producto.IdProducto,
                    IdSede = createdSede.IdSede,
                    CantidadActual = 0,
                    PuntoReposicion = 10, // Valor por defecto o configurable
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

            // Validación DIC: No permitir eliminar si hay usuarios asignados.
            var usuariosAsignados = await _userRepository.GetAllAsync();
            var tieneUsuarios = usuariosAsignados.Any(u => u.IdSede == id);

            if (tieneUsuarios)
            {
                throw new InvalidOperationException("No se puede eliminar la Sede porque tiene usuarios asignados. Debe reasignar los usuarios antes de eliminarla.");
            }

            await _sedeRepository.DeleteAsync(sede);
            return true;
        }
    }
}
