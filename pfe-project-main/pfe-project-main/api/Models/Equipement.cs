using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PFE_PROJECT.Models
{
    public class Equipement
    {
        [Key]
        public int idEqpt { get; set; }

        public int idType { get; set; }
        public int idCat { get; set; }
        public int idMarq { get; set; }

        [Required]
        [StringLength(50)]
        public string codeEqp { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string design { get; set; } = string.Empty;

        public int? idGrpIdq { get; set; }

        [StringLength(50)]
        public string? état { get; set; }

        public DateTime? DateMiseService { get; set; }
        public int? AnnéeFabrication { get; set; }
        public DateTime? DateAcquisition { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? ValeurAcquisition { get; set; }

        [ForeignKey("Unite")]
        public int? idunite { get; set; }
        public Unite? Unite { get; set; }
    }
}


