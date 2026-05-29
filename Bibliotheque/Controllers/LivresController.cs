using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BibliothequeApp.Data;
using BibliothequeApp.Models;

namespace BibliothequeApp.Controllers
{
    public class LivresController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public LivresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Livres - Page de parcours pour les membres
        [HttpGet]
        public async Task<IActionResult> Parcourir(string? searchTitle)
        {
            var query = _context.Livres.AsQueryable();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                query = query.Where(l => l.Titre.Contains(searchTitle));
            }

            var livres = await query.ToListAsync();
            ViewData["searchTitle"] = searchTitle;
            return View(livres);
        }

        // GET: Livres/Index - Page d'administration
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> Index()
        {
            var livres = await _context.Livres.ToListAsync();
            return View(livres);
        }

        // GET: Livres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres.FirstOrDefaultAsync(m => m.Id_livre == id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // GET: Livres/Create
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Livres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> Create([Bind("Titre,Auteur,Categorie,ISBN,ImageUrl,Disponibilite,Quantite,QuantiteDisponible,Description")] Livre livre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(livre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(livre);
        }

        // GET: Livres/Edit/5
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres.FindAsync(id);
            if (livre == null)
            {
                return NotFound();
            }
            return View(livre);
        }

        // POST: Livres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> Edit(int id, [Bind("Id_livre,Titre,Auteur,Categorie,ISBN,ImageUrl,Disponibilite,Quantite,QuantiteDisponible,Description")] Livre livre)
        {
            if (id != livre.Id_livre)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(livre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LivreExists(livre.Id_livre))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(livre);
        }

        // GET: Livres/Delete/5
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var livre = await _context.Livres.FirstOrDefaultAsync(m => m.Id_livre == id);
            if (livre == null)
            {
                return NotFound();
            }

            return View(livre);
        }

        // POST: Livres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizeRoles("Administrateur", "Utilisateur")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var livre = await _context.Livres.FindAsync(id);
            if (livre != null)
            {
                _context.Livres.Remove(livre);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool LivreExists(int id)
        {
            return _context.Livres.Any(e => e.Id_livre == id);
        }
    }
}
