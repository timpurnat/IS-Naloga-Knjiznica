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
        // GET: Knjiga
// GET: Knjiga
public async Task<IActionResult> Index(string searchString, string sortOrder, int? page)
{
    // Set the number of items per page
    int pageSize = 3;

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
            knjige = knjige.OrderBy(k => k.Naslov);
            break;
        case "naslov_desc":
            knjige = knjige.OrderByDescending(k => k.Naslov);
            break;
        case "avtor":
            knjige = knjige.OrderBy(k => k.Avtor.PriimekIme);
            break;
        case "avtor_desc":
            knjige = knjige.OrderByDescending(k => k.Avtor.PriimekIme);
            break;
        case "zvrst":
            knjige = knjige.OrderBy(k => k.Zvrst.ImeZvrsti);
            break;
        case "zvrst_desc":
            knjige = knjige.OrderByDescending(k => k.Zvrst.ImeZvrsti);
            break;
        case "kategorija":
            knjige = knjige.OrderBy(k => k.Kategorija.imeKategorije);
            break;
        case "kategorija_desc":
            knjige = knjige.OrderByDescending(k => k.Kategorija.imeKategorije);
            break;
        case "ocena":
            knjige = knjige.OrderBy(k => k.Ocena);
            break;
        case "ocena_desc":
            knjige = knjige.OrderByDescending(k => k.Ocena);
            break;
        default:
            knjige = knjige.OrderBy(k => k.Naslov);
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

        // GET: Knjiga/Create
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Create()
        {
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "AvtorID");
            ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID");
            ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID");
            return View();
        }

        // POST: Knjiga/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KnjigaId,Naslov,Ocena,AvtorID,ZvrstID,KategorijaID")] Knjiga knjiga)
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
            ViewData["AvtorID"] = new SelectList(_context.Avtorji, "AvtorID", "AvtorID", knjiga.AvtorID);
            ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID", knjiga.KategorijaID);
            ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID", knjiga.ZvrstID);
            return View(knjiga);
        }

        // POST: Knjiga/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KnjigaId,Naslov,Ocena,AvtorID,ZvrstID,KategorijaID")] Knjiga knjiga)
        {
            if (id != knjiga.KnjigaId)
            {
                return NotFound();
            }

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

        // GET: Knjiga/Delete/5
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

            return View(knjiga);
        }

        // POST: Knjiga/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var knjiga = await _context.Knjige.FindAsync(id);
            if (knjiga != null)
            {
                _context.Knjige.Remove(knjiga);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KnjigaExists(int id)
        {
            return _context.Knjige.Any(e => e.KnjigaId == id);
        }
    }
}