using BibliothequeApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BibliothequeApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Membre> Membres { get; set; }
        public DbSet<Livre> Livres { get; set; }
        public DbSet<Abonnement> Abonnements { get; set; }
        public DbSet<Emprunt> Emprunts { get; set; }
        public DbSet<Penalite> Penalites { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ─── Livre ────────────────────────────────────────────────────────
            modelBuilder.Entity<Livre>(e =>
            {
                e.HasKey(l => l.Id_livre);
                e.Property(l => l.Disponibilite).HasDefaultValue("Disponible");
            });

            // ─── Membre ───────────────────────────────────────────────────────
            modelBuilder.Entity<Membre>(e =>
            {
                e.HasKey(m => m.Id_membre);
            });

            // ─── Abonnement ───────────────────────────────────────────────────
            modelBuilder.Entity<Abonnement>(e =>
            {
                e.HasKey(a => a.Id_abonnement);
                e.Property(a => a.Montant).HasColumnType("decimal(15,2)");
                e.HasOne(a => a.Membre)
                    .WithMany(m => m.Abonnements)
                    .HasForeignKey(a => a.Id_membre)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Emprunt ──────────────────────────────────────────────────────
            modelBuilder.Entity<Emprunt>(e =>
            {
                e.HasKey(emp => emp.Id_emprunt);
                e.Property(emp => emp.Penalite).HasColumnType("decimal(15,2)");
                e.HasOne(emp => emp.Membre)
                    .WithMany(m => m.Emprunts)
                    .HasForeignKey(emp => emp.Id_membre)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(emp => emp.Livre)
                    .WithMany(l => l.Emprunts)
                    .HasForeignKey(emp => emp.Id_livre)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Penalite ─────────────────────────────────────────────────────
            modelBuilder.Entity<Penalite>(e =>
            {
                e.HasKey(p => p.Id_penalite);
                e.HasOne(p => p.Emprunt)
                    .WithMany(emp => emp.Penalites)
                    .HasForeignKey(p => p.Id_emprunt)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Reservation ──────────────────────────────────────────────────
            modelBuilder.Entity<Reservation>(e =>
            {
                e.HasKey(r => r.Id_reservation);
                e.HasOne(r => r.Membre)
                    .WithMany(m => m.Reservations)
                    .HasForeignKey(r => r.Id_membre)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(r => r.Livre)
                    .WithMany(l => l.Reservations)
                    .HasForeignKey(r => r.Id_livre)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Notification ─────────────────────────────────────────────────
            modelBuilder.Entity<Notification>(e =>
            {
                e.HasKey(n => n.Id_notification);
                e.HasOne(n => n.Membre)
                    .WithMany(m => m.Notifications)
                    .HasForeignKey(n => n.Id_membre)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // ─── Utilisateur ──────────────────────────────────────────────────
            modelBuilder.Entity<Utilisateur>(e =>
            {
                e.HasKey(u => u.Id);
                e.HasIndex(u => u.Username).IsUnique();
            });

            // ─── Seed des 15 livres populaires ────────────────────────────────
            modelBuilder.Entity<Livre>().HasData(
                new Livre { Id_livre = 1, Titre = "L'Alchimiste", Auteur = "Paulo Coelho", ISBN = "9782266066838", ImageUrl = "https://covers.openlibrary.org/b/id/8242088-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 2, Titre = "1984", Auteur = "George Orwell", ISBN = "9782070360833", ImageUrl = "https://covers.openlibrary.org/b/id/8241625-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 3, Titre = "Le Petit Prince", Auteur = "Antoine de Saint-Exupéry", ISBN = "9782070612758", ImageUrl = "https://covers.openlibrary.org/b/id/7919477-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 4, Titre = "Harry Potter à l'École des Sorciers", Auteur = "J.K. Rowling", ISBN = "9782253044932", ImageUrl = "https://covers.openlibrary.org/b/id/8251599-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 5, Titre = "Les Misérables", Auteur = "Victor Hugo", ISBN = "9782253096344", ImageUrl = "https://covers.openlibrary.org/b/id/8415236-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 6, Titre = "Sapiens", Auteur = "Yuval Noah Harari", ISBN = "9782226257017", ImageUrl = "https://covers.openlibrary.org/b/id/8255978-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 7, Titre = "Le Seigneur des Anneaux", Auteur = "J.R.R. Tolkien", ISBN = "9782253050766", ImageUrl = "https://covers.openlibrary.org/b/id/8241708-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 8, Titre = "Don Quichotte", Auteur = "Miguel de Cervantes", ISBN = "9782253084496", ImageUrl = "https://covers.openlibrary.org/b/id/8380151-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 9, Titre = "Orgueil et Préjugés", Auteur = "Jane Austen", ISBN = "9782253064688", ImageUrl = "https://covers.openlibrary.org/b/id/8415226-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 10, Titre = "Le Comte de Monte-Cristo", Auteur = "Alexandre Dumas", ISBN = "9782253053261", ImageUrl = "https://covers.openlibrary.org/b/id/8254066-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 11, Titre = "Cent Ans de Solitude", Auteur = "Gabriel García Márquez", ISBN = "9782253044529", ImageUrl = "https://covers.openlibrary.org/b/id/8252886-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 12, Titre = "Le Code Da Vinci", Auteur = "Dan Brown", ISBN = "9782253050995", ImageUrl = "https://covers.openlibrary.org/b/id/8234605-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 13, Titre = "Le Hobbit", Auteur = "J.R.R. Tolkien", ISBN = "9782253053681", ImageUrl = "https://covers.openlibrary.org/b/id/8415328-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 14, Titre = "La Bête humaine", Auteur = "Émile Zola", ISBN = "9782253052654", ImageUrl = "https://covers.openlibrary.org/b/id/8385577-M.jpg", Disponibilite = "Disponible" },
                new Livre { Id_livre = 15, Titre = "Croisades", Auteur = "Jonathan Riley-Smith", ISBN = "9782746004092", ImageUrl = "https://covers.openlibrary.org/b/id/8380242-M.jpg", Disponibilite = "Disponible" }
            );

            // ─── Seed admin par défaut ────────────────────────────────────────
            // Note: Le mot de passe "1234" est hashé avec SHA256
            modelBuilder.Entity<Utilisateur>().HasData(new Utilisateur
            {
                Id = 1,
                Username = "admin",
                Password = BibliothequeApp.Models.Utilisateur.HashPassword("1234"),
                Role = "Administrateur",
                Nom = "Admin",
                Prenom = "Système"
            });
        }
    }
}
