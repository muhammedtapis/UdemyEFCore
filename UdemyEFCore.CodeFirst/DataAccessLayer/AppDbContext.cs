using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    public class AppDbContext:DbContext
    {
        public DbSet<Product> Products { get; set; } //Products EFCore migrations tarafından veritabanında oluşturacağı tablonun ismi olacak.
        public DbSet<Category> Categories { get; set; } //Categories EFCore migrations tarafından veritabanında oluşturacağı tablonun ismi olacak.
                                                        // Bunu eklemezsen AppDbcontextten bu tabloya erişim sağlayıp işlem yapamazsın.
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        //alttaki ikisi many to many için
        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            DbContextInitializer.Build();
            optionsBuilder.UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
        }
        

        //Product entity de yapılan conf. nazaran buradakiler direkt veritabanını etkiler.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //<---------------EFCORE a uygun tanımlamalar proplar yazmayınca FLUENT-API kullanıyoruz.-------------->

            //<---------------CONFIGURATION WITH FLUENT-API-------------->
            //modelBuilder.Entity<Product>().ToTable("ProductTb", "productstb"); //product entitisindeki conf. ile aynı işi yapar ama bu daha iyi
            //modelBuilder.Entity<Product>().HasKey(p => p.Product_Id); //Product Entity içinde yaptığımız data annotaton attr. ile aynı iş bu FluentAPI product_Id primary key olarak atar.
            //modelBuilder.Entity<Product>().Property(x=> x.Name).IsRequired(); // validation tarafında bize fayda sağlar ve bu alanın required eder.
            //modelBuilder.Entity<Product>().Property(x => x.Name).HasMaxLength(50).IsFixedLength(); //IsFixedLength en fazla ve en az 50  karakter olcağın belirtir.
            //<---------------CONFIGURATION WITH FLUENT-API-------------->

            //<---------------RELATIONS WITH FLUENT-API--------------> 

            //HER ZAMAN HAS ILE BAŞLA!!!!!!!!!!!!
            //<---------------ONE TO MANY-------------->
            //modelBuilder.Entity<Category>().HasMany(x => x.Products).WithOne(x => x.Category).HasForeignKey(x => x.Category_Id); //BU KODU EFCORE senin verdiğin idyi örnek olarak Category_Id tanımazsa yazcaz
            //ÖNEMLİ!!  Bir kategorinin birden fazla product olabilir demek daha sonra products ın ise bir kategorisi olabilir ve bu productın foreign key Category_Id olacaktır.
            //<---------------ONE TO MANY-------------->

            //<---------------ONE TO ONE-------------->
            //modelBuilder.Entity<Product>().HasOne(x => x.ProductFeature).WithOne(x => x.Product).HasForeignKey<ProductFeature>(x => x.Product_Id);
            //Product entitysi one to one bir ilişkiye sahip productFEature ile productfeature da aynı ilişkiye sahip ve foreignkey generic olarak<ProductFeature> alıyor one to one da.
            //modelBuilder.Entity<Product>().HasOne(x => x.ProductFeature).WithOne(x => x.Product).HasForeignKey<ProductFeature>(x => x.Id);
            //burada ise iki üstteki koddan farklı olarak ben productfeature tablomda tekrardan Product_Id olsun istemiyorum zaten ProductFeature un Idsi de uniq o yüzden
            //productfeatureid hem primary key hem foreign key olsun id değerlerini direkt product entitysindeki id den alsın.
            //bunu EFcore kendisi yapamıyor belirtmemiz gerek bu ilişkiyi kurarken.
            //<---------------ONE TO ONE-------------->
            //<---------------RELATIONS WITH FLUENT-API-------------->

            //<---------------MANY TO MANY-------------->
            //modelBuilder.Entity<Student>()  //öğrencinin birden fazla öğretmeni öğretmenin birden fazla öğrencisi olabilir.
            //    .HasMany(x => x.Teachers)
            //    .WithMany(x => x.Students)
            //    .UsingEntity<Dictionary<string, object>>(  //aradaki yardımcı tablonun tanımlanması ve  ilişkilerinin oluşturulması.
            //    "StudentTeacherManyToMany",
            //    x => x.HasOne<Teacher>().WithMany().HasForeignKey("Teacher_Id").HasConstraintName("FK_TeacherId"), //yardımcı tablo ile Teacher tablosu iliişkisi tanımı
            //    x => x.HasOne<Student>().WithMany().HasForeignKey("Student_Id").HasConstraintName("FK_StudentId")  //yardımcı tablo ile Student tablosu iliişkisi tanımı
            //    );

            //<---------------MANY TO MANY-------------->
            base.OnModelCreating(modelBuilder);
        }

        

        //!!! Buradaki ChangeTracker savechanges den sürekli önce  çalışacağı için saveChanges methodunu override edip bu kod bloğunu oraya taşıdık ki kod kalabalığı olmasın.!!!

        //public override int SaveChanges()
        //{
        //    ChangeTracker.Entries().ToList().ForEach(e => //Track edilen entities foreach ile döndürülüyor e değerine atılıyor.
        //    {
        //        if (e.Entity is Product product) // listedeki entities arasındakiler  Product ile yakala eğer Product ise product olarak ata eğer değilse false dön.
        //        {
        //            // product.Stock = 500;
        //            // Console.WriteLine($"{product.Id} :{product.Name} -{product.Price} -{product.Stock}");

        //            //_context.SaveChanges methodunun çalışma prensibi memoryde tutulan entity stateine bağlı eğer entity ADDED ise eklemeyei yapar
        //            if (e.State == EntityState.Added)                                  //bu sebeple createdDate set işlemini bu entities state ADDED ise set edip güncel tarihi atıyoruz.
        //            {
        //                product.CreatedDate = DateTime.Now;
        //            }
        //        }
        //    });

        //    //DAHA SONRA YİNE BASECLASS saveChanges methodunu çağırıyoruz ama öncesinde sürekli bu işlemi yaptığımız için methodu buraya taşıdık.
        //    //SaveChanges methodu her çağırıldığında Product nesnesinin her instanceina CreatedDate properties eklemesi otomatik olarak yapılacak.
        //    return base.SaveChanges();
        //}


    }
}
