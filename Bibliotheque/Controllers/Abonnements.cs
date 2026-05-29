using BibliothequeApp.Data;
using BibliothequeApp.Helpers;
using BibliothequeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliothequeApp.Controllers
{
    [AuthorizeRoles]
    public class AbonnementsController : BaseController
    {
        private readonly ApplicationDbContext _db;
        public AbonnementsController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var query = _db.Abonnements.Include(a => a.Membre).AsQueryable();
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId))
                query = query.Where(a => a.Id_membre == membreId);
            return View(await query.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var a = await _db.Abonnements.Include(x => x.Membre)
                             .FirstOrDefaultAsync(x => x.Id_abonnement == id);
            if (a == null) return NotFound();
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && a.Id_membre != membreId)
                return Forbid();
            return View(a);
        }

        public async Task<IActionResult> Create()
        {
            var model = new Abonnement();
            var members = await _db.Membres.ToListAsync();

            // Préparer une liste d'éléments pour asp-items
            ViewBag.MembresSelect = members
                .Select(m => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = m.Id_membre.ToString(),
                    Text = m.Nom + " " + m.Prenom
                })
                .ToList();

            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId))
                model.Id_membre = membreId;

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Abonnement model)
        {
            // Générer un code unique
            string code;
            do { code = CodeGenerator.GenerateCodeAbonnement(); }
            while (await _db.Abonnements.AnyAsync(a => a.CodeAbonnement == code));
            model.CodeAbonnement = code;

            // Calcul automatique dates
            model.DateDebut = DateTime.Today;
            model.DateFin = DateTime.Today.AddMonths(1);

            _db.Abonnements.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Abonnement créé.";
            return RedirectToAction(nameof(Index));
        }

        [AuthorizeRoles("Administrateur")]
        public async Task<IActionResult> Edit(int id)
        {
            var a = await _db.Abonnements.FindAsync(id);
            if (a == null) return NotFound();
            ViewBag.Membres = await _db.Membres.ToListAsync();
            return View(a);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AuthorizeRoles("Administrateur")]
        public async Task<IActionResult> Edit(Abonnement model)
        {
            _db.Abonnements.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Abonnement mis à jour.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var a = await _db.Abonnements.Include(x => x.Membre)
                             .FirstOrDefaultAsync(x => x.Id_abonnement == id);
            if (a == null) return NotFound();
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && a.Id_membre != membreId)
                return Forbid();
            return View(a);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var a = await _db.Abonnements.FindAsync(id);
            if (a != null)
            {
                if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && a.Id_membre != membreId)
                    return Forbid();
                _db.Abonnements.Remove(a);
                await _db.SaveChangesAsync();
            }
            TempData["Success"] = "Abonnement supprimé.";
            return RedirectToAction(nameof(Index));
        }
    }
}