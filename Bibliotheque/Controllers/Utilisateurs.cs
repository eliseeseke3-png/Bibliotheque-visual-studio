using BibliothequeApp.Controllers;
using BibliothequeApp.Data;
using BibliothequeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliothequeApp.Controllers 
{
    // ─── Gestion des comptes utilisateurs/employés : Admin uniquement ───────
    [AuthorizeRoles("Administrateur")]
    public class UtilisateursController : BaseController
    {
        private readonly ApplicationDbContext _db;
        public UtilisateursController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index() 
            => View(await _db.Utilisateurs.ToListAsync());

        public async Task<IActionResult> Details(int id)
        {
            var u = await _db.Utilisateurs.FindAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        // ─── Créer un nouvel utilisateur (employé/utilisateur, pas membre) ─────
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Utilisateur model)
        {
            if (!ModelState.IsValid) return View(model);

            // Vérifier doublon username
            if (await _db.Utilisateurs.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Ce nom d'utilisateur est déjà pris");
                return View(model);
            }

            // Hasher le mot de passe
            model.Password = Utilisateur.HashPassword(model.Password);

            // Forcer le rôle à "Utilisateur" si Admin ne le spécifie pas
            if (model.Role != "Administrateur" && model.Role != "Utilisateur")
                model.Role = "Utilisateur";

            _db.Utilisateurs.Add(model);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Utilisateur créé avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // ─── Modifier un utilisateur ──────────────────────────────────────────
        public async Task<IActionResult> Edit(int id)
        {
            var u = await _db.Utilisateurs.FindAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Utilisateur model)
        {
            if (id != model.Id) return NotFound();

            var user = await _db.Utilisateurs.FindAsync(id);
            if (user == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            // Vérifier doublon username (sauf si c'est le même)
            if (user.Username != model.Username && await _db.Utilisateurs.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Ce nom d'utilisateur est déjà pris");
                return View(model);
            }

            // Mettre à jour les champs
            user.Username = model.Username;
            user.Nom = model.Nom;
            user.Prenom = model.Prenom;
            user.Email = model.Email;
            user.Telephone = model.Telephone;
            user.Adresse = model.Adresse;
            user.Role = model.Role;

            // Mettre à jour le mot de passe seulement s'il a changé
            if (!string.IsNullOrEmpty(model.Password) && model.Password != user.Password)
            {
                user.Password = Utilisateur.HashPassword(model.Password);
            }

            _db.Utilisateurs.Update(user);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Utilisateur modifié avec succès.";
            return RedirectToAction(nameof(Index));
        }

        // ─── Supprimer un utilisateur ──────────────────────────────────────────
        public async Task<IActionResult> Delete(int id)
        {
            var u = await _db.Utilisateurs.FindAsync(id);
            if (u == null) return NotFound();
            return View(u);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var u = await _db.Utilisateurs.FindAsync(id);
            if (u != null && u.Username != "admin")
            {
                _db.Utilisateurs.Remove(u);
                await _db.SaveChangesAsync();
                TempData["Success"] = "Utilisateur supprimé.";
            }
            else if (u?.Username == "admin")
            {
                TempData["Error"] = "L'administrateur par défaut ne peut pas être supprimé.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}


