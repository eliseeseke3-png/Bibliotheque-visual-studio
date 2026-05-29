using BibliothequeApp.Controllers;
using BibliothequeApp.Data;
using BibliothequeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace BibliothequeApp.Controllers
{

    [AuthorizeRoles]
public class PenalitesController : BaseController
{
    private readonly ApplicationDbContext _db;
    public PenalitesController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        var query = _db.Penalites.Include(p => p.Emprunt).ThenInclude(e => e!.Membre)
            .Include(p => p.Emprunt).ThenInclude(e => e!.Livre).AsQueryable();

        if (IsMembre && !string.IsNullOrEmpty(CurrentMembreId) && int.TryParse(CurrentMembreId, out int membreId))
            query = query.Where(p => p.Emprunt!.Id_membre == membreId);

        return View(await query.ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var p = await _db.Penalites.Include(x => x.Emprunt).ThenInclude(e => e!.Membre)
            .FirstOrDefaultAsync(x => x.Id_penalite == id);
        if (p == null) return NotFound();
        return View(p);
    }

    [AuthorizeRoles("Administrateur")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Emprunts = await _db.Emprunts.Include(e => e.Membre).ToListAsync();
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    [AuthorizeRoles("Administrateur")]
    public async Task<IActionResult> Create(Penalite model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Emprunts = await _db.Emprunts.Include(e => e.Membre).ToListAsync();
            return View(model);
        }
        _db.Penalites.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Pénalité créée.";
        return RedirectToAction(nameof(Index));
    }

    [AuthorizeRoles("Administrateur")]
    public async Task<IActionResult> Edit(int id)
    {
        var p = await _db.Penalites.FindAsync(id);
        if (p == null) return NotFound();
        ViewBag.Emprunts = await _db.Emprunts.Include(e => e.Membre).ToListAsync();
        return View(p);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [AuthorizeRoles("Administrateur")]
    public async Task<IActionResult> Edit(Penalite model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Emprunts = await _db.Emprunts.Include(e => e.Membre).ToListAsync();
            return View(model);
        }
        _db.Penalites.Update(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Pénalité mise à jour.";
        return RedirectToAction(nameof(Index));
    }

    [AuthorizeRoles("Administrateur")]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Penalites.Include(x => x.Emprunt).FirstOrDefaultAsync(x => x.Id_penalite == id);
        if (p == null) return NotFound();
        return View(p);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    [AuthorizeRoles("Administrateur")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var p = await _db.Penalites.FindAsync(id);
        if (p != null) { _db.Penalites.Remove(p); await _db.SaveChangesAsync(); }
        TempData["Success"] = "Pénalité supprimée.";
        return RedirectToAction(nameof(Index));
    }
}
}
