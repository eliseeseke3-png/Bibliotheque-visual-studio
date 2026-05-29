using System.ComponentModel.DataAnnotations;

namespace BibliothequeApp.Models
{
    public class Membre
    {
        [Key]
        public int Id_membre { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Nom")]
        public string Nom { get; set; } = string.Empty;

        [Required, StringLength(50)]
        [Display(Name = "Prénom")]
        public string Prenom { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Téléphone")]
        [RegularExpression(@"^\+229\s?[0-9]{2}\s?[0-9]{2}\s?[0-9]{2}\s?[0-9]{2}$", 
            ErrorMessage = "Le téléphone doit être au format +229 XX XX XX XX")]
        [StringLength(20)]
        public string? Telephone { get; set; }

        [StringLength(255)]
        public string? Adresse { get; set; }

        // Navigation
        public ICollection<Abonnement> Abonnements { get; set; } = new List<Abonnement>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Emprunt> Emprunts { get; set; } = new List<Emprunt>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}