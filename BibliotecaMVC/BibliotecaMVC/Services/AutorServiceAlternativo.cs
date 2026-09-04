using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public class AutorServiceAlternativo : IAutorService
    {
        private static List<Autor> autores = new List<Autor>
        {
            new Autor { ID = 1, Nombre = "Stephen", Apellido = "King", Nacionalidad = "Estadounidense", FechaNacimiento = new DateTime(1947, 9, 21), Activo = true },
            new Autor { ID = 2, Nombre = "J.K.", Apellido = "Rowling", Nacionalidad = "Británica", FechaNacimiento = new DateTime(1965, 7, 31), Activo = true },
            new Autor { ID = 3, Nombre = "George", Apellido = "Orwell", Nacionalidad = "Británica", FechaNacimiento = new DateTime(1903, 6, 25), Activo = false }
        };

        public List<Autor> ObtenerTodos() => autores;

        public Autor ObtenerPorId(int id) => autores.FirstOrDefault(a => a.ID == id);

        public void Actualizar(Autor autorModificado)
        {
            var autor = autores.FirstOrDefault(a => a.ID == autorModificado.ID);
            if (autor != null)
            {
                autor.Nombre = autorModificado.Nombre;
                autor.Apellido = autorModificado.Apellido;
                autor.Nacionalidad = autorModificado.Nacionalidad;
                autor.Activo = autorModificado.Activo;
            }
        }

        public void Eliminar(int id)
        {
            var autor = autores.FirstOrDefault(a => a.ID == id);
            if (autor != null) autores.Remove(autor);
        }
    }
}