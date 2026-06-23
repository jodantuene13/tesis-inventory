using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class UnidadMedidaService : IUnidadMedidaService
    {
        private readonly IUnidadMedidaRepository _repo;

        public UnidadMedidaService(IUnidadMedidaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<UnidadMedidaDto>> GetAllAsync(bool includeInactive = false)
        {
            var list = await _repo.GetAllAsync(includeInactive);
            return list.Select(Map);
        }

        public async Task<UnidadMedidaDto?> GetByIdAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            return u == null ? null : Map(u);
        }

        public async Task<UnidadMedidaDto> CreateAsync(CreateUnidadMedidaDto dto)
        {
            var existing = await _repo.GetBySimbolo(dto.Simbolo);
            if (existing != null)
            {
                if (!existing.Activo)
                {
                    existing.Nombre = dto.Nombre;
                    existing.Activo = true;
                    await _repo.UpdateAsync(existing);
                    return Map(existing);
                }
                throw new InvalidOperationException($"Ya existe una unidad con símbolo '{dto.Simbolo}'.");
            }

            var u = new UnidadMedida { Simbolo = dto.Simbolo, Nombre = dto.Nombre, Activo = dto.Activo };
            await _repo.AddAsync(u);
            return Map(u);
        }

        public async Task<UnidadMedidaDto> UpdateAsync(int id, UpdateUnidadMedidaDto dto)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) throw new KeyNotFoundException("Unidad de medida no encontrada.");

            if (u.Simbolo != dto.Simbolo)
            {
                var other = await _repo.GetBySimbolo(dto.Simbolo);
                if (other != null) throw new InvalidOperationException($"Ya existe otra unidad con símbolo '{dto.Simbolo}'.");
            }

            u.Simbolo = dto.Simbolo;
            u.Nombre = dto.Nombre;
            u.Activo = dto.Activo;
            await _repo.UpdateAsync(u);
            return Map(u);
        }

        public async Task DeleteAsync(int id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) throw new KeyNotFoundException("Unidad de medida no encontrada.");
            await _repo.DeleteAsync(u);
        }

        private static UnidadMedidaDto Map(UnidadMedida u) => new()
        {
            IdUnidadMedida = u.IdUnidadMedida,
            Simbolo = u.Simbolo,
            Nombre = u.Nombre,
            Activo = u.Activo
        };
    }
}
