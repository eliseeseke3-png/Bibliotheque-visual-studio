using BibliothequeApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliothequeApp.Models
{
    public class Emprunt
    {
        [Key]
        public int Id_emprunt { get; set; }

        [StringLength(50)]
        [Display(Name = "Code d'emprunt")]
        public string CodeEmprunt { get; set; } = string.Empty;

        [Required]
        public int Id_membre { get; set; }

        [Required]
        public int Id_livre { get; set; }

        [Display(Name = "Date d'emprunt")]
        [DataType(DataType.Date)]
        public DateTime Date_emprunt { get; set; } = DateTime.Today;

        [Display(Name = "Date de retour prévue")]
        [DataType(DataType.Date)]
        public DateTime Date_retour_prevue { get; set; }

        [Display(Name = "Date de retour effective")]
        [DataType(DataType.Date)]
        public DateTime? Date_retour_effective { get; set; }

        [Display(Name = "Pénalité (FCFA)")]
        public decimal Penalite { get; set; } = 0;

        [StringLength(50)]
        [Display(Name = "Statut")]
        public string Statut { get; set; } = "En cours";

        // Navigation
        [ForeignKey("Id_membre")]
        public Membre? Membre { get; set; }

        [ForeignKey("Id_livre")]
        public Livre? Livre { get; set; }

        public ICollection<Penalite> Penalites { get; set; } = new List<Penalite>();
    }
}