using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LibrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var libros = _context.Libros.ToList();
            return View(libros);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                _context.Libros.Add(libro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var libro = _context.Libros.Find(id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Libro libro)
        {
            if (id != libro.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Libros.Update(libro);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(libro);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            // Uso del método Find() según la indicación
            var libro = _context.Libros.Find(id);
            if (libro == null) return NotFound();

            return View(libro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = _context.Libros.Find(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}