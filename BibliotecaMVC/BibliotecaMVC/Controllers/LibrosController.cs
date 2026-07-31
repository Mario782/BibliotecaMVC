using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        private static List<Libro> libros = new List<Libro>
        {
            new Libro { ID = 1, Titulo = "Juego de Tronos", Autor = "Gorge R. R. Martin", Categoria = "Fantasía", Precio = 35.5M, Disponible = true, ImagenUrl = "Juego de Tronos.webp" },
            new Libro { ID = 2, Titulo = "La Casa del Dragon", Autor = "Gorge R. R. Martin", Categoria = "Fantasía", Precio = 18.0M, Disponible = false, ImagenUrl = "La Casa del Dragon.webp" }
        };

        public IActionResult Index() => View(libros);

        public IActionResult Details(int id)
        {
            var libro = libros.FirstOrDefault(l => l.ID == id);
            return libro == null ? NotFound() : View(libro);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Libro nuevoLibro)
        {
            nuevoLibro.ID = libros.Count > 0 ? libros.Max(l => l.ID) + 1 : 1;
            libros.Add(nuevoLibro);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var libro = libros.FirstOrDefault(l => l.ID == id);
            return libro == null ? NotFound() : View(libro);
        }

        [HttpPost]
        public IActionResult Edit(Libro libroModificado)
        {
            var libro = libros.FirstOrDefault(l => l.ID == libroModificado.ID);
            if (libro != null)
            {
                libro.Titulo = libroModificado.Titulo;
                libro.Autor = libroModificado.Autor;
                libro.Categoria = libroModificado.Categoria;
                libro.Precio = libroModificado.Precio;
                libro.Disponible = libroModificado.Disponible;
                libro.ImagenUrl = libroModificado.ImagenUrl;
                return RedirectToAction("Index");
            }
            return View(libroModificado);
        }

        public IActionResult Delete(int id)
        {
            var libro = libros.FirstOrDefault(l => l.ID == id);
            return libro == null ? NotFound() : View(libro);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var libro = libros.FirstOrDefault(l => l.ID == id);
            if (libro != null) libros.Remove(libro);
            return RedirectToAction("Index");
        }
    }
}