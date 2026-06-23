using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TesisInventory.Application.DTOs;
using TesisInventory.Application.DTOs.Atributos;
using TesisInventory.Application.Interfaces;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;

namespace TesisInventory.Application.Services
{
    public class AtributosService : IAtributosService
    {
        private readonly IAtributoRepository _atributoRepository;
        private readonly IFamiliaRepository _familiaRepository;
        private readonly IProductosService _productosService;

        public AtributosService(IAtributoRepository atributoRepository, IFamiliaRepository familiaRepository, IProductosService productosService)
        {
            _atributoRepository = atributoRepository;
            _familiaRepository = familiaRepository;
            _productosService = productosService;
        }

        public async Task<IEnumerable<AtributoDto>> GetAllAtributosAsync(bool includeInactive = false)
        {
            var attrs = await _atributoRepository.GetAllAtributosAsync(includeInactive);
            return attrs.Select(MapAtributoDto);
        }

        public async Task<AtributoDto?> GetAtributoByIdAsync(int id)
        {
            var a = await _atributoRepository.GetAtributoByIdAsync(id);
            return a == null ? null : MapAtributoDto(a);
        }

        public async Task<AtributoDto> CreateAtributoAsync(CreateAtributoDto createDto)
        {
            var existing = await _atributoRepository.GetAtributoByCodigoAsync(createDto.CodigoAtributo);
            if (existing != null)
            {
                if (!existing.Activo)
                {
                    existing.Nombre = createDto.Nombre;
                    existing.TipoDato = createDto.TipoDato;
                    existing.Descripcion = createDto.Descripcion;
                    existing.Activo = true;
                    existing.FechaActualizacion = DateTime.UtcNow;
                    await _atributoRepository.UpdateAtributoAsync(existing);
                    await _atributoRepository.SyncUnidadesMedidaAsync(existing.IdAtributo, createDto.IdsUnidadesMedida);
                    return await GetAtributoByIdAsync(existing.IdAtributo) ?? throw new Exception("Error al reactivar atributo.");
                }
                throw new InvalidOperationException("Ya existe un atributo con este código.");
            }

            var newAttr = new Atributo
            {
                CodigoAtributo = createDto.CodigoAtributo,
                Nombre = createDto.Nombre,
                TipoDato = createDto.TipoDato,
                Descripcion = createDto.Descripcion,
                Activo = createDto.Activo,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow
            };

            await _atributoRepository.AddAtributoAsync(newAttr);
            await _atributoRepository.SyncUnidadesMedidaAsync(newAttr.IdAtributo, createDto.IdsUnidadesMedida);
            return await GetAtributoByIdAsync(newAttr.IdAtributo) ?? throw new Exception("Error al obtener atributo creado");
        }

        public async Task<AtributoDto> UpdateAtributoAsync(int id, UpdateAtributoDto updateDto)
        {
            var attr = await _atributoRepository.GetAtributoByIdAsync(id);
            if (attr == null) throw new KeyNotFoundException("Atributo no encontrado.");

            if (attr.CodigoAtributo != updateDto.CodigoAtributo)
            {
                var existing = await _atributoRepository.GetAtributoByCodigoAsync(updateDto.CodigoAtributo);
                if (existing != null) throw new InvalidOperationException("Ya existe otro atributo con este código.");
            }

            attr.CodigoAtributo = updateDto.CodigoAtributo;
            attr.Nombre = updateDto.Nombre;
            attr.TipoDato = updateDto.TipoDato;
            attr.Descripcion = updateDto.Descripcion;
            attr.Activo = updateDto.Activo;
            attr.FechaActualizacion = DateTime.UtcNow;

            await _atributoRepository.UpdateAtributoAsync(attr);
            await _atributoRepository.SyncUnidadesMedidaAsync(id, updateDto.IdsUnidadesMedida);
            return await GetAtributoByIdAsync(attr.IdAtributo) ?? throw new Exception("Error al leer post update");
        }

