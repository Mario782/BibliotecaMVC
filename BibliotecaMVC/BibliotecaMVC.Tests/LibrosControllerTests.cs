using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using BibliotecaMVC.Controllers;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Tests
{
    public class LibrosControllerTests
    {
        private ApplicationDbContext GetInMemoryDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public void Index_ReturnsViewResult_ConListaDeLibros()
        {
            // Arrange (Preparar)
            var dbName = "DbIndexTest";
            using var context = GetInMemoryDbContext(dbName);
            context.Libros.Add(new Libro { Id = 1, Titulo = "Libro 1", Autor = "Autor 1", Genero = "Drama", AnioPublicacion = 2001 });
            context.Libros.Add(new Libro { Id = 2, Titulo = "Libro 2", Autor = "Autor 2", Genero = "Acción", AnioPublicacion = 2002 });
            context.SaveChanges();

            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act (Ejecutar)
            var result = controller.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Libro>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
        }

        [Fact]
        public void CreatePost_ModeloValido_RedirigeAIndex()
        {
            // Arrange
            var dbName = "DbCreateValidTest";
            using var context = GetInMemoryDbContext(dbName);
            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            var nuevoLibro = new Libro { Id = 3, Titulo = "Nuevo Libro", Autor = "Nuevo Autor", Genero = "Terror", AnioPublicacion = 2023 };

            // Act
            var result = controller.Create(nuevoLibro);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, context.Libros.Count()); // Verifica que se guardó en BD
        }

        [Fact]
        public void CreatePost_ModeloInvalido_RetornaViewResultConModelo()
        {
            var dbName = "DbCreateInvalidTest";
            using var context = GetInMemoryDbContext(dbName);
            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Forzar error de validación
            controller.ModelState.AddModelError("Titulo", "El título es obligatorio");
            var libroInvalido = new Libro { Autor = "Autor sin titulo" };

            // Act
            var result = controller.Create(libroInvalido);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<Libro>(viewResult.Model);
            Assert.Empty(context.Libros); // No se debió guardar nada en BD
        }

        [Fact]
        public void EditGet_IdNulo_RetornaNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext("DbEditNullTest");
            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act
            var result = controller.Edit((int?)null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void EditPost_ConflictoDeIds_RetornaNotFound()
        {
            // Arrange
            using var context = GetInMemoryDbContext("DbEditConflictTest");
            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            var libro = new Libro { Id = 1, Titulo = "Test" };

            // Act (Se envía ID 2 en la ruta, pero el libro es ID 1)
            var result = controller.Edit(2, libro);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteConfirmed_EliminaLibro_RedirigeAIndex()
        {
            // Arrange
            var dbName = "DbDeleteTest";
            using var context = GetInMemoryDbContext(dbName);
            var libro = new Libro { Id = 10, Titulo = "Libro a eliminar", Autor = "Autor X", Genero = "Comedia", AnioPublicacion = 2020 };
            context.Libros.Add(libro);
            context.SaveChanges();

            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act
            var result = controller.DeleteConfirmed(10);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Empty(context.Libros); // Verifica que la BD quedó vacía
        }

        [Fact]
        public void DeleteGet_IdInexistente_RetornaNotFound()
        {
            var dbName = "DbDeleteNotFoundTest";
            using var context = GetInMemoryDbContext(dbName);
            var mockLogger = new Mock<ILogger<LibrosController>>();
            var controller = new LibrosController(context, mockLogger.Object);

            // Act (Buscar ID 99 que no existe)
            var result = controller.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}