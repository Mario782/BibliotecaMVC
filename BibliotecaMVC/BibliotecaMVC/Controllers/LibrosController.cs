using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    [Authorize] 
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


        [Authorize(Roles = "Administrador")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var libro = _context.Libros.Find(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
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

        [Authorize(Roles = "Administrador")]
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var libro = _context.Libros.Find(id);
            if (libro == null) return NotFound();
            return View(libro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
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