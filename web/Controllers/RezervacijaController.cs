using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;
namespace web.Controllers
{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly SchoolContext _context;
private readonly UserManager<ApplicationUser> _usermanager;
       public RezervacijaController(SchoolContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

        // GET: Rezervacija
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Rezervacije.Include(r => r.Knjiga);
            return View(await schoolContext.ToListAsync());
        }

        // GET: Rezervacija/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Knjiga)
                .FirstOrDefaultAsync(m => m.RezervacijaId == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // GET: Rezervacija/Create
        public IActionResult Create()
        {
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId");
            return View();
        }

        // POST: Rezervacija/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RezervacijaId,datumPrevzema,datumZapadlosti,KnjigaId,DateEdited,DateCreated")] Rezervacija rezervacija)
        {
           var currentUser = await _usermanager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                rezervacija.DateCreated = DateTime.Now;
                rezervacija.DateEdited = DateTime.Now;
                rezervacija.Owner = currentUser;
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId", rezervacija.KnjigaId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId", rezervacija.KnjigaId);
            return View(rezervacija);
        }

        // POST: Rezervacija/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RezervacijaId,datumPrevzema,datumZapadlosti,KnjigaId,DateEdited,DateCreated")] Rezervacija rezervacija)
        {
            if (id != rezervacija.RezervacijaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.RezervacijaId))
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
            ViewData["KnjigaId"] = new SelectList(_context.Knjige, "KnjigaId", "KnjigaId", rezervacija.KnjigaId);
            return View(rezervacija);
        }

        // GET: Rezervacija/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Knjiga)
                .FirstOrDefaultAsync(m => m.RezervacijaId == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        // POST: Rezervacija/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.RezervacijaId == id);
        }
    }
}
