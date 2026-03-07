using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Familias;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class FamiliasService : IFamiliasService
    {
        private readonly IFamiliaRepository _familiaRepository;
        private readonly IRubroRepository _rubroRepository;

        public FamiliasService(IFamiliaRepository familiaRepository, IRubroRepository rubroRepository)
        {
            _familiaRepository = familiaRepository;
            _rubroRepository = rubroRepository;
        }

        public async Task<IEnumerable<FamiliaDto>> GetAllFamiliasAsync(bool includeInactive = false)
        {
            var familias = await _familiaRepository.GetAllAsync(includeInactive);
            return familias.Select(f => new FamiliaDto
            {
                IdFamilia = f.IdFamilia,
                IdRubro = f.IdRubro,
                NombreRubro = f.Rubro?.Nombre ?? "N/A",
                CodigoFamilia = f.CodigoFamilia,
                Nombre = f.Nombre,
                Activo = f.Activo,
                FechaCreacion = f.FechaCreacion,
                FechaActualizacion = f.FechaActualizacion
            });
        }

        public async Task<IEnumerable<FamiliaDto>> GetFamiliasByRubroAsync(int idRubro, bool includeInactive = false)
        {
            var familias = await _familiaRepository.GetByRubroIdAsync(idRubro, includeInactive);
            return familias.Select(f => new FamiliaDto
            {
                IdFamilia = f.IdFamilia,
                IdRubro = f.IdRubro,
                NombreRubro = f.Rubro?.Nombre ?? "N/A",
                CodigoFamilia = f.CodigoFamilia,
                Nombre = f.Nombre,
                Activo = f.Activo,
                FechaCreacion = f.FechaCreacion,
                FechaActualizacion = f.FechaActualizacion
            });
        }

        public async Task<FamiliaDto?> GetFamiliaByIdAsync(int id)
        {
            var f = await _familiaRepository.GetByIdAsync(id);
            if (f == null) return null;

            return new FamiliaDto
            {
                IdFamilia = f.IdFamilia,
                IdRubro = f.IdRubro,
                NombreRubro = f.Rubro?.Nombre ?? "N/A",
                CodigoFamilia = f.CodigoFamilia,
                Nombre = f.Nombre,
                Activo = f.Activo,
                FechaCreacion = f.FechaCreacion,
                FechaActualizacion = f.FechaActualizacion
            };
        }

        public async Task<FamiliaDto> CreateFamiliaAsync(CreateFamiliaDto createFamiliaDto)
        {
            var rubro = await _rubroRepository.GetByIdAsync(createFamiliaDto.IdRubro);
            if (rubro == null)
                throw new InvalidOperationException("El rubro asignado no existe.");

            var existing = await _familiaRepository.GetByCodigoLocalAsync(createFamiliaDto.IdRubro, createFamiliaDto.CodigoFamilia);
            if (existing != null)
                throw new InvalidOperationException("Ya existe una familia con este código dentro de este rubro.");

            var nuevaFamilia = new Familia
            {
                IdRubro = createFamiliaDto.IdRubro,
                CodigoFamilia = createFamiliaDto.CodigoFamilia,
                Nombre = createFamiliaDto.Nombre,
                Activo = createFamiliaDto.Activo,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow
            };

            await _familiaRepository.AddAsync(nuevaFamilia);

            return new FamiliaDto
            {
                IdFamilia = nuevaFamilia.IdFamilia,
                IdRubro = nuevaFamilia.IdRubro,
                NombreRubro = rubro.Nombre, // Forzamos el nombre ya que no tenemos tracking activo post-add
                CodigoFamilia = nuevaFamilia.CodigoFamilia,
                Nombre = nuevaFamilia.Nombre,
                Activo = nuevaFamilia.Activo,
                FechaCreacion = nuevaFamilia.FechaCreacion,
                FechaActualizacion = nuevaFamilia.FechaActualizacion
            };
        }

        public async Task<FamiliaDto> UpdateFamiliaAsync(int id, UpdateFamiliaDto updateFamiliaDto)
        {
            var familia = await _familiaRepository.GetByIdAsync(id);
            if (familia == null)
                throw new KeyNotFoundException("Familia no encontrada.");

            if (familia.IdRubro != updateFamiliaDto.IdRubro)
            {
                var rubro = await _rubroRepository.GetByIdAsync(updateFamiliaDto.IdRubro);
                if (rubro == null)
                    throw new InvalidOperationException("El rubro asignado no existe.");
            }

            if (familia.CodigoFamilia != updateFamiliaDto.CodigoFamilia || familia.IdRubro != updateFamiliaDto.IdRubro)
            {
                var existing = await _familiaRepository.GetByCodigoLocalAsync(updateFamiliaDto.IdRubro, updateFamiliaDto.CodigoFamilia);
                if (existing != null)
                    throw new InvalidOperationException("Ya existe otra familia con este código dentro del rubro.");
            }

            familia.IdRubro = updateFamiliaDto.IdRubro;
            familia.CodigoFamilia = updateFamiliaDto.CodigoFamilia;
            familia.Nombre = updateFamiliaDto.Nombre;
            familia.Activo = updateFamiliaDto.Activo;
            familia.FechaActualizacion = DateTime.UtcNow;

            await _familiaRepository.UpdateAsync(familia);

            var rubroFinal = await _rubroRepository.GetByIdAsync(familia.IdRubro);

            return new FamiliaDto
            {
                IdFamilia = familia.IdFamilia,
                IdRubro = familia.IdRubro,
                NombreRubro = rubroFinal?.Nombre ?? "N/A",
                CodigoFamilia = familia.CodigoFamilia,
                Nombre = familia.Nombre,
                Activo = familia.Activo,
                FechaCreacion = familia.FechaCreacion,
                FechaActualizacion = familia.FechaActualizacion
            };
        }

        public async Task DeleteFamiliaAsync(int id)
        {
            var familia = await _familiaRepository.GetByIdAsync(id);
            if (familia == null)
                throw new KeyNotFoundException("Familia no encontrada.");

            await _familiaRepository.DeleteAsync(familia);
        }
    }
}
