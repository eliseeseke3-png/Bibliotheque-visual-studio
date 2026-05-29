using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BibliothequeApp.Helpers;

namespace BibliothequeApp.Models
{
    public class Abonnement
    {
        [Key]
        public int Id_abonnement { get; set; }

        [StringLength(50)]
        [Display(Name = "Code abonnement")]
        public string CodeAbonnement { get; set; } = string.Empty;

        [Required]
        public int Id_membre { get; set; }

        [Display(Name = "Date de début")]
        [DataType(DataType.Date)]
        public DateTime DateDebut { get; set; } = DateTime.Today;

        [Display(Name = "Date de fin")]
        [DataType(DataType.Date)]
        public DateTime DateFin { get; set; }

        [Display(Name = "Actif")]
        public bool Actif { get; set; } = true;

        [Display(Name = "Montant (FCFA)")]
        public decimal Montant { get; set; } = 5000;

        // Navigation
        [ForeignKey("Id_membre")]
        public Membre? Membre { get; set; }
    }
}