        public async Task DeleteAtributoAsync(int id)
        {
            var attr = await _atributoRepository.GetAtributoByIdAsync(id);
            if (attr == null) throw new KeyNotFoundException("Atributo no encontrado.");

            await _atributoRepository.DeleteAtributoAsync(attr);
        }

        // ====== Opciones ======

        public async Task<IEnumerable<AtributoOpcionDto>> GetOpcionesAsync(int idAtributo)
        {
            var opciones = await _atributoRepository.GetOpcionesByAtributoIdAsync(idAtributo);
            return opciones.Select(o => new AtributoOpcionDto
            {
                IdAtributoOpcion = o.IdAtributoOpcion,
                IdAtributo = o.IdAtributo,
                CodigoOpcion = o.CodigoOpcion,
                Valor = o.Valor,
                Activo = o.Activo
            });
        }

        public async Task<AtributoOpcionDto> AddOpcionAsync(int idAtributo, CreateAtributoOpcionDto createDto)
        {
            var attr = await _atributoRepository.GetAtributoByIdAsync(idAtributo);
            if (attr == null || attr.TipoDato != "LIST")
                throw new InvalidOperationException("El atributo no existe o no es de tipo LIST.");

            var newOp = new AtributoOpcion
            {
                IdAtributo = idAtributo,
                CodigoOpcion = createDto.CodigoOpcion,
                Valor = createDto.Valor,
                Activo = createDto.Activo
            };

            await _atributoRepository.AddOpcionAsync(newOp);
            return new AtributoOpcionDto
            {
                IdAtributoOpcion = newOp.IdAtributoOpcion,
                IdAtributo = newOp.IdAtributo,
                CodigoOpcion = newOp.CodigoOpcion,
                Valor = newOp.Valor,
                Activo = newOp.Activo
            };
        }

        public async Task DeleteOpcionAsync(int idOpcion)
        {
            var op = await _atributoRepository.GetOpcionByIdAsync(idOpcion);
            if (op == null) throw new KeyNotFoundException("Opción no encontrada.");
            await _atributoRepository.DeleteOpcionAsync(op);
        }

        // ====== Familia Atributo ======

        public async Task<IEnumerable<FamiliaAtributoDto>> GetAtributosDeFamiliaAsync(int idFamilia)
        {
            var faList = await _atributoRepository.GetAtributosByFamiliaIdAsync(idFamilia);
            return faList.Select(fa => new FamiliaAtributoDto
            {
                IdFamiliaAtributo = fa.IdFamiliaAtributo,
                IdFamilia = fa.IdFamilia,
                NombreFamilia = fa.Familia?.Nombre ?? "",
                IdAtributo = fa.IdAtributo,
                NombreAtributo = fa.Atributo?.Nombre ?? "",
                TipoDatoAtributo = fa.Atributo?.TipoDato ?? "",
                Obligatorio = fa.Obligatorio,
                Activo = fa.Activo,
                UnidadesMedida = (fa.Atributo?.UnidadesMedida ?? Enumerable.Empty<Domain.Entities.AtributoUnidadMedida>())
                    .Where(au => au.UnidadMedida != null)
                    .Select(au => new UnidadMedidaDto
                    {
                        IdUnidadMedida = au.UnidadMedida!.IdUnidadMedida,
                        Simbolo = au.UnidadMedida.Simbolo,
                        Nombre = au.UnidadMedida.Nombre,
                        Activo = au.UnidadMedida.Activo
                    }).ToList()
            });
        }

