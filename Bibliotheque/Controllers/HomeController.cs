using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliothequeApp.Data;
using BibliothequeApp.Models;

namespace BibliothequeApp.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            // Rediriger vers login si non connecté
            if (string.IsNullOrEmpty(CurrentUsername))
                return RedirectToAction("Login", "Auth");

            var vm = new ViewModel
            {
                TotalLivres = await _db.Livres.CountAsync(),
                LivresDisponibles = await _db.Livres.CountAsync(l => l.Disponibilite == "Disponible"),
                TotalMembres = await _db.Membres.CountAsync(),
                EmpruntsActifs = await _db.Emprunts.CountAsync(e => e.Statut == "En cours"),
                ReservationsEnAttente = await _db.Reservations.CountAsync(r => r.Statut == "En attente"),
                PenalitesNonPayees = await _db.Penalites.CountAsync(p => p.Statut == "Non payée"),
                TotalAbonnements = await _db.Abonnements.CountAsync(),
                // Livres populaires (les plus empruntés, avec limite 6)
                PopularBooks = await _db.Livres
                    .Where(l => l.Disponibilite == "Disponible")
                    .OrderByDescending(l => l.Quantite)
                    .Take(6)
                    .ToListAsync(),
                // Livres récents (ajoutés récemment)
                RecentBooks = await _db.Livres
                    .OrderByDescending(l => l.Id_livre)
                    .Take(5)
                    .ToListAsync(),
                // Catégories uniques
                Categories = await _db.Livres
                    .Where(l => !string.IsNullOrEmpty(l.Categorie))
                    .Select(l => l.Categorie)
                    .Distinct()
                    .ToListAsync()
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}