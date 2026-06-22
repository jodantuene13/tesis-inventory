using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TesisInventory.Domain.Entities;
using TesisInventory.Domain.Interfaces;
using TesisInventory.Infrastructure.Persistence;

namespace TesisInventory.Infrastructure.Repositories
{
    public class GrupoAtributoRepository : IGrupoAtributoRepository
    {
        private readonly InventoryDbContext _context;

        public GrupoAtributoRepository(InventoryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GrupoAtributo>> GetAllAsync(bool includeInactive = false)
        {
            var query = _context.GrupoAtributo
                .Include(g => g.Items.Where(i => i.Activo))
                    .ThenInclude(i => i.Atributo)
                .Include(g => g.Items.Where(i => i.Activo))
                    .ThenInclude(i => i.UnidadMedida)
                .AsQueryable();

            if (!includeInactive)
                query = query.Where(g => g.Activo);

            return await query.OrderBy(g => g.Nombre).ToListAsync();
        }

        public async Task<GrupoAtributo?> GetByIdAsync(int id)
        {
            return await _context.GrupoAtributo
                .Include(g => g.Items.Where(i => i.Activo))
                    .ThenInclude(i => i.Atributo)
                .Include(g => g.Items.Where(i => i.Activo))
                    .ThenInclude(i => i.UnidadMedida)
                .FirstOrDefaultAsync(g => g.IdGrupoAtributo == id);
        }

        public async Task<GrupoAtributo?> GetByCodigoAsync(string codigo)
        {
            return await _context.GrupoAtributo
                .FirstOrDefaultAsync(g => g.CodigoGrupo == codigo);
        }

        public async Task<GrupoAtributo> AddAsync(GrupoAtributo grupo)
        {
            await _context.GrupoAtributo.AddAsync(grupo);
            await _context.SaveChangesAsync();
            return grupo;
        }

        public async Task UpdateAsync(GrupoAtributo grupo)
        {
            _context.GrupoAtributo.Update(grupo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(GrupoAtributo grupo)
        {
            grupo.Activo = false;
            grupo.FechaActualizacion = DateTime.UtcNow;

            var items = await _context.GrupoAtributoItem
                .Where(i => i.IdGrupoAtributo == grupo.IdGrupoAtributo)
                .ToListAsync();
            foreach (var item in items) item.Activo = false;

            var asignaciones = await _context.FamiliaGrupoAtributo
                .Where(fg => fg.IdGrupoAtributo == grupo.IdGrupoAtributo)
                .ToListAsync();
            foreach (var fg in asignaciones) fg.Activo = false;

            _context.GrupoAtributo.Update(grupo);
            await _context.SaveChangesAsync();
        }

        // --- Items ---

        public async Task<IEnumerable<GrupoAtributoItem>> GetItemsByGrupoIdAsync(int idGrupo)
        {
            return await _context.GrupoAtributoItem
                .Include(i => i.Atributo)
                .Where(i => i.IdGrupoAtributo == idGrupo && i.Activo)
                .OrderBy(i => i.Orden)
                .ToListAsync();
        }

        public async Task<GrupoAtributoItem?> GetItemAsync(int idGrupo, int idAtributo)
        {
            return await _context.GrupoAtributoItem
                .FirstOrDefaultAsync(i => i.IdGrupoAtributo == idGrupo && i.IdAtributo == idAtributo);
        }

        public async Task<GrupoAtributoItem> AddItemAsync(GrupoAtributoItem item)
        {
            await _context.GrupoAtributoItem.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateItemAsync(GrupoAtributoItem item)
        {
            _context.GrupoAtributoItem.Update(item);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteItemAsync(GrupoAtributoItem item)
        {
            _context.GrupoAtributoItem.Remove(item);
            await _context.SaveChangesAsync();
        }

        // --- Familia-Grupo ---

        public async Task<IEnumerable<FamiliaGrupoAtributo>> GetGruposByFamiliaIdAsync(int idFamilia)
        {
            return await _context.FamiliaGrupoAtributo
                .Include(fg => fg.GrupoAtributo)
                    .ThenInclude(g => g!.Items.Where(i => i.Activo))
                        .ThenInclude(i => i.Atributo)
                .Include(fg => fg.GrupoAtributo)
                    .ThenInclude(g => g!.Items.Where(i => i.Activo))
                        .ThenInclude(i => i.UnidadMedida)
                .Where(fg => fg.IdFamilia == idFamilia && fg.Activo)
                .ToListAsync();
        }

        public async Task<FamiliaGrupoAtributo?> GetFamiliaGrupoAsync(int idFamilia, int idGrupo)
        {
            return await _context.FamiliaGrupoAtributo
                .FirstOrDefaultAsync(fg => fg.IdFamilia == idFamilia && fg.IdGrupoAtributo == idGrupo);
        }

        public async Task<FamiliaGrupoAtributo> AddFamiliaGrupoAsync(FamiliaGrupoAtributo familiaGrupo)
        {
            await _context.FamiliaGrupoAtributo.AddAsync(familiaGrupo);
            await _context.SaveChangesAsync();
            return familiaGrupo;
        }

        public async Task UpdateFamiliaGrupoAsync(FamiliaGrupoAtributo familiaGrupo)
        {
            _context.FamiliaGrupoAtributo.Update(familiaGrupo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFamiliaGrupoAsync(FamiliaGrupoAtributo familiaGrupo)
        {
            _context.FamiliaGrupoAtributo.Remove(familiaGrupo);
            await _context.SaveChangesAsync();
        }
    }
}