        public async Task<FamiliaAtributoDto> AssignAtributoToFamiliaAsync(int idFamilia, CreateFamiliaAtributoDto req)
        {
            var familia = await _familiaRepository.GetByIdAsync(idFamilia);
            if (familia == null) throw new KeyNotFoundException("Familia no existe.");

            var attr = await _atributoRepository.GetAtributoByIdAsync(req.IdAtributo);
            if (attr == null || !attr.Activo) throw new KeyNotFoundException("Atributo no existe o inactivo.");

            var exists = await _atributoRepository.GetFamiliaAtributoAsync(idFamilia, req.IdAtributo);
            if (exists != null)
            {
                if (exists.Activo) throw new InvalidOperationException("El atributo ya está asignado a esta familia.");
                // Reactivarlo
                exists.Activo = true;
                exists.Obligatorio = req.Obligatorio;
                exists.FechaActualizacion = DateTime.UtcNow;
                await _atributoRepository.UpdateFamiliaAtributoAsync(exists);
                
                return new FamiliaAtributoDto
                {
                    IdFamiliaAtributo = exists.IdFamiliaAtributo,
                    IdFamilia = exists.IdFamilia,
                    NombreFamilia = familia.Nombre,
                    IdAtributo = exists.IdAtributo,
                    NombreAtributo = attr.Nombre,
                    TipoDatoAtributo = attr.TipoDato,
                    Obligatorio = exists.Obligatorio,
                    Activo = exists.Activo
                };
            }

            var newFa = new FamiliaAtributo
            {
                IdFamilia = idFamilia,
                IdAtributo = req.IdAtributo,
                Obligatorio = req.Obligatorio,
                Activo = req.Activo,
                FechaCreacion = DateTime.UtcNow,
                FechaActualizacion = DateTime.UtcNow
            };

            await _atributoRepository.AddFamiliaAtributoAsync(newFa);
            
            return new FamiliaAtributoDto
            {
                IdFamiliaAtributo = newFa.IdFamiliaAtributo,
                IdFamilia = newFa.IdFamilia,
                NombreFamilia = familia.Nombre,
                IdAtributo = newFa.IdAtributo,
                NombreAtributo = attr.Nombre,
                TipoDatoAtributo = attr.TipoDato,
                Obligatorio = newFa.Obligatorio,
                Activo = newFa.Activo
            };
        }

        public async Task RemoveAtributoFromFamiliaAsync(int idFamilia, int idAtributo)
        {
            var exists = await _atributoRepository.GetFamiliaAtributoAsync(idFamilia, idAtributo);
            if (exists == null) throw new KeyNotFoundException("Asignación no encontrada.");

            bool eraObligatorio = exists.Obligatorio;

            await _atributoRepository.DeleteFamiliaAtributoAsync(exists);

            if (eraObligatorio)
            {
                await _productosService.RegenerateSkusForFamiliaAsync(idFamilia);
            }
        }

        public async Task<IEnumerable<FamiliaAtributoDto>> GetFamiliasDeAtributoAsync(int idAtributo)
        {
            var faList = await _atributoRepository.GetFamiliasByAtributoIdAsync(idAtributo);
            return faList.Select(fa => new FamiliaAtributoDto
            {
                IdFamiliaAtributo = fa.IdFamiliaAtributo,
                IdFamilia = fa.IdFamilia,
                NombreFamilia = fa.Familia?.Nombre ?? "",
                IdAtributo = fa.IdAtributo,
                NombreAtributo = fa.Atributo?.Nombre ?? "",
                TipoDatoAtributo = fa.Atributo?.TipoDato ?? "",
                Obligatorio = fa.Obligatorio,
                Activo = fa.Activo
            });
        }

        private static AtributoDto MapAtributoDto(Domain.Entities.Atributo a) => new()
        {
            IdAtributo = a.IdAtributo,
            CodigoAtributo = a.CodigoAtributo,
            Nombre = a.Nombre,
            TipoDato = a.TipoDato,
            UnidadesMedida = a.UnidadesMedida
                .Where(au => au.UnidadMedida != null)
                .Select(au => new UnidadMedidaDto
                {
                    IdUnidadMedida = au.UnidadMedida!.IdUnidadMedida,
                    Simbolo = au.UnidadMedida.Simbolo,
                    Nombre = au.UnidadMedida.Nombre,
                    Activo = au.UnidadMedida.Activo
                }).ToList(),
            Descripcion = a.Descripcion,
            Activo = a.Activo,
            FechaCreacion = a.FechaCreacion,
            FechaActualizacion = a.FechaActualizacion
        };
    }
}
