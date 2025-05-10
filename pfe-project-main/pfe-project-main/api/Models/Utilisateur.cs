using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PFE_PROJECT.Models
{
    public class Utilisateur
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int iduser { get; set; }

        [Required]
        public int idunite { get; set; }

        [Required]
        [StringLength(255)]
        public string nom { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string prenom { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string numtel { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string motpasse { get; set; } = string.Empty;

        [Required]
        public int idrole { get; set; }

        [JsonIgnore]
        public virtual Role Role { get; set; }

        [Required]
        [StringLength(1)]
        public string Actif { get; set; } = "1"; // "1" = actif, "0" = inactif
    }
}
