namespace PFE_PROJECT.Models
{
    public class OrganeDTO
    {
        public int id_organe { get; set; }
        public string code_organe { get; set; } = string.Empty;
        public string libelle_organe { get; set; } = string.Empty;
        public int id_marque { get; set; }
        public string nom_marque { get; set; } = string.Empty;

        public List<CaracteristiqueDTO> caracteristiques { get; set; } = new();
    }

    public class CreateOrganeDTO
    {
        public string code_organe { get; set; } = string.Empty;
        public string libelle_organe { get; set; } = string.Empty;
        public int id_marque { get; set; }
        public List<int> id_caracteristiques { get; set; } = new();
    }

    public class UpdateOrganeDTO
    {
        public string libelle_organe { get; set; } = string.Empty;
        public int id_marque { get; set; }
        public List<int> id_caracteristiques { get; set; } = new();
    }

    
}
