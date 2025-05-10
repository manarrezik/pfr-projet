using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;

public class GroupeIdentiqueService : IGroupeIdentiqueService
{
    private readonly ApplicationDbContext _context;

    public GroupeIdentiqueService(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<GroupeIdentiqueDTO>> GetAllAsync(
    string? searchTerm = null,
    string? sortBy = null,
    bool ascending = true)
{
    var query = _context.GroupeIdentiques
        .Include(g => g.Marque)
        .Include(g => g.TypeEquip)
        .Include(g => g.GroupeOrganes).ThenInclude(go => go.Organe)
        .Include(g => g.GroupeCaracteristiques).ThenInclude(gc => gc.Caracteristique)
        .AsQueryable();

    if (!string.IsNullOrEmpty(searchTerm))
    {
        string lowerSearch = searchTerm.ToLower();
        query = query.Where(g =>
            g.designation.ToLower().Contains(lowerSearch) ||
            g.Marque.nom_fabriquant.ToLower().Contains(lowerSearch) ||
            g.TypeEquip.designation.ToLower().Contains(lowerSearch));
    }


    sortBy = sortBy?.ToLower() ?? "id";

    query = sortBy switch
    {
        "designation" => ascending ? query.OrderBy(g => g.designation) : query.OrderByDescending(g => g.designation),
        "marque" => ascending ? query.OrderBy(g => g.Marque.nom_fabriquant) : query.OrderByDescending(g => g.Marque.nom_fabriquant),
        "type" or "typeequip" => ascending ? query.OrderBy(g => g.TypeEquip.designation) : query.OrderByDescending(g => g.TypeEquip.designation),
        _ => ascending ? query.OrderBy(g => g.Id) : query.OrderByDescending(g => g.Id)
    };

    return await query.Select(g => new GroupeIdentiqueDTO
    {
        Id = g.Id,
        Designation = g.designation,
        MarqueNom = g.Marque.nom_fabriquant,
        TypeEquipNom = g.TypeEquip.designation,
        Organes = g.GroupeOrganes.Select(o => o.Organe.libelle_organe).ToList(),
        Caracteristiques = g.GroupeCaracteristiques.Select(c => c.Caracteristique.libelle).ToList()
    }).ToListAsync();
}

    public async Task<GroupeIdentiqueDTO?> GetByIdAsync(int id)
    {
        var g = await _context.GroupeIdentiques
            .Include(g => g.Marque)
            .Include(g => g.TypeEquip)
            .Include(g => g.GroupeOrganes).ThenInclude(go => go.Organe)
            .Include(g => g.GroupeCaracteristiques).ThenInclude(gc => gc.Caracteristique)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (g == null) return null;

        return new GroupeIdentiqueDTO
        {
            Id = g.Id,
            Designation = g.designation,
            MarqueNom = g.Marque.nom_fabriquant,
            TypeEquipNom = g.TypeEquip.designation,
            Organes = g.GroupeOrganes.Select(o => o.Organe.libelle_organe).ToList(),
            Caracteristiques = g.GroupeCaracteristiques.Select(c => c.Caracteristique.libelle).ToList()
        };
    }

    public async Task<GroupeIdentiqueDTO?> UpdateAsync(int id, UpdateGroupeIdentiqueDTO dto)
    {
        var groupe = await _context.GroupeIdentiques
            .Include(g => g.GroupeOrganes)
            .Include(g => g.GroupeCaracteristiques)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (groupe == null) return null;

        groupe.designation = dto.Designation;

        _context.GroupeOrganes.RemoveRange(groupe.GroupeOrganes);
        _context.GroupeCaracteristiques.RemoveRange(groupe.GroupeCaracteristiques);

        groupe.GroupeOrganes = dto.id_organes.Select(orgId => new GroupeOrgane
        {
            idgrpidq = id,
            idorg = orgId
        }).ToList();

        groupe.GroupeCaracteristiques = dto.id_caracteristiques.Select(caracId => new GroupeCaracteristique
        {
            idgrpidq = id,
            idcarac = caracId
        }).ToList();

        await _context.SaveChangesAsync();

        return await GetByIdAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        bool isUsed = await _context.Equipements.AnyAsync(e => e.idGrpIdq == id);
        if (isUsed) return false;

        var group = await _context.GroupeIdentiques.FindAsync(id);
        if (group == null) return false;

        _context.GroupeIdentiques.Remove(group);
        await _context.SaveChangesAsync();
        return true;
    }
}
