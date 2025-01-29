using Microsoft.EntityFrameworkCore;
using WebCookie.Models;

namespace WebCookie.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options){ }

        public DbSet<Cookie> Cookies { get; set; }

    }
}
