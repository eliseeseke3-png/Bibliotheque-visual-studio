using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bibliotheque.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Livres",
                columns: table => new
                {
                    Id_livre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titre = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Auteur = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Categorie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Disponibilite = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Disponible"),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    QuantiteDisponible = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Livres", x => x.Id_livre);
                });

            migrationBuilder.CreateTable(
                name: "Membres",
                columns: table => new
                {
                    Id_membre = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membres", x => x.Id_membre);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Adresse = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Id_membre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateurs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Abonnements",
                columns: table => new
                {
                    Id_abonnement = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeAbonnement = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id_membre = table.Column<int>(type: "int", nullable: false),
                    DateDebut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Actif = table.Column<bool>(type: "bit", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abonnements", x => x.Id_abonnement);
                    table.ForeignKey(
                        name: "FK_Abonnements_Membres_Id_membre",
                        column: x => x.Id_membre,
                        principalTable: "Membres",
                        principalColumn: "Id_membre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emprunts",
                columns: table => new
                {
                    Id_emprunt = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeEmprunt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id_membre = table.Column<int>(type: "int", nullable: false),
                    Id_livre = table.Column<int>(type: "int", nullable: false),
                    Date_emprunt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_retour_prevue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_retour_effective = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Penalite = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emprunts", x => x.Id_emprunt);
                    table.ForeignKey(
                        name: "FK_Emprunts_Livres_Id_livre",
                        column: x => x.Id_livre,
                        principalTable: "Livres",
                        principalColumn: "Id_livre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Emprunts_Membres_Id_membre",
                        column: x => x.Id_membre,
                        principalTable: "Membres",
                        principalColumn: "Id_membre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id_notification = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_membre = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Date_envoie = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Lu = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id_notification);
                    table.ForeignKey(
                        name: "FK_Notifications_Membres_Id_membre",
                        column: x => x.Id_membre,
                        principalTable: "Membres",
                        principalColumn: "Id_membre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id_reservation = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CodeReservation = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Id_membre = table.Column<int>(type: "int", nullable: false),
                    Id_livre = table.Column<int>(type: "int", nullable: false),
                    Date_reservation = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date_expiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id_reservation);
                    table.ForeignKey(
                        name: "FK_Reservations_Livres_Id_livre",
                        column: x => x.Id_livre,
                        principalTable: "Livres",
                        principalColumn: "Id_livre",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Membres_Id_membre",
                        column: x => x.Id_membre,
                        principalTable: "Membres",
                        principalColumn: "Id_membre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penalites",
                columns: table => new
                {
                    Id_penalite = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_emprunt = table.Column<int>(type: "int", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(15,2)", nullable: false),
                    Raison = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Date_ = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalites", x => x.Id_penalite);
                    table.ForeignKey(
                        name: "FK_Penalites_Emprunts_Id_emprunt",
                        column: x => x.Id_emprunt,
                        principalTable: "Emprunts",
                        principalColumn: "Id_emprunt",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Livres",
                columns: new[] { "Id_livre", "Auteur", "Categorie", "Description", "Disponibilite", "ISBN", "ImageUrl", "Quantite", "QuantiteDisponible", "Titre" },
                values: new object[,]
                {
                    { 1, "Paulo Coelho", null, null, "Disponible", "9782266066838", "https://covers.openlibrary.org/b/id/8242088-M.jpg", 1, 1, "L'Alchimiste" },
                    { 2, "George Orwell", null, null, "Disponible", "9782070360833", "https://covers.openlibrary.org/b/id/8241625-M.jpg", 1, 1, "1984" },
                    { 3, "Antoine de Saint-Exupéry", null, null, "Disponible", "9782070612758", "https://covers.openlibrary.org/b/id/7919477-M.jpg", 1, 1, "Le Petit Prince" },
                    { 4, "J.K. Rowling", null, null, "Disponible", "9782253044932", "https://covers.openlibrary.org/b/id/8251599-M.jpg", 1, 1, "Harry Potter à l'École des Sorciers" },
                    { 5, "Victor Hugo", null, null, "Disponible", "9782253096344", "https://covers.openlibrary.org/b/id/8415236-M.jpg", 1, 1, "Les Misérables" },
                    { 6, "Yuval Noah Harari", null, null, "Disponible", "9782226257017", "https://covers.openlibrary.org/b/id/8255978-M.jpg", 1, 1, "Sapiens" },
                    { 7, "J.R.R. Tolkien", null, null, "Disponible", "9782253050766", "https://covers.openlibrary.org/b/id/8241708-M.jpg", 1, 1, "Le Seigneur des Anneaux" },
                    { 8, "Miguel de Cervantes", null, null, "Disponible", "9782253084496", "https://covers.openlibrary.org/b/id/8380151-M.jpg", 1, 1, "Don Quichotte" },
                    { 9, "Jane Austen", null, null, "Disponible", "9782253064688", "https://covers.openlibrary.org/b/id/8415226-M.jpg", 1, 1, "Orgueil et Préjugés" },
                    { 10, "Alexandre Dumas", null, null, "Disponible", "9782253053261", "https://covers.openlibrary.org/b/id/8254066-M.jpg", 1, 1, "Le Comte de Monte-Cristo" },
                    { 11, "Gabriel García Márquez", null, null, "Disponible", "9782253044529", "https://covers.openlibrary.org/b/id/8252886-M.jpg", 1, 1, "Cent Ans de Solitude" },
                    { 12, "Dan Brown", null, null, "Disponible", "9782253050995", "https://covers.openlibrary.org/b/id/8234605-M.jpg", 1, 1, "Le Code Da Vinci" },
                    { 13, "J.R.R. Tolkien", null, null, "Disponible", "9782253053681", "https://covers.openlibrary.org/b/id/8415328-M.jpg", 1, 1, "Le Hobbit" },
                    { 14, "Émile Zola", null, null, "Disponible", "9782253052654", "https://covers.openlibrary.org/b/id/8385577-M.jpg", 1, 1, "La Bête humaine" },
                    { 15, "Jonathan Riley-Smith", null, null, "Disponible", "9782746004092", "https://covers.openlibrary.org/b/id/8380242-M.jpg", 1, 1, "Croisades" }
                });

            migrationBuilder.InsertData(
                table: "Utilisateurs",
                columns: new[] { "Id", "Adresse", "Email", "Id_membre", "Nom", "Password", "Prenom", "Role", "Telephone", "Username" },
                values: new object[] { 1, null, null, null, "Admin", "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", "Système", "Administrateur", null, "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Abonnements_Id_membre",
                table: "Abonnements",
                column: "Id_membre");

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_Id_livre",
                table: "Emprunts",
                column: "Id_livre");

            migrationBuilder.CreateIndex(
                name: "IX_Emprunts_Id_membre",
                table: "Emprunts",
                column: "Id_membre");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_Id_membre",
                table: "Notifications",
                column: "Id_membre");

            migrationBuilder.CreateIndex(
                name: "IX_Penalites_Id_emprunt",
                table: "Penalites",
                column: "Id_emprunt");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Id_livre",
                table: "Reservations",
                column: "Id_livre");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Id_membre",
                table: "Reservations",
                column: "Id_membre");

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateurs_Username",
                table: "Utilisateurs",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abonnements");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Penalites");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Utilisateurs");

            migrationBuilder.DropTable(
                name: "Emprunts");

            migrationBuilder.DropTable(
                name: "Livres");

            migrationBuilder.DropTable(
                name: "Membres");
        }
    }
}
