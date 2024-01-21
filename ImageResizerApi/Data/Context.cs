using ImageResizerApi.Model;
using Microsoft.EntityFrameworkCore;

namespace ImageResizerApi.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions options): base(options) { }
        public DbSet<Image> Images { get; set; }
        public DbSet<ImageSizings> ImageSizings { get; set; }

    }
}
