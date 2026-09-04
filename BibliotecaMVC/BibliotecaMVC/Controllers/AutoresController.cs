using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;
using BibliotecaMVC.Services; // Agregamos la referencia a nuestra carpeta Services

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        private readonly IAutorService _autorService;

        // Inyección de Dependencias a través del constructor
        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService;
        }

        public IActionResult Index()
        {
            var autores = _autorService.ObtenerTodos();
            return View(autores);
        }

        public IActionResult Edit(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost]
        public IActionResult Edit(Autor autorModificado)
        {
            _autorService.Actualizar(autorModificado);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var autor = _autorService.ObtenerPorId(id);
            if (autor == null) return NotFound();
            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _autorService.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}