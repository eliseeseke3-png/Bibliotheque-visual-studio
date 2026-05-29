using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace BibliothequeApp.Models
{
    public class Utilisateur
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Nom d'utilisateur")]
        public string Username { get; set; } = string.Empty;

        [Required, StringLength(255)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; } = string.Empty;

        [Required, StringLength(50)]
        [Display(Name = "Rôle")]
        public string Role { get; set; } = "Utilisateur";

        [StringLength(50)]
        public string? Nom { get; set; }

        [StringLength(50)]
        public string? Prenom { get; set; }

        [EmailAddress, StringLength(100)]
        public string? Email { get; set; }

        [StringLength(20)]
        public string? Telephone { get; set; }

        [StringLength(100)]
        public string? Adresse { get; set; }

        // Lier au membre si rôle = Membre
        [StringLength(50)]
        public string? Id_membre { get; set; }

        // ─── Méthodes utilitaires ────────────────────────────────────────────
        /// <summary>Hache un mot de passe en clair avec SHA256</summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Le mot de passe ne peut pas être vide");

            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        /// <summary>Vérifie si un mot de passe en clair correspond au hash stocké</summary>
        public static bool VerifyPassword(string plainPassword, string hash)
        {
            var hashOfInput = HashPassword(plainPassword);
            return hashOfInput == hash;
        }
    }
}