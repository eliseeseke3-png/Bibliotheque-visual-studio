using BibliothequeApp.Controllers;
using BibliothequeApp.Data;
using BibliothequeApp.Helpers;
using BibliothequeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliothequeApp.Controllers
{
    [AuthorizeRoles]
    public class ReservationsController : BaseController
    {
        private readonly ApplicationDbContext _db;
        public ReservationsController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var query = _db.Reservations.Include(r => r.Membre).Include(r => r.Livre).AsQueryable();
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId))
                query = query.Where(r => r.Id_membre == membreId);
            return View(await query.OrderByDescending(r => r.Date_reservation).ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var r = await _db.Reservations.Include(x => x.Membre).Include(x => x.Livre)
                .FirstOrDefaultAsync(x => x.Id_reservation == id);
            if (r == null) return NotFound();
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && r.Id_membre != membreId) return Forbid();
            return View(r);
        }

        public async Task<IActionResult> Create(int? livreId)
        {
            ViewBag.Membres = await _db.Membres.ToListAsync();
            ViewBag.Livres = await _db.Livres.ToListAsync();
            var model = new Reservation();
            if (livreId.HasValue) model.Id_livre = livreId.Value;
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId))
                model.Id_membre = membreId;
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation model)
        {
            // Générer code unique
            string code;
            do { code = CodeGenerator.GenerateCodeReservation(); }
            while (await _db.Reservations.AnyAsync(r => r.CodeReservation == code));
            model.CodeReservation = code;

            if (!ModelState.IsValid)
            {
                ViewBag.Membres = await _db.Membres.ToListAsync();
                ViewBag.Livres = await _db.Livres.ToListAsync();
                return View(model);
            }
            _db.Reservations.Add(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Réservation créée.";
            return RedirectToAction(nameof(Index));
        }

        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _db.Reservations.FindAsync(id);
            if (r == null) return NotFound();
            ViewBag.Membres = await _db.Membres.ToListAsync();
            ViewBag.Livres = await _db.Livres.ToListAsync();
            return View(r);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> Edit(Reservation model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Membres = await _db.Membres.ToListAsync();
                ViewBag.Livres = await _db.Livres.ToListAsync();
                return View(model);
            }
            _db.Reservations.Update(model);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Réservation mise à jour.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var r = await _db.Reservations.Include(x => x.Livre).Include(x => x.Membre)
                .FirstOrDefaultAsync(x => x.Id_reservation == id);
            if (r == null) return NotFound();
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && r.Id_membre != membreId) return Forbid();
            return View(r);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var r = await _db.Reservations.FindAsync(id);
            if (r != null)
            {
                if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && r.Id_membre != membreId) return Forbid();
                _db.Reservations.Remove(r);
                await _db.SaveChangesAsync();
            }
            TempData["Success"] = "Réservation annulée.";
            return RedirectToAction(nameof(Index));
        }
    }
}
