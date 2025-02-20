using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace WebCookie.Models
{
    public class AppUser: IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }

        [PersonalData]
        public DateTime UltimaConexion { get; set; }

    }
}
