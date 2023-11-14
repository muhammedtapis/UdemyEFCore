using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyEFCore.CodeFirst.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
   
    public class AppDbContext : DbContext
    {
        //dışardan değer alıp AppDbContexte gönderceğin zaman 
        private readonly int Barcode;

        //bu değeri constructorda gönder.
        public AppDbContext(int barcode)
        {
            Barcode = barcode;
        }

        public AppDbContext()
        {
            
        }

        public DbSet<Product> Products { get; set; } //Products EFCore migrations tarafından veritabanında oluşturacağı tablonun ismi olacak.
        public DbSet<Category> Categories { get; set; } //Categories EFCore migrations tarafından veritabanında oluşturacağı tablonun ismi olacak.
                                                        // Bunu eklemezsen AppDbcontextten bu tabloya erişim sağlayıp işlem yapamazsın.
        public DbSet<ProductFeature> ProductFeatures { get; set; }

        //alttaki ikisi many to many için
        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }

        //alttaki ikisi EfCore Inheritance için

        public DbSet<Manager> Managers { get; set; }
        public DbSet<Employee> Employees { get; set; }

        //bir de BASECLASS yani BasePerson eklemesi yaptığımızda EFCore davranışı değişiyor.
        //Migration ettiğimizde databasede sadece Persons tablosu oluşacak Employees ve Managers tablosu eklenmeyecek.
        public DbSet<BasePerson> Persons { get; set; }  //OwnedEntity Tipi olarak kullanacaksan bu baseclass kalkacak burda olmayacak. ve  bu classtan miras alma durumu olmaycak.

        public  DbSet<ProductFull> ProductFulls { get; set; } //Keyless Tipi için oluşturuldu

        public DbSet<Person> People { get; set; }  //Query kısmı için oluşturduk

        public DbSet<ProductEssential> ProductEssentials { get; set; }  // raw sql için oluşturuldu bu entity sorgularımızı karşılayacağımız sınıflardan birisi.

        public DbSet<ProductWithFeature> ProductWithFeatures { get; set; }

        public DbSet<ProductAll> ProductAlls { get; set; }    //ToView için oluşturduk sqlde oluşturduğmuz view bunda karşılıcaz
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Trace-Debug-Information-Warning-Error-Critical loglama sıralaması information ve solundakiler loglanacak.

            DbContextInitializer.Build();
            //optionsBuilder.LogTo(Console.WriteLine,LogLevel.Information).UseLazyLoadingProxies().UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
            //lazy loading kullanacaksan önce efcoreproxies kütüphanesini indir sonra burada UseLazyLoadingProxies() methodunu çağır
            //ondan önce de bu olayı anlayabilmek için loglama yapmasını istedik console sadece information olanları yazdırcak

            optionsBuilder.LogTo(Console.WriteLine,LogLevel.Information).UseSqlServer(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));
        }


        //Product entity de yapılan conf. nazaran buradakiler direkt veritabanını etkiler.
     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //YAPILAN HER MODEL DEĞİŞİKLİĞİNDE MIGRATION YAPMAN GEREKİYOR YOKSA VERITABANI VE VS BAGLANTISI OLMAZ

            //< ---------------DATABASE - GENERATED - ATTRIBUTES-------------- >

            //modelBuilder.Entity<Product>().Property(x => x.PriceStock).HasComputedColumnSql("[Price]*[Stock]"); //iki propun çarpma işlemini sqle bıraktık.Property tanımında generate ifadesini kullan.
            //modelBuilder.Entity<Product>().Property(x => x.PriceStock).ValueGeneratedOnAddOrUpdate();   //Attributes ta Computed denk gelir. bunları Entity içinde tanımlamazsan burda yapcaksın
            //modelBuilder.Entity<Product>().Property(x => x.PriceStock).ValueGeneratedOnAdd();  //Attributes ta Identity denk gelir
            //modelBuilder.Entity<Product>().Property(x => x.PriceStock).ValueGeneratedNever(); //Attributes ta None denk gelir

            //< ---------------DATABASE - GENERATED - ATTRIBUTES-------------- >

            //< ---------------DATA - DELETE - BEHAVIORS-------------- >

            //modelBuilder.Entity<Category>().HasMany(x=> x.Products).WithOne(x=>x.Category).HasForeignKey(x=>x.CategoryId)
            //    .OnDelete(DeleteBehavior.Cascade); //default davranış olarak cascade verdik parenttan satır silinirse childdaki ona bağlı productlar silinir.

            //modelBuilder.Entity<Category>().HasMany(x => x.Products).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId)
            //    .OnDelete(DeleteBehavior.Restrict); //default davranış olarak restrict verdik parenttan satır silinirse childdaki ona bağlı productlar silinmez.

            //modelBuilder.Entity<Category>().HasMany(x => x.Products).WithOne(x => x.Category).HasForeignKey(x => x.CategoryId)
            //    .OnDelete(DeleteBehavior.SetNull); //default davranış olarak setnull verdik parenttan satır silinirse childdaki ona bağlı productlardaki foreign key null olur.


            //< ---------------DATA - DELETE - BEHAVIORS-------------- >

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

            //<---------------RELATIONS WITH FLUENT-API-------------->

            //<---------------TPT TABLE PER TYPE-------------->

            //modelBuilder.Entity<BasePerson>().ToTable("Persons");  //her entity karşı tablo oluşturcaz. ilişkiyi kendisi oluşturuyor. 
            //modelBuilder.Entity<Employee>().ToTable("Employees");
            //modelBuilder.Entity<Manager>().ToTable("Managers");

            //<---------------TPT TABLE PER TYPE-------------->

            //<---------------OWNED ENTITY TYPES-------------->

            //modelBuilder.Entity<Manager>().OwnsOne(x => x.Person, p =>
            //{
            //    p.Property(x => x.FirstName).HasColumnName("FirstName"); // oluşan tabloda default Person_FirstName olur
            //    p.Property(x => x.LastName).HasColumnName("LastName"); // oluşan tabloda default Person_LastName olur
            //    p.Property(x => x.Age).HasColumnName("Age"); // oluşan tabloda default Person_Age olur bunu değiştirebiliyoruz.


            //});

            //modelBuilder.Entity<Employee>().OwnsOne(x => x.Person, p =>  //bu ikinci lambda p si persona denk geliyor ilk lambda x ise Employee 
            //{
            //    p.Property(x => x.FirstName).HasColumnName("FirstName"); // oluşan tabloda default Person_FirstName olur
            //    p.Property(x => x.LastName).HasColumnName("LastName"); // oluşan tabloda default Person_LastName olur
            //    p.Property(x => x.Age).HasColumnName("Age"); // oluşan tabloda default Person_Age olur bunu değiştirebiliyoruz.


            //});

            //<---------------OWNED ENTITY TYPES-------------->

            //<---------------KEYLESS ENTITY TYPES-------------->
            //modelBuilder.Entity<ProductFull>().HasNoKey();               //bu entitynin keyless olduğunu belirttik.

            //<---------------KEYLESS ENTITY TYPES-------------->

            //<---------------ENTITY PROPERTIES-------------->

            //modelBuilder.Entity<Product>().Ignore(x => x.Barcode); //product entitysindeki barcode propertysini Maplemek için ignore et
            //modelBuilder.Entity<Product>().Property(x => x.Name).IsUnicode(false); // bu da [Unicode(false)]:varchar örneği
            //modelBuilder.Entity<Product>().Property(x => x.Url).HasColumnType("varchar(500)").HasColumnName("ProductUrl");

            //<---------------ENTITY PROPERTIES-------------->


            //<---------------INDEKSLEME-------------->

            //modelBuilder.Entity<Product>().HasIndex(x => x.Name);  //product entitysinin Name propu indekslendi 
            //modelBuilder.Entity<Product>().HasIndex(x => new { x.Name, x.Price }).HasDatabaseName("NAMEPRICE");  //composed indexleme örneği iki prop için tek index oluşturduk.
            //modelBuilder.Entity<Product>().HasIndex(x => x.Name).IncludeProperties(x => new { x.Price, x.Stock }); //Name göre yapılam sorguda name,price,stock istiyorsak bu şekilde
            //indeksleme yapmalıyız.o indekse hangi propları eklemek istiyorsak onu belirtiyoruz

            //Check Constraint
            //modelBuilder.Entity<Product>().HasCheckConstraint("PriceDiscountCheck", "[Price] > [DiscountPrice]"); //dbde price discount pricetan büyük olamaz.
            //< ---------------INDEKSLEME-------------- >


            //< ---------------QUERY-------------- >

            //rawSqlCustom
            //modelBuilder.Entity<ProductEssential>().HasNoKey(); //SQL sorgusunu karşılamak için kullancağımız entity sorguda id yoksa entityden de kaldırmak gerekir.
            modelBuilder.Entity<ProductWithFeature>().HasNoKey();  //entity de id var ama bu primary key olsun istemiyoruz EFCORE primary key olarak anlarsa bunu track etmeye çalışır.

            //ToSqlQuery kullanımı merkezi yerden sql kodu yazma
            modelBuilder.Entity<ProductEssential>().HasNoKey().ToSqlQuery("select Name,Price from Products"); //merkezi sorgu program.cs te ToList(); dendiğinde çalışacak kod.

            //toView methodu için özel sorgular özel viewlar görmek istediimizde bu viewları sqlde oluşturup burada belirtebiliriz.
            modelBuilder.Entity<ProductAll>().HasNoKey().ToView("productWithFeature"); //databasede view klasöründeki oluşturduğumz view ismini verdik.
            //< ---------------QUERY-------------- >


            //< ---------------INNER JOIN-------------- > iki tablo arasındaki ortak alanları almak istediğimizde kullanılan join tipi



            //< ---------------INNER JOIN-------------- >

            //< ---------------GLOBAL QUERY FILTER-------------- >

            modelBuilder.Entity<Product>().Property(x => x.IsDeleted).HasDefaultValue(false); //product entitysinde oluşturduğumuz IsDeleted a default değer atadık.
            //modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsDeleted==false); //productla ilglil yaptığımız her sorguda where koşulunda  bu is deleted kısmı da eklenecek

         if (Barcode != default(int))  //integerin default değeri olan 0 a eşit değilse
            {
                modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsDeleted == false && x.Barcode ==Barcode); //hem isdeleted false ise hem de kullanıcıdan gelen Barcode x.Barcode eşitse
            }
            else
            {
                modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsDeleted == false); //eğer ctor boş ise direkt Isdeleted false olanları getir
            }
            //< ---------------GLOBAL QUERY FILTER-------------- >
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
