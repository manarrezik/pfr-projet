namespace PFE_PROJECT.Models
{
    public class UpdateGroupeIdentiqueDTO
    {
        public string Designation { get; set; } = string.Empty;
        public List<int> id_organes { get; set; } = new();
        public List<int> id_caracteristiques { get; set; } = new();
    }
}

