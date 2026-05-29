using BibliothequeApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliothequeApp.Models
{
    public class Reservation
    {
        [Key]
        public int Id_reservation { get; set; }

        [StringLength(50)]
        [Display(Name = "Code réservation")]
        public string CodeReservation { get; set; } = string.Empty;

        [Required]
        public int Id_membre { get; set; }

        [Required]
        public int Id_livre { get; set; }

        [Display(Name = "Date de réservation")]
        [DataType(DataType.Date)]
        public DateTime Date_reservation { get; set; } = DateTime.Today;

        [Display(Name = "Date expiration")]
        [DataType(DataType.Date)]
        public DateTime? Date_expiration { get; set; }

        [StringLength(50)]
        [Display(Name = "Statut")]
        public string Statut { get; set; } = "En attente";

        // Navigation
        [ForeignKey("Id_membre")]
        public Membre? Membre { get; set; }

        [ForeignKey("Id_livre")]
        public Livre? Livre { get; set; }
    }
}