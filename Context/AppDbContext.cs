using Microsoft.EntityFrameworkCore;
using WebCookie.Models;

namespace WebCookie.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){ }
        //comentrio para saber si el git esta funcioinado
        public DbSet<Cookie> Cookies { get; set; }

    }
}
