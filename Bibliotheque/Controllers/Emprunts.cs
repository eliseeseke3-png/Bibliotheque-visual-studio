using BibliothequeApp.Controllers;
using BibliothequeApp.Data;
using BibliothequeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BibliothequeApp.Controllers
{

    [AuthorizeRoles]
public class EmpruntsController : BaseController
{
    private readonly ApplicationDbContext _db;
    public EmpruntsController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var query = _db.Emprunts.Include(e => e.Membre).Include(e => e.Livre).AsQueryable();
        if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId))
            query = query.Where(e => e.Id_membre == membreId);
        return View(await query.OrderByDescending(e => e.Date_emprunt).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var e = await _db.Emprunts
            .Include(x => x.Membre).Include(x => x.Livre).Include(x => x.Penalites)
            .FirstOrDefaultAsync(x => x.Id_emprunt == id);
        if (e == null) return NotFound();
        if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && e.Id_membre != membreId) return Forbid();
        return View(e);
    }

    [AuthorizeRoles("Administrateur", "Utilisateur", "Membre")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Membres = await _db.Membres.ToListAsync();
        ViewBag.Livres = await _db.Livres.Where(l => l.Disponibilite == "Disponible").ToListAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    [AuthorizeRoles("Administrateur", "Utilisateur", "Membre")]
    public async Task<IActionResult> Create(Emprunt model)
    {
        // Si l'utilisateur est un membre connecté, forcer l'Id_membre depuis la session
        if (IsMembre)
        {
            if (string.IsNullOrEmpty(CurrentMembreId) || !int.TryParse(CurrentMembreId, out int membreId))
            {
                ModelState.AddModelError("", "Impossible de récupérer votre identifiant membre. Veuillez vous reconnecter.");
                ViewBag.Membres = await _db.Membres.ToListAsync();
                ViewBag.Livres = await _db.Livres.Where(l => l.Disponibilite == "Disponible").ToListAsync();
                return View(model);
            }
            model.Id_membre = membreId;
        }

        // Générer un code d'emprunt si manquant
        if (string.IsNullOrWhiteSpace(model.CodeEmprunt))
        {
            model.CodeEmprunt = $"EMP-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        // Définir des dates par défaut si nécessaire
        if (model.Date_emprunt == default)
            model.Date_emprunt = DateTime.Today;
        if (model.Date_retour_prevue == default)
            model.Date_retour_prevue = DateTime.Today.AddDays(14);

        if (!ModelState.IsValid)
        {
            ViewBag.Membres = await _db.Membres.ToListAsync();
            ViewBag.Livres = await _db.Livres.Where(l => l.Disponibilite == "Disponible").ToListAsync();
            return View(model);
        }

        // Marquer livre comme indisponible
        var livre = await _db.Livres.FindAsync(model.Id_livre);
        if (livre != null) { livre.Disponibilite = "Emprunté"; _db.Livres.Update(livre); }

        _db.Emprunts.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Emprunt enregistré.";
        return RedirectToAction(nameof(Index));
    }

    [AuthorizeRoles("Administrateur", "Utilisateur")]
    public async Task<IActionResult> Edit(int id)
    {
        var e = await _db.Emprunts.FindAsync(id);
        if (e == null) return NotFound();
        ViewBag.Membres = await _db.Membres.ToListAsync();
        ViewBag.Livres = await _db.Livres.ToListAsync();
        return View(e);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [AuthorizeRoles("Administrateur", "Utilisateur")]
    public async Task<IActionResult> Edit(Emprunt model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Membres = await _db.Membres.ToListAsync();
            ViewBag.Livres = await _db.Livres.ToListAsync();
            return View(model);
        }
        // Si retour effectué → remettre dispo
        if (model.Date_retour_effective.HasValue)
        {
            var livre = await _db.Livres.FindAsync(model.Id_livre);
            if (livre != null) { livre.Disponibilite = "Disponible"; _db.Livres.Update(livre); }
            model.Statut = "Retourné";
        }
        _db.Emprunts.Update(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Emprunt mis à jour.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var e = await _db.Emprunts.Include(x => x.Livre).Include(x => x.Membre)
            .FirstOrDefaultAsync(x => x.Id_emprunt == id);
        if (e == null) return NotFound();
        if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && e.Id_membre != membreId) return Forbid();
        return View(e);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var e = await _db.Emprunts.FindAsync(id);
        if (e != null)
        {
            if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId) && e.Id_membre != membreId) return Forbid();
            // Remettre livre dispo
            var livre = await _db.Livres.FindAsync(e.Id_livre);
            if (livre != null) { livre.Disponibilite = "Disponible"; _db.Livres.Update(livre); }
            _db.Emprunts.Remove(e);
            await _db.SaveChangesAsync();
        }
        TempData["Success"] = "Emprunt supprimé.";
        return RedirectToAction(nameof(Index));
    }
}
}
