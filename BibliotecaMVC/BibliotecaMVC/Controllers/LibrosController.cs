using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BibliotecaMVC.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class LibrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LibrosController> _logger;

        public LibrosController(ApplicationDbContext context, ILogger<LibrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Usuario consultó el listado general de libros.");
            try
            {
                var libros = _context.Libros.ToList();
                return View(libros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al cargar la lista de libros desde la base de datos.");
                throw; // Lanzar excepción para que la capture la página de error global
            }
        }

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Create()
        {
            _logger.LogInformation("Usuario ingresó al formulario para crear un nuevo libro.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Libros.Add(libro);
                    _context.SaveChanges();
                    _logger.LogInformation("Se creó exitosamente el libro con Título: {Titulo}", libro.Titulo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar guardar el libro {Titulo} en la base de datos.", libro.Titulo);
                }
            }
            _logger.LogWarning("Intento fallido de crear un libro. El modelo no era válido.");
            return View(libro);
        }

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Se intentó acceder a Editar Libro sin proporcionar un ID.");
                return NotFound();
            }

            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                _logger.LogWarning("Intento de editar un libro inexistente. ID buscado: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Usuario ingresó a la vista de edición del libro ID: {Id}", id);
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Edit(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                _logger.LogWarning("Conflicto de IDs durante la edición de libro. ID en ruta: {RutaId}, ID en modelo: {ModeloId}", id, libro.Id);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Libros.Update(libro);
                    _context.SaveChanges();
                    _logger.LogInformation("Se actualizó correctamente el libro ID: {Id}", id);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al intentar actualizar el libro ID: {Id}", id);
                }
            }
            return View(libro);
        }

        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogWarning("Se intentó acceder a Eliminar Libro sin proporcionar un ID.");
                return NotFound();
            }

            var libro = _context.Libros.Find(id);
            if (libro == null)
            {
                _logger.LogWarning("Intento de eliminar un libro inexistente. ID buscado: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("Usuario está verificando la eliminación del libro ID: {Id}", id);
            return View(libro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Bibliotecario")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                var libro = _context.Libros.Find(id);
                if (libro != null)
                {
                    _context.Libros.Remove(libro);
                    _context.SaveChanges();
                    _logger.LogInformation("Se eliminó definitivamente el libro ID: {Id}", id);
                }
                else
                {
                    _logger.LogWarning("Se intentó confirmar la eliminación de un libro que ya no existe. ID: {Id}", id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al intentar eliminar el libro ID: {Id}", id);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}