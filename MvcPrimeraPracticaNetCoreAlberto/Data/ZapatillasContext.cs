using Microsoft.EntityFrameworkCore;
using MvcPrimeraPracticaNetCoreAlberto.Models;

namespace MvcPrimeraPracticaNetCoreAlberto.Data
{
    public class ZapatillasContext : DbContext
    {
        public ZapatillasContext(DbContextOptions<ZapatillasContext> options): base(options) { }

        public DbSet<Zapatillas> Zapatillas { get; set; }
        public DbSet<ImagenesZapas> ImagenesZapas { get; set; }

    }
}
