using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public class AutorService : IAutorService
    {
        // Trasladamos tu lista estática exacta aquí
        private static List<Autor> autores = new List<Autor>
        {
            new Autor { ID = 1, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
            new Autor { ID = 2, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = false },
            new Autor { ID = 3, Nombre = "Julio", Apellido = "Cortázar", Nacionalidad = "Argentina", FechaNacimiento = new DateTime(1914, 8, 26), Activo = false },
            new Autor { ID = 4, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true },
            new Autor { ID = 5, Nombre = "Laura", Apellido = "Esquivel", Nacionalidad = "Mexicana", FechaNacimiento = new DateTime(1950, 9, 30), Activo = true }
        };

        public List<Autor> ObtenerTodos()
        {
            return autores;
        }

        public Autor ObtenerPorId(int id)
        {
            return autores.FirstOrDefault(a => a.ID == id);
        }

        public void Actualizar(Autor autorModificado)
        {
            var autor = autores.FirstOrDefault(a => a.ID == autorModificado.ID);
            if (autor != null)
            {
                autor.Nombre = autorModificado.Nombre;
                autor.Apellido = autorModificado.Apellido;
                autor.Nacionalidad = autorModificado.Nacionalidad;
                autor.FechaNacimiento = autorModificado.FechaNacimiento;
                autor.Activo = autorModificado.Activo;
            }
        }

        public void Eliminar(int id)
        {
            var autor = autores.FirstOrDefault(a => a.ID == id);
            if (autor != null)
            {
                autores.Remove(autor);
            }
        }
    }
}