using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    public class AppDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; } //Products EFCore migrations tarafından veritabanında oluşturacağı tablonun ismi olacak.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DbContextInitializer.Build();
            optionsBuilder.UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
        }

        //!!! Buradaki ChangeTracker savechanges den sürekli önce  çalışacağı için saveChanges methodunu override edip bu kod bloğunu oraya taşıdık ki kod kalabalığı olmasın.!!!

        public override int SaveChanges()
        {
            ChangeTracker.Entries().ToList().ForEach(e => //Track edilen entities foreach ile döndürülüyor e değerine atılıyor.
            {
                if (e.Entity is Product product) // listedeki entities arasındakiler  Product ile yakala eğer Product ise product olarak ata eğer değilse false dön.
                {
                    // product.Stock = 500;
                    // Console.WriteLine($"{product.Id} :{product.Name} -{product.Price} -{product.Stock}");

                    //_context.SaveChanges methodunun çalışma prensibi memoryde tutulan entity stateine bağlı eğer entity ADDED ise eklemeyei yapar
                    if (e.State == EntityState.Added)                                  //bu sebeple createdDate set işlemini bu entities state ADDED ise set edip güncel tarihi atıyoruz.
                    {
                        product.CreatedDate = DateTime.Now;
                    }
                }
            });

            //DAHA SONRA YİNE BASECLASS saveChanges methodunu çağırıyoruz ama öncesinde sürekli bu işlemi yaptığımız için methodu buraya taşıdık.
            //SaveChanges methodu her çağırıldığında Product nesnesinin her instanceina CreatedDate properties eklemesi otomatik olarak yapılacak.
            return base.SaveChanges();
        }


    }
}
