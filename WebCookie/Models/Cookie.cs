using System.ComponentModel.DataAnnotations;

namespace WebCookie.Models
{
    public class Cookie
    {
        [Key]
        public int Id {  get; set; }
     
        [Required]
        [MaxLength(40)]
        public string Nombre { get; set; } 

        [Required]
        [MaxLength(1000)]
        public string Descripcion { get; set; } 

    }
}
