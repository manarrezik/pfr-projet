using Microsoft.EntityFrameworkCore;
using PFE_PROJECT.Data;
using PFE_PROJECT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PFE_PROJECT.Services
{
    public class CaracteristiqueService : ICaracteristiqueService
    {
        private readonly ApplicationDbContext _context;

        public CaracteristiqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CaracteristiqueDTO>> GetAllCaracteristiquesAsync(string? searchTerm = null)
        {
            var query = _context.Caracteristiques.AsQueryable();

           
if (!string.IsNullOrEmpty(searchTerm))
{
    var lowerSearch = searchTerm.ToLower();
    query = query.Where(c => c.libelle.ToLower().Contains(lowerSearch));
}

            return await query
                .OrderBy(c => c.id_caracteristique)
                .Select(c => new CaracteristiqueDTO
                {
                    id_caracteristique = c.id_caracteristique,
                    libelle = c.libelle
                })
                .ToListAsync();
        }

        public async Task<CaracteristiqueDTO?> GetCaracteristiqueByIdAsync(int id)
        {
            var carac = await _context.Caracteristiques.FindAsync(id);
            return carac == null ? null : new CaracteristiqueDTO
            {
                id_caracteristique = carac.id_caracteristique,
                libelle = carac.libelle
            };
        }

        public async Task<CaracteristiqueDTO> CreateCaracteristiqueAsync(CreateCaracteristiqueDTO dto)
        {
            var carac = new Caracteristique { libelle = dto.libelle };
            _context.Caracteristiques.Add(carac);
            await _context.SaveChangesAsync();
            return new CaracteristiqueDTO { id_caracteristique = carac.id_caracteristique, libelle = carac.libelle };
        }

        public async Task<CaracteristiqueDTO?> UpdateCaracteristiqueAsync(int id, UpdateCaracteristiqueDTO dto)
        {
            var carac = await _context.Caracteristiques.FindAsync(id);
            if (carac == null) return null;
            carac.libelle = dto.libelle;
            await _context.SaveChangesAsync();
            return new CaracteristiqueDTO { id_caracteristique = carac.id_caracteristique, libelle = carac.libelle };
        }

        public async Task<bool> CanDeleteCaracteristiqueAsync(int id)
        {
            return !await _context.CaracteristiqueEquipements.AnyAsync(ce => ce.idcarac == id);
        }

        public async Task<bool> DeleteCaracteristiqueAsync(int id)
        {
            if (!await CanDeleteCaracteristiqueAsync(id)) return false;
            var carac = await _context.Caracteristiques.FindAsync(id);
            if (carac == null) return false;
            _context.Caracteristiques.Remove(carac);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

