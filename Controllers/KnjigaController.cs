using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using web.Models;
using help;

namespace web.Controllers
{
    public class KnjigaController : Controller
    {
        private readonly SchoolContext _context;

        public KnjigaController(SchoolContext context)
        {
            _context = context;
        }

        // GET: Knjiga
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? page, string sortBy)
        {
            // Set the number of items per page
            int pageSize = 6;

            // Retrieve all books from the database without including related entities for now
            var knjige = _context.Knjige.AsQueryable();

            // If a search string is provided, filter the books by name
            if (!String.IsNullOrEmpty(searchString))
            {
                knjige = knjige.Where(k => k.Naslov.Contains(searchString));
            }

            // Sorting logic
            ViewData["NaslovSortParm"] = sortOrder == "naslov" ? "naslov_desc" : "naslov";
            ViewData["AvtorSortParm"] = sortOrder == "avtor" ? "avtor_desc" : "avtor";
            ViewData["ZvrstSortParm"] = sortOrder == "zvrst" ? "zvrst_desc" : "zvrst";
            ViewData["KategorijaSortParm"] = sortOrder == "kategorija" ? "kategorija_desc" : "kategorija";
            ViewData["OcenaSortParm"] = sortOrder == "ocena" ? "ocena_desc" : "ocena";
 switch (sortOrder)
{
    case "naslov":
        knjige = sortOrder == "desc" ? knjige.OrderByDescending(k => k.Naslov) : knjige.OrderBy(k => k.Naslov);
        break;
    case "avtor":
        knjige = sortOrder == "desc" ? knjige.OrderByDescending(k => k.Avtor.PriimekIme) : knjige.OrderBy(k => k.Avtor.PriimekIme);
        break;
    case "ocena":
        knjige = sortOrder == "desc" ? knjige.OrderByDescending(k => k.Ocena) : knjige.OrderBy(k => k.Ocena);
        break;
    // Add other cases for your other sorting options
    default:
        knjige = sortOrder == "desc" ? knjige.OrderByDescending(k => k.Naslov) : knjige.OrderBy(k => k.Naslov);
        break;
}


            // Now include the related entities after filtering and sorting
            knjige = knjige.Include(k => k.Avtor).Include(k => k.Kategorija).Include(k => k.Zvrst);

            // Use PaginatedList to paginate the result
            var pageNumber = page ?? 1;
            var pagedKnjige = await PaginatedList<Knjiga>.CreateAsync(knjige.AsNoTracking(), pageNumber, pageSize);

            // Return the paginated list of books to the view
            return View(pagedKnjige);
        }

        // GET: Knjiga/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige
                .Include(k => k.Avtor)
                .Include(k => k.Kategorija)
                .Include(k => k.Zvrst)
                .FirstOrDefaultAsync(m => m.KnjigaId == id);
            if (knjiga == null)
            {
                return NotFound();
            }

            return View(knjiga);
        }
public IActionResult Rezervacija(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    // Perform any logic related to reservation here.
    // For example, you can check if the book is available and perform the reservation process.

    // Assuming you have a Rezervacija view named Index.cshtml, you can redirect to it.
    return RedirectToAction("Create", "Rezervacija", new { id = id });
}
        // GET: Knjiga/Create
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Create()
        {
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "PriimekIme");
            ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "imeKategorije");
            ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ImeZvrsti");
            return View();
        }

        // POST: Knjiga/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Create([Bind("KnjigaId,Naslov,Ocena,AvtorID,ZvrstID,KategorijaID,ImageLink")] Knjiga knjiga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(knjiga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "AvtorID", knjiga.AvtorID);
            ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID", knjiga.KategorijaID);
            ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID", knjiga.ZvrstID);
            return View(knjiga);
        }

        // GET: Knjiga/Edit/5
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga == null)
            {
                return NotFound();
            }

            // Check if the current user is in the "Administrator" or "Manager" role
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "AvtorID", knjiga.AvtorID);
                ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID", knjiga.KategorijaID);
                ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID", knjiga.ZvrstID);
                return View(knjiga);
            }
            else
            {
                // Redirect to an unauthorized page or return a specific view
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

        // POST: Knjiga/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("KnjigaId,Naslov,Ocena,AvtorID,ZvrstID,KategorijaID")] Knjiga knjiga)
        {
            if (id != knjiga.KnjigaId)
            {
                return NotFound();
            }

            // Check if the current user is in the "Administrator" or "Manager" role
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(knjiga);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KnjigaExists(knjiga.KnjigaId))
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
                ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "AvtorID", knjiga.AvtorID);
                ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID", knjiga.KategorijaID);
                ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID", knjiga.ZvrstID);
                return View(knjiga);
            }
            else
            {
                // Redirect to an unauthorized page or return a specific view
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

        // GET: Knjiga/Delete/5
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Knjige
                .Include(k => k.Avtor)
                .Include(k => k.Kategorija)
                .Include(k => k.Zvrst)
                .FirstOrDefaultAsync(m => m.KnjigaId == id);
            if (knjiga == null)
            {
                return NotFound();
            }

            // Check if the current user is in the "Administrator" or "Manager" role
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                return View(knjiga);
            }
            else
            {
                // Redirect to an unauthorized page or return a specific view
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

        // POST: Knjiga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Check if the current user is in the "Administrator" or "Manager" role
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                var knjiga = await _context.Knjige.FindAsync(id);
                if (knjiga != null)
                {
                    _context.Knjige.Remove(knjiga);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Redirect to an unauthorized page or return a specific view
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

        private bool KnjigaExists(int id)
        {
            return _context.Knjige.Any(e => e.KnjigaId == id);
        }
    }
}
