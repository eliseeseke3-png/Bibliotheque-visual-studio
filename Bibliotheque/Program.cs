using Microsoft.EntityFrameworkCore;
using BibliothequeApp.Data;
using BibliothequeApp.Models;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// ─── Services ─────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ─── Auto-migration et seeder au démarrage ────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // Appliquer les migrations
    db.Database.Migrate();

    // Forcer la création/mise à jour de l'admin par défaut (Email : admin@bibliotheque.com / Mot de passe : 1234)
    if (!db.Utilisateurs.Any(u => u.Email == "admin@bibliotheque.com" || u.Username == "admin"))
    {
        db.Utilisateurs.Add(new Utilisateur
        {
            Username = "admin",
            Email = "admin@bibliotheque.com",
            Password = Utilisateur.HashPassword("1234"),
            Role = "Administrateur",
            Nom = "Admin",
            Prenom = "Système"
        });
        db.SaveChanges();
    }

    // Seed les 30+ livres si la table est vide
    if (!db.Livres.Any(l => l.Id_livre > 15))
    {
        var nouveauLivres = new List<Livre>
        {
            new Livre { Titre = "L'île au trésor", Auteur = "Robert Louis Stevenson", Categorie = "Aventure", ISBN = "9782253054284", ImageUrl = "https://covers.openlibrary.org/b/id/8388241-M.jpg", Disponibilite = "Disponible", Quantite = 6, QuantiteDisponible = 6, Description = "Une quête de trésor légendaire" },
            new Livre { Titre = "Moby Dick", Auteur = "Herman Melville", Categorie = "Aventure", ISBN = "9782253058199", ImageUrl = "https://covers.openlibrary.org/b/id/8389541-M.jpg", Disponibilite = "Disponible", Quantite = 4, QuantiteDisponible = 4, Description = "La chasse à la baleine blanche" },
            new Livre { Titre = "Jane Eyre", Auteur = "Charlotte Brontë", Categorie = "Romance", ISBN = "9782253055662", ImageUrl = "https://covers.openlibrary.org/b/id/8415364-M.jpg", Disponibilite = "Disponible", Quantite = 5, QuantiteDisponible = 5, Description = "Un roman d'amour et de rébellion" },
            new Livre { Titre = "Wuthering Heights", Auteur = "Emily Brontë", Categorie = "Romance", ISBN = "9782253058267", ImageUrl = "https://covers.openlibrary.org/b/id/8415452-M.jpg", Disponibilite = "Disponible", Quantite = 4, QuantiteDisponible = 4, Description = "Une histoire de passion tourmentée" },
            new Livre { Titre = "Les Hauts de Hurlevent", Auteur = "Emily Brontë", Categorie = "Classique", ISBN = "9782253084513", ImageUrl = "https://covers.openlibrary.org/b/id/8415452-M.jpg", Disponibilite = "Disponible", Quantite = 3, QuantiteDisponible = 3, Description = "Un drame romantique intense" },
            new Livre { Titre = "Frankenstein", Auteur = "Mary Shelley", Categorie = "Science-fiction", ISBN = "9782253058373", ImageUrl = "https://covers.openlibrary.org/b/id/8388654-M.jpg", Disponibilite = "Disponible", Quantite = 5, QuantiteDisponible = 5, Description = "L'histoire de la créature et de son créateur" },
            new Livre { Titre = "Le Fantôme de l'Opéra", Auteur = "Gaston Leroux", Categorie = "Thriller", ISBN = "9782253048459", ImageUrl = "https://covers.openlibrary.org/b/id/8388852-M.jpg", Disponibilite = "Disponible", Quantite = 6, QuantiteDisponible = 6, Description = "Le mystère du Fantôme de l'Opéra" },
            new Livre { Titre = "Les Trois Mousquetaires", Auteur = "Alexandre Dumas", Categorie = "Aventure", ISBN = "9782253056621", ImageUrl = "https://covers.openlibrary.org/b/id/8389652-M.jpg", Disponibilite = "Disponible", Quantite = 7, QuantiteDisponible = 7, Description = "Les aventures de d'Artagnan et ses amis" },
            new Livre { Titre = "L'Homme invisible", Auteur = "H.G. Wells", Categorie = "Science-fiction", ISBN = "9782253053859", ImageUrl = "https://covers.openlibrary.org/b/id/8388954-M.jpg", Disponibilite = "Disponible", Quantite = 4, QuantiteDisponible = 4, Description = "L'histoire d'un homme rendu invisible" },
            new Livre { Titre = "La Machine à explorer le temps", Auteur = "H.G. Wells", Categorie = "Science-fiction", ISBN = "9782253057406", ImageUrl = "https://covers.openlibrary.org/b/id/8388965-M.jpg", Disponibilite = "Disponible", Quantite = 5, QuantiteDisponible = 5, Description = "Un voyage dans le temps" },
            new Livre { Titre = "Atomic Habits", Auteur = "James Clear", Categorie = "Développement personnel", ISBN = "9782378150570", ImageUrl = "https://covers.openlibrary.org/b/id/12265156-M.jpg", Disponibilite = "Disponible", Quantite = 8, QuantiteDisponible = 8, Description = "Comment construire de bonnes habitudes" },
            new Livre { Titre = "Les 7 habitudes des gens efficaces", Auteur = "Stephen Covey", Categorie = "Développement personnel", ISBN = "9782100518456", ImageUrl = "https://covers.openlibrary.org/b/id/8254963-M.jpg", Disponibilite = "Disponible", Quantite = 6, QuantiteDisponible = 6, Description = "Les clés de la productivité" },
            new Livre { Titre = "L'art de la guerre", Auteur = "Sun Tzu", Categorie = "Philosophie", ISBN = "9782253055860", ImageUrl = "https://covers.openlibrary.org/b/id/8389745-M.jpg", Disponibilite = "Disponible", Quantite = 5, QuantiteDisponible = 5, Description = "La stratégie dans la vie et les affaires" },
            new Livre { Titre = "De la Terre à la Lune", Auteur = "Jules Verne", Categorie = "Science-fiction", ISBN = "9782253053170", ImageUrl = "https://covers.openlibrary.org/b/id/8388852-M.jpg", Disponibilite = "Disponible", Quantite = 4, QuantiteDisponible = 4, Description = "Une aventure lunaire extraordinaire" },
            new Livre { Titre = "Vingt mille lieues sous les mers", Auteur = "Jules Verne", Categorie = "Aventure", ISBN = "9782253051305", ImageUrl = "https://covers.openlibrary.org/b/id/8389875-M.jpg", Disponibilite = "Disponible", Quantite = 5, QuantiteDisponible = 5, Description = "Les mystères de l'océan avec le Capitaine Némo" },
            new Livre { Titre = "Le Voyage au centre de la Terre", Auteur = "Jules Verne", Categorie = "Aventure", ISBN = "9782253057581", ImageUrl = "https://covers.openlibrary.org/b/id/8389956-M.jpg", Disponibilite = "Disponible", Quantite = 4, QuantiteDisponible = 4, Description = "Une expédition souterraine fascinante" }
        };

        db.Livres.AddRange(nouveauLivres);
        db.SaveChanges();
    }
}

// ─── Pipeline ─────────────────────────────────────────────────────────────────
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

// Route par défaut → Login
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// Redirection racine vers Auth/Login
app.MapGet("/", () => Results.Redirect("/Auth/Login"));

app.Run();
