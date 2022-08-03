using Microsoft.EntityFrameworkCore;

namespace AlbumFotos.Models
{
    public class Context : DbContext
    {
        public DbSet<Album> Albuns { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
        public Context(DbContextOptions options) : base(options)
        {
        }
    }
}
