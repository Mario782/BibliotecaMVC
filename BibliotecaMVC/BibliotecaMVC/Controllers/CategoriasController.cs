using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using BibliotecaMVC.Models;

namespace BibliotecaMVC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly string _conexion;

        public CategoriasController(IConfiguration config)
        {
            _conexion = config.GetConnectionString("ConexionSQL");
        }

        public IActionResult Index()
        {
            List<Categoria> lista = new List<Categoria>();
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
        public IActionResult Edit(int id)
        {
            Categoria categoria = new Categoria();
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
                }
            }
            return View(categoria);
        }

        [HttpPost]
        public IActionResult Edit(Categoria categoria)
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
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Categoria categoria = new Categoria();
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
                }
            }
            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection con = new SqlConnection(_conexion))
            {
                string query = "DELETE FROM Categorias WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}