using Microsoft.EntityFrameworkCore;

namespace Concurrency.Web.Models
{
    public class AppDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; }  //database için oluşacak tablo.
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)  //method override et
        {
            //FLUENT API

            modelBuilder.Entity<Product>().Property(x => x.RowVersion).IsRowVersion(); //bu alanı efcore a tanıttık row version olcak. Eğer bu olmazsa default olarak diğer transaction üstüne yazar.
            modelBuilder.Entity<Product>().Property(x => x.Price).HasPrecision(18,2); //decimal alanın tanımlama tipi


            base.OnModelCreating(modelBuilder);

        }
    }
}
