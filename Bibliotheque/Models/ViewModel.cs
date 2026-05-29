using BibliothequeApp.Models;
using System.ComponentModel.DataAnnotations;
namespace BibliothequeApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(50)]
        public string Nom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(50)]
        public string Prenom { get; set; } = string.Empty;

        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        [StringLength(50)]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(20)]
        [Display(Name = "Téléphone")]
        public string? Telephone { get; set; }

        [StringLength(100)]
        public string? Adresse { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "La confirmation est requise")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        [Display(Name = "Confirmer le mot de passe")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rôle")]
        public string Role { get; set; } = "Utilisateur";
    }

    public class ViewModel
    {
        public int TotalLivres { get; set; }
        public int LivresDisponibles { get; set; }
        public int TotalMembres { get; set; }
        public int EmpruntsActifs { get; set; }
        public int ReservationsEnAttente { get; set; }
        public int PenalitesNonPayees { get; set; }
        public int TotalAbonnements { get; set; }
        public List<Emprunt> DerniersEmprunts { get; set; } = new();
        public List<Notification> DernieresNotifications { get; set; } = new();

        // Propriétés pour la page d'accueil membre
        public List<Livre> PopularBooks { get; set; } = new();
        public List<Livre> RecentBooks { get; set; } = new();
        public List<string> Categories { get; set; } = new();
    }
}
