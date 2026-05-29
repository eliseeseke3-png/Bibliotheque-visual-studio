
using BibliothequeApp.Controllers;
using BibliothequeApp.Data;
using BibliothequeApp.Models;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


namespace BibliothequeApp.Controllers
{
    [AuthorizeRoles("Administrateur")]
public class MembresController : BaseController
{
    private readonly ApplicationDbContext _db;
    public MembresController(ApplicationDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Membres.ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var m = await _db.Membres
            .Include(x => x.Emprunts).ThenInclude(e => e.Livre)
            .Include(x => x.Reservations).ThenInclude(r => r.Livre)
            .Include(x => x.Abonnements)
            .FirstOrDefaultAsync(x => x.Id_membre == id);
        if (m == null) return NotFound();
        return View(m);
    }

    public IActionResult Create() => View();

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Membre model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.Membres.Add(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Membre créé.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(string id)
    {
        var m = await _db.Membres.FindAsync(id);
        if (m == null) return NotFound();
        return View(m);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Membre model)
    {
        if (!ModelState.IsValid) return View(model);
        _db.Membres.Update(model);
        await _db.SaveChangesAsync();
        TempData["Success"] = "Membre modifié.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(string id)
    {
        var m = await _db.Membres.FindAsync(id);
        if (m == null) return NotFound();
        return View(m);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var m = await _db.Membres.FindAsync(id);
        if (m != null) { _db.Membres.Remove(m); await _db.SaveChangesAsync(); }
        TempData["Success"] = "Membre supprimé.";
        return RedirectToAction(nameof(Index));
    }
}
}
