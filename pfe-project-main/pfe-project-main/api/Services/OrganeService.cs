using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public class OrganeService : IOrganeService
    {
        private readonly ApplicationDbContext _context;

        public OrganeService(ApplicationDbContext context)
        {
            _context = context;
        }
public async Task<IEnumerable<OrganeDTO>> GetAllAsync(
    string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true)
{
    var query = _context.Organes
        .Include(o => o.Marque)
        .Include(o => o.OrganeCaracteristiques)
            .ThenInclude(oc => oc.Caracteristique)
        .AsQueryable();

    // 🔍 Filtrage par mot-clé
    if (!string.IsNullOrEmpty(searchTerm))
    {
        string lowerSearch = searchTerm.ToLower();
        query = query.Where(o =>
            o.libelle_organe.ToLower().Contains(lowerSearch) ||
            o.code_organe.ToLower().Contains(lowerSearch) ||
            o.Marque.nom_fabriquant.ToLower().Contains(lowerSearch));
    }

    // 🔍 Filtrage par marque

    // ⬆⬇ Tri
    sortBy ??= "id_organe";
    query = sortBy.ToLower() switch
    {
        "libelle_organe" => ascending ? query.OrderBy(o => o.libelle_organe) : query.OrderByDescending(o => o.libelle_organe),
        "code_organe" => ascending ? query.OrderBy(o => o.code_organe) : query.OrderByDescending(o => o.code_organe),
        "nom_marque" => ascending ? query.OrderBy(o => o.Marque.nom_fabriquant) : query.OrderByDescending(o => o.Marque.nom_fabriquant),
        _ => ascending ? query.OrderBy(o => o.id_organe) : query.OrderByDescending(o => o.id_organe)
    };

    // 🟢 Projection vers DTO
    return await query.Select(o => new OrganeDTO
    {
        id_organe = o.id_organe,
        code_organe = o.code_organe,
        libelle_organe = o.libelle_organe,
        id_marque = o.id_marque,
        nom_marque = o.Marque.nom_fabriquant,
        caracteristiques = o.OrganeCaracteristiques
            .Select(oc => new CaracteristiqueDTO
            {
                id_caracteristique = oc.Caracteristique.id_caracteristique,
                libelle = oc.Caracteristique.libelle
            }).ToList()
    }).ToListAsync();
}

        public async Task<OrganeDTO?> GetByIdAsync(int id)
        {
            var o = await _context.Organes
                .Include(o => o.Marque)
                .Include(o => o.OrganeCaracteristiques)
                    .ThenInclude(oc => oc.Caracteristique)
                .FirstOrDefaultAsync(o => o.id_organe == id);

            if (o == null) return null;

            return new OrganeDTO
            {
                id_organe = o.id_organe,
                code_organe = o.code_organe,
                libelle_organe = o.libelle_organe,
                id_marque = o.id_marque,
                nom_marque = o.Marque.nom_fabriquant,
                caracteristiques = o.OrganeCaracteristiques
                    .Select(oc => new CaracteristiqueDTO
                    {
                        id_caracteristique = oc.Caracteristique.id_caracteristique,
                        libelle = oc.Caracteristique.libelle
                    }).ToList()
            };
        }

        public async Task<OrganeDTO> CreateAsync(CreateOrganeDTO dto)
        {
            var organe = new Organe
            {
                code_organe = dto.code_organe,
                libelle_organe = dto.libelle_organe,
                id_marque = dto.id_marque,
                OrganeCaracteristiques = dto.id_caracteristiques.Select(id => new OrganeCaracteristique
                {
                    id_caracteristique = id
                }).ToList()
            };

            _context.Organes.Add(organe);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(organe.id_organe) ?? throw new Exception("Création échouée");
        }

        public async Task<OrganeDTO?> UpdateAsync(int id, UpdateOrganeDTO dto)
        {
            var o = await _context.Organes
                .Include(o => o.OrganeCaracteristiques)
                .FirstOrDefaultAsync(o => o.id_organe == id);

            if (o == null) return null;

            o.libelle_organe = dto.libelle_organe;
            o.id_marque = dto.id_marque;

            // Suppression des anciennes relations
            _context.OrganeCaracteristiques.RemoveRange(o.OrganeCaracteristiques);

            // Ajout des nouvelles
            o.OrganeCaracteristiques = dto.id_caracteristiques.Select(id => new OrganeCaracteristique
            {
                id_caracteristique = id,
                id_organe = id // Attention, corriger ici
            }).ToList();

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task<bool> CanDeleteAsync(int id)
        {
            return !await _context.OrganeEquipements.AnyAsync(oe => oe.idorg == id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (!await CanDeleteAsync(id)) return false;

            var o = await _context.Organes
                .Include(o => o.OrganeCaracteristiques)
                .FirstOrDefaultAsync(o => o.id_organe == id);

            if (o == null) return false;

            _context.OrganeCaracteristiques.RemoveRange(o.OrganeCaracteristiques);
            _context.Organes.Remove(o);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
