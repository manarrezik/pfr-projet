using System.Collections.Generic;
using System.Threading.Tasks;
using PFE_PROJECT.Models;

namespace PFE_PROJECT.Services
{
    public interface ICaracteristiqueService
    {
        Task<IEnumerable<CaracteristiqueDTO>> GetAllCaracteristiquesAsync(string? searchTerm = null);
        Task<CaracteristiqueDTO?> GetCaracteristiqueByIdAsync(int id);
        Task<CaracteristiqueDTO> CreateCaracteristiqueAsync(CreateCaracteristiqueDTO dto);
        Task<CaracteristiqueDTO?> UpdateCaracteristiqueAsync(int id, UpdateCaracteristiqueDTO dto);
        Task<bool> CanDeleteCaracteristiqueAsync(int id);
        Task<bool> DeleteCaracteristiqueAsync(int id);
    }
}