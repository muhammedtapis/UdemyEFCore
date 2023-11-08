using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.DatabaseFirst.DataAccessLayer
{
    public class AppDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; } // önce veritabanında tablo oluşturduk sonra burada kod ile bağlıycaz bu sebeple properties ismi
                                                     // veritabanı tablosuyla aynı verirsen EFcore kendisi eşleştiricek.
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)  //veritabanı ile ilgili tüm ayarları DbContextOptions sınıf üzerinde yapıcaz.
        { 
            //farklı veritabanlarına bağlancağın zaman bir projede bu ctor kullanıyosun ki her çağırdığında bağlantılarını versin.
        }

        public AppDbContext()
        {
                
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
            
        }
    }

}
