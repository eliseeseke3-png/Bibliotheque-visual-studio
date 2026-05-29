using System.ComponentModel.DataAnnotations;

namespace BibliothequeApp.Models
{
    public class Livre
    {
        [Key]
        public int Id_livre { get; set; }

        [Required, StringLength(255)]
        [Display(Name = "Titre")]
        public string Titre { get; set; } = string.Empty;

        [Required, StringLength(255)]
        [Display(Name = "Auteur")]
        public string Auteur { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "ISBN")]
        public string? ISBN { get; set; }

        [StringLength(100)]
        [Display(Name = "Catégorie")]
        public string? Categorie { get; set; }

        [StringLength(50)]
        [Display(Name = "Disponibilité")]
        public string Disponibilite { get; set; } = "Disponible";

        [Display(Name = "Quantité")]
        public int Quantite { get; set; } = 1;

        [Display(Name = "Quantité disponible")]
        public int QuantiteDisponible { get; set; } = 1;

        [StringLength(500)]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [StringLength(2000)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        // Navigation
        public ICollection<Emprunt> Emprunts { get; set; } = new List<Emprunt>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
