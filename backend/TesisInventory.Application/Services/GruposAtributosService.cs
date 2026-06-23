using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs.Atributos;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class GruposAtributosService : IGruposAtributosService
    {
        private readonly IGrupoAtributoRepository _grupoRepo;
        private readonly IAtributoRepository _atributoRepo;
        private readonly IFamiliaRepository _familiaRepo;

        public GruposAtributosService(
            IGrupoAtributoRepository grupoRepo,
            IAtributoRepository atributoRepo,
            IFamiliaRepository familiaRepo)
        {
            _grupoRepo = grupoRepo;
            _atributoRepo = atributoRepo;
            _familiaRepo = familiaRepo;
        }

        public async Task<IEnumerable<GrupoAtributoDto>> GetAllGruposAsync(bool includeInactive = false)
        {
            var grupos = await _grupoRepo.GetAllAsync(includeInactive);
            return grupos.Select(MapToDto);
        }

        public async Task<GrupoAtributoDto?> GetGrupoByIdAsync(int id)
        {
            var g = await _grupoRepo.GetByIdAsync(id);
            return g == null ? null : MapToDto(g);
        }

        public async Task<GrupoAtributoDto> CreateGrupoAsync(CreateGrupoAtributoDto dto)
        {
            var existing = await _grupoRepo.GetByCodigoAsync(dto.CodigoGrupo);
            if (existing != null)
            {
                if (!existing.Activo)
                {
                    existing.Nombre = dto.Nombre;
                    existing.Separador = dto.Separador;
                    existing.UnidadSufijo = dto.UnidadSufijo;
                    existing.Activo = true;
                    existing.FechaActualizacion = DateTime.UtcNow;
                    await _grupoRepo.UpdateAsync(existing);
                    return MapToDto(await _grupoRepo.GetByIdAsync(existing.IdGrupoAtributo) ?? existing);
                }
                throw new InvalidOperationException("Ya existe un grupo con este código.");
            }

            var grupo = new GrupoAtributo
            {
                CodigoGrupo = dto.CodigoGrupo,
                Nombre = dto.Nombre,
                Separador = dto.Separador,
                UnidadSufijo = dto.UnidadSufijo,
                Activo = dto.Activo,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow
            };

            await _grupoRepo.AddAsync(grupo);
            return MapToDto(await _grupoRepo.GetByIdAsync(grupo.IdGrupoAtributo) ?? grupo);
        }

        public async Task<GrupoAtributoDto> UpdateGrupoAsync(int id, UpdateGrupoAtributoDto dto)
        {
            var grupo = await _grupoRepo.GetByIdAsync(id);
            if (grupo == null) throw new KeyNotFoundException("Grupo de atributos no encontrado.");

            if (grupo.CodigoGrupo != dto.CodigoGrupo)
            {
                var other = await _grupoRepo.GetByCodigoAsync(dto.CodigoGrupo);
                if (other != null) throw new InvalidOperationException("Ya existe otro grupo con este código.");
            }

            grupo.CodigoGrupo = dto.CodigoGrupo;
            grupo.Nombre = dto.Nombre;
            grupo.Separador = dto.Separador;
            grupo.UnidadSufijo = dto.UnidadSufijo;
            grupo.Activo = dto.Activo;
            grupo.FechaActualizacion = DateTime.UtcNow;

            await _grupoRepo.UpdateAsync(grupo);
            return MapToDto(await _grupoRepo.GetByIdAsync(grupo.IdGrupoAtributo) ?? grupo);
        }

        public async Task DeleteGrupoAsync(int id)
        {
            var grupo = await _grupoRepo.GetByIdAsync(id);
            if (grupo == null) throw new KeyNotFoundException("Grupo de atributos no encontrado.");
            await _grupoRepo.DeleteAsync(grupo);
        }

        public async Task<GrupoAtributoItemDto> AddItemAsync(int idGrupo, AddItemToGrupoDto dto)
        {
            var grupo = await _grupoRepo.GetByIdAsync(idGrupo);
            if (grupo == null || !grupo.Activo)
                throw new KeyNotFoundException("Grupo no encontrado o inactivo.");

            var atributo = await _atributoRepo.GetAtributoByIdAsync(dto.IdAtributo);
            if (atributo == null || !atributo.Activo)
                throw new KeyNotFoundException("Atributo no encontrado o inactivo.");

            if (atributo.TipoDato != "NUMBER" && atributo.TipoDato != "DECIMAL")
                throw new InvalidOperationException("Solo se pueden agregar atributos de tipo NUMBER o DECIMAL a un grupo.");

            var existing = await _grupoRepo.GetItemAsync(idGrupo, dto.IdAtributo);
            if (existing != null)
            {
                if (existing.Activo) throw new InvalidOperationException("El atributo ya está en este grupo.");
                existing.Activo = true;
                existing.Orden = dto.Orden;
                await _grupoRepo.UpdateItemAsync(existing);
                return MapItemToDto(existing, atributo);
            }

            var item = new GrupoAtributoItem
            {
                IdGrupoAtributo = idGrupo,
                IdAtributo = dto.IdAtributo,
                Orden = dto.Orden,
                IdUnidadMedida = dto.IdUnidadMedida,
                Activo = true
            };

            await _grupoRepo.AddItemAsync(item);
            item.Atributo = atributo;
            return MapItemToDto(item, atributo);
        }

        public async Task DeleteItemAsync(int idGrupo, int idAtributo)
        {
            var item = await _grupoRepo.GetItemAsync(idGrupo, idAtributo);
            if (item == null) throw new KeyNotFoundException("Item no encontrado en el grupo.");
            await _grupoRepo.DeleteItemAsync(item);
        }

        public async Task<IEnumerable<FamiliaGrupoAtributoDto>> GetGruposDeFamiliaAsync(int idFamilia)
        {
            var list = await _grupoRepo.GetGruposByFamiliaIdAsync(idFamilia);
            return list.Select(fg => MapFamiliaGrupoToDto(fg));
        }

        public async Task<FamiliaGrupoAtributoDto> AssignGrupoToFamiliaAsync(int idFamilia, CreateFamiliaGrupoAtributoDto dto)
        {
            var familia = await _familiaRepo.GetByIdAsync(idFamilia);
            if (familia == null) throw new KeyNotFoundException("Familia no existe.");

            var grupo = await _grupoRepo.GetByIdAsync(dto.IdGrupoAtributo);
            if (grupo == null || !grupo.Activo) throw new KeyNotFoundException("Grupo no existe o está inactivo.");

            var existing = await _grupoRepo.GetFamiliaGrupoAsync(idFamilia, dto.IdGrupoAtributo);
            if (existing != null)
            {
                if (existing.Activo) throw new InvalidOperationException("El grupo ya está asignado a esta familia.");
                existing.Activo = true;
                existing.Obligatorio = dto.Obligatorio;
                existing.FechaActualizacion = DateTime.UtcNow;
                await _grupoRepo.UpdateFamiliaGrupoAsync(existing);
                existing.GrupoAtributo = grupo;
                existing.Familia = familia;
                return MapFamiliaGrupoToDto(existing);
            }

            var fg = new FamiliaGrupoAtributo
            {
                IdFamilia = idFamilia,
                IdGrupoAtributo = dto.IdGrupoAtributo,
                Obligatorio = dto.Obligatorio,
                Activo = dto.Activo,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow
            };

            await _grupoRepo.AddFamiliaGrupoAsync(fg);
            fg.GrupoAtributo = grupo;
            fg.Familia = familia;
            return MapFamiliaGrupoToDto(fg);
        }

        public async Task RemoveGrupoFromFamiliaAsync(int idFamilia, int idGrupo)
        {
            var fg = await _grupoRepo.GetFamiliaGrupoAsync(idFamilia, idGrupo);
            if (fg == null) throw new KeyNotFoundException("Asignación no encontrada.");
            await _grupoRepo.DeleteFamiliaGrupoAsync(fg);
        }

        // --- Mappers ---

        private static GrupoAtributoDto MapToDto(GrupoAtributo g) => new()
        {
            IdGrupoAtributo = g.IdGrupoAtributo,
            CodigoGrupo = g.CodigoGrupo,
            Nombre = g.Nombre,
            Separador = g.Separador,
            UnidadSufijo = g.UnidadSufijo,
            Activo = g.Activo,
            FechaCreacion = g.FechaCreacion,
            FechaActualizacion = g.FechaActualizacion,
            Items = g.Items
                .Where(i => i.Activo)
                .OrderBy(i => i.Orden)
                .Select(i => MapItemToDto(i, i.Atributo))
                .ToList()
        };

        private static GrupoAtributoItemDto MapItemToDto(GrupoAtributoItem i, Atributo? atributo) => new()
        {
            IdGrupoAtributoItem = i.IdGrupoAtributoItem,
            IdGrupoAtributo = i.IdGrupoAtributo,
            IdAtributo = i.IdAtributo,
            NombreAtributo = atributo?.Nombre ?? "",
            TipoDatoAtributo = atributo?.TipoDato ?? "",
            IdUnidadMedida = i.IdUnidadMedida,
            SimboloUnidad = i.UnidadMedida?.Simbolo,
            Orden = i.Orden,
            Activo = i.Activo
        };

        private static FamiliaGrupoAtributoDto MapFamiliaGrupoToDto(FamiliaGrupoAtributo fg) => new()
        {
            IdFamiliaGrupoAtributo = fg.IdFamiliaGrupoAtributo,
            IdFamilia = fg.IdFamilia,
            NombreFamilia = fg.Familia?.Nombre ?? "",
            IdGrupoAtributo = fg.IdGrupoAtributo,
            NombreGrupo = fg.GrupoAtributo?.Nombre ?? "",
            Separador = fg.GrupoAtributo?.Separador ?? "*",
            UnidadSufijo = fg.GrupoAtributo?.UnidadSufijo,
            Obligatorio = fg.Obligatorio,
            Activo = fg.Activo,
            Items = fg.GrupoAtributo?.Items
                .Where(i => i.Activo)
                .OrderBy(i => i.Orden)
                .Select(i => MapItemToDto(i, i.Atributo))
                .ToList() ?? new()
        };
    }
}
