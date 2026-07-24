using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class LibrosController : Controller
    {
        public IActionResult Index()
        {
            List<Libro> libros = new List<Libro>()
            {
                new Libro
                {
                    ID = 1,
                    Titulo = "Clean Code",
                    Autor = "Robert Martin",
                    Categoria = "Programación",
                    Precio = 35.5M,
                    Disponible = true,
                },
                new Libro
                {
                    ID = 2,
                    Titulo = "Cien años de soledad",
                    Autor = "Gabriel Garcia Márquez",
                    Categoria = "Literatura",
                    Precio = 18M,
                    Disponible = false,
                }
            };
            ViewBag.Nombre = "Carlos Rivera";
            ViewBag.Libros = libros;

            return View();
        }
    }
}
