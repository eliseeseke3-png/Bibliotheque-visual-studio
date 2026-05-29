using BibliothequeApp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BibliothequeApp.Models
{
    public class Notification
    {
        [Key]
        public int Id_notification { get; set; }

        [Required]
        public int Id_membre { get; set; }

        [StringLength(255)]
        [Display(Name = "Message")]
        public string? Message { get; set; }

        [Display(Name = "Date d'envoi")]
        [DataType(DataType.DateTime)]
        public DateTime? Date_envoie { get; set; } = DateTime.Now;

        [StringLength(50)]
        [Display(Name = "Type")]
        public string? Type { get; set; }

        [Display(Name = "Lu")]
        public bool Lu { get; set; } = false;

        // Navigation
        [ForeignKey("Id_membre")]
        public Membre? Membre { get; set; }
    }
}