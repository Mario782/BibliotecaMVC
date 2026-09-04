using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    public interface IAutorService
    {
        List<Autor> ObtenerTodos();
        Autor ObtenerPorId(int id);
        void Actualizar(Autor autor);
        void Eliminar(int id);
    }
}