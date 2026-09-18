using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio.")]
        [StringLength(100, ErrorMessage = "El título no puede exceder los 100 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es obligatorio.")]
        public string Autor { get; set; }

        [Required(ErrorMessage = "El género es obligatorio.")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El año de publicación es obligatorio.")]
        [Range(1000, 2100, ErrorMessage = "Ingrese un año válido.")]
        [Display(Name = "Año de Publicación")]
        public int AnioPublicacion { get; set; }
    }
}