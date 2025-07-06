using Microsoft.EntityFrameworkCore;
using WebUygulamaProje1.Models;

namespace WebUygulamaProje1.Utility
{
    public class UygulamaDbContext : DbContext
    {
        public UygulamaDbContext(DbContextOptions<UygulamaDbContext> options) : base(options) { }
        
        public DbSet<KitapTuru> KitapTurleri { get; set; }

        public DbSet<Kitaplar> Kitaplar {  get; set; }

        public DbSet<Kiralama> Kiralama { get; set; }

    }
}
