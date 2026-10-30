using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BibliotecaMVC.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BibliotecaMVC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _conexion;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IConfiguration config, ILogger<CategoriasController> logger)
        {
            _conexion = config.GetConnectionString("ConexionSQL");
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Usuario consultó el listado general de Categorías.");
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    string query = "SELECT Id, Nombre, Descripcion FROM Categorias";
                    SqlCommand cmd = new SqlCommand(query, con);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Categoria
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Descripcion = reader["Descripcion"].ToString()
                            });
                        }
                    }
                }
                return View(lista);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al intentar cargar la lista de categorías desde SQL.");
                throw;
            }
        }

        public IActionResult Edit(int id)
        {
            _logger.LogInformation("Usuario ingresó a la vista de edición para la categoría ID: {Id}", id);
            Categoria categoria = new Categoria();

            try
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    string query = "SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoria.Id = Convert.ToInt32(reader["Id"]);
                            categoria.Nombre = reader["Nombre"].ToString();
                            categoria.Descripcion = reader["Descripcion"].ToString();
                        }
                        else
                        {
                            _logger.LogWarning("Intento de editar una categoría inexistente. ID buscado: {Id}", id);
                        }
                    }
                }
                return View(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de la categoría ID: {Id} para editar.", id);
                throw;
            }
        }

        [HttpPost]
        public IActionResult Edit(Categoria categoria)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    string query = "UPDATE Categorias SET Nombre = @Nombre, Descripcion = @Descripcion WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", categoria.Id);
                    cmd.Parameters.AddWithValue("@Nombre", categoria.Nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", categoria.Descripcion);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                _logger.LogInformation("Se actualizó exitosamente la categoría ID: {Id}", categoria.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al intentar guardar los cambios de la categoría ID: {Id}.", categoria.Id);
                throw;
            }
        }

        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Usuario ingresó a la vista de confirmación de eliminación para la categoría ID: {Id}", id);
            Categoria categoria = new Categoria();

            try
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    string query = "SELECT Id, Nombre, Descripcion FROM Categorias WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            categoria.Id = Convert.ToInt32(reader["Id"]);
                            categoria.Nombre = reader["Nombre"].ToString();
                            categoria.Descripcion = reader["Descripcion"].ToString();
                        }
                        else
                        {
                            _logger.LogWarning("Intento de eliminar una categoría inexistente. ID buscado: {Id}", id);
                        }
                    }
                }
                return View(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos de la categoría ID: {Id} para eliminar.", id);
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_conexion))
                {
                    string query = "DELETE FROM Categorias WHERE Id = @Id";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                _logger.LogInformation("Se eliminó definitivamente la categoría ID: {Id}", id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error crítico al intentar eliminar de la base de datos la categoría ID: {Id}.", id);
                throw;
            }
        }
    }
}