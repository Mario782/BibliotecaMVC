using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        private static List<Autor> autores = new List<Autor>
        {
            new Autor { ID = 1, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
            new Autor { ID = 2, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = false },
            new Autor { ID = 3, Nombre = "Julio", Apellido = "Cortázar", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1914, 8, 26), Activo = false },
            new Autor { ID = 4, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true },
            new Autor { ID = 5, Nombre = "Laura", Apellido = "Esquivel", Nacionalidad = "Mexicana", FechaNacimiento = new DateTime(1950, 9, 30), Activo = true }
        };

        public IActionResult Index()
        {
            return View(autores);
        }

        public IActionResult Edit(int id)
        {
            var autor = autores.FirstOrDefault(a => a.ID == id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost]
        public IActionResult Edit(Autor autorModificado)
        {
            var autor = autores.FirstOrDefault(a => a.ID == autorModificado.ID);
            if (autor != null)
            {
                autor.Nombre = autorModificado.Nombre;
                autor.Apellido = autorModificado.Apellido;
                autor.Nacionalidad = autorModificado.Nacionalidad;
                autor.FechaNacimiento = autorModificado.FechaNacimiento;
                autor.Activo = autorModificado.Activo;
                return RedirectToAction("Index");
            }
            return View(autorModificado);
        }

        public IActionResult Delete(int id)
        {
            var autor = autores.FirstOrDefault(a => a.ID == id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var autor = autores.FirstOrDefault(a => a.ID == id);
            if (autor != null) autores.Remove(autor);
            return RedirectToAction("Index");
        }
    }
}