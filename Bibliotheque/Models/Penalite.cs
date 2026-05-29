using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliothequeApp.Models
{
    public class Penalite
    {
        [Key]
        public int Id_penalite { get; set; }

        [Required]
        public int Id_emprunt { get; set; }

        [Required]
        [Column(TypeName = "decimal(15,2)")]
        [Display(Name = "Montant (FCFA)")]
        public decimal Montant { get; set; }

        [StringLength(255)]
        [Display(Name = "Raison")]
        public string? Raison { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime? Date_ { get; set; } = DateTime.Today;

        [StringLength(50)]
        [Display(Name = "Statut")]
        public string Statut { get; set; } = "Non payée";

        [NotMapped]
        [Display(Name = "Date affichée")]
        public string Date_penalite => Date_?.ToString("dd/MM/yyyy") ?? string.Empty;

        [NotMapped]
        public string? Motif { get => Raison; set => Raison = value; }

        // Navigation
        [ForeignKey("Id_emprunt")]
        public Emprunt? Emprunt { get; set; }
    }
}
