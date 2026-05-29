using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliothequeApp.Data;
using BibliothequeApp.Models;

namespace BibliothequeApp.Controllers
{
    public class AuthController : BaseController
    {
        private readonly ApplicationDbContext _db;

        public AuthController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ─── GET Login ────────────────────────────────────────────────────────
        public IActionResult Login(bool registered = false)
        {
            // Si l'utilisateur est déjà connecté ET que ce n'est pas un retour après inscription,
            // on le redirige vers la page d'accueil.
            if (!string.IsNullOrEmpty(CurrentUsername) && !registered)
                return RedirectToAction("Index", "Home");
            return View("~/Views/Shared/Auth/Login.cshtml");
        }

        // ─── POST Login ───────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) 
                return View("~/Views/Shared/Auth/Login.cshtml", model);

            // 1️⃣ Chercher l'utilisateur par username
            var user = await _db.Utilisateurs
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            // 2️⃣ Si utilisateur n'existe pas → message spécifique
            if (user == null)
            {
                ModelState.AddModelError("", "Ce compte n'existe pas. Veuillez vous inscrire.");
                ViewBag.ShowRegisterLink = true;
                return View("~/Views/Shared/Auth/Login.cshtml", model);
            }

            // 3️⃣ Vérifier le mot de passe (hashé)
            if (!Utilisateur.VerifyPassword(model.Password, user.Password))
            {
                ModelState.AddModelError("", "Nom d'utilisateur ou mot de passe incorrect");
                return View("~/Views/Shared/Auth/Login.cshtml", model);
            }

            // 4️⃣ Authentification réussie → créer la session
            HttpContext.Session.SetString("Username", user.Username);
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            if (user.Id_membre != null)
                HttpContext.Session.SetString("MembreId", user.Id_membre);

            // 5️⃣ Redirection selon le rôle
            if (user.Role == "Administrateur" || user.Role == "Utilisateur")
                return RedirectToAction("Index", "Home");
            else if (user.Role == "Membre")
                return RedirectToAction("Index", "Livres"); // Membres vont aux livres
            else
                return RedirectToAction("Index", "Home");
        }

        // ─── GET Register ─────────────────────────────────────────────────────
        public IActionResult Register()
        {
            return View();
        }

        // ─── POST Register ────────────────────────────────────────────────────
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Vérifier doublon username
            if (await _db.Utilisateurs.AnyAsync(u => u.Username == model.Username))
            {
                ModelState.AddModelError("Username", "Ce nom d'utilisateur est déjà pris");
                return View(model);
            }

            model.Role = "Membre";

            try
            {
                var membre = new Membre
                {
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    Telephone = model.Telephone,
                    Adresse = model.Adresse
                };

                _db.Membres.Add(membre);
                await _db.SaveChangesAsync();
                int membreId = membre.Id_membre;

                var user = new Utilisateur
                {
                    Username = model.Username,
                    Password = Utilisateur.HashPassword(model.Password),
                    Role = "Membre",
                    Nom = model.Nom,
                    Prenom = model.Prenom,
                    Email = model.Email,
                    Telephone = model.Telephone,
                    Adresse = model.Adresse,
                    Id_membre = membreId.ToString()
                };

                _db.Utilisateurs.Add(user);
                await _db.SaveChangesAsync();

                // Connecter automatiquement l'utilisateur en créant la session
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                if (user.Id_membre != null)
                    HttpContext.Session.SetString("MembreId", user.Id_membre);

                TempData["SuccessMessage"] = "Compte créé";
                // Rediriger le membre vers la page de parcours des livres
                return RedirectToAction("Parcourir", "Livres");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Erreur lors de la création du compte, veuillez réessayer plus tard.");
                return View(model);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Erreur inattendue lors de la création du compte.");
                return View(model);
            }

        }

        // ─── Logout ───────────────────────────────────────────────────────────
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // ─── Accès Refusé ─────────────────────────────────────────────────────
        public IActionResult AccesDenied()
        {
            return View();
        }
    }
}
