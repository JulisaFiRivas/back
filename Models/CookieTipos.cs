using System.ComponentModel.DataAnnotations;

namespace WebCookie.Models
{
    public class CookieTipos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string NombreTipo { get; set; }

        [Required]
        [MaxLength(1000)]
        public string DescripcionTipo { get; set; }
    }
}
