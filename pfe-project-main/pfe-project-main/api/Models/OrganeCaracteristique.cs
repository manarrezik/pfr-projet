using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    [Table("organecaracteristique")]
    public class OrganeCaracteristique
    {
        public int id_organe { get; set; }
        public Organe Organe { get; set; } = null!;

        public int id_caracteristique { get; set; }
        public Caracteristique Caracteristique { get; set; } = null!;
    }
}
