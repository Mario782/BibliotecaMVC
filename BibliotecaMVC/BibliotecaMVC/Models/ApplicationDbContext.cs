using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Aquí agregamos el DbSet para los Libros
        public DbSet<Libro> Libros { get; set; }
    }
}