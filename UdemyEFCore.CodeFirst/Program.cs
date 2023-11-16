// See https://aka.ms/new-console-template for more information
using AutoMapper.QueryableExtensions;
using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UdemyEFCore.CodeFirst;
using UdemyEFCore.CodeFirst.DataAccessLayer;
using UdemyEFCore.CodeFirst.DTOs;
using UdemyEFCore.CodeFirst.Mappers;

DbContextInitializer.Build();

//<--------------MULTIPLE DBCONTEXT-------------->
var connection = new SqlConnection(DbContextInitializer.Configuration.GetConnectionString("SqlCon"));

//<--------------DBCONTEXT-------------->

async Task StatesAndTracking(AppDbContext _context)
{
    //var p1 = new Product { Name = "Kalem 4", Price = 400, Stock = 40, Barcode = 456 };

    var p1 = await _context.Products.FirstAsync();

    //p1 context.Add öncesi state
    Console.WriteLine($"ilk state :{_context.Entry(p1).State}");

    // p1.Stock = 1000;
    _context.Remove(p1);
    // _context.Entry(p1).State = EntityState.Deleted;  //bir üst satırdaki kod ile aynı işi yapıyor çünkü EF statelerle çalışıyor memory state atıyor daha sonra save changes
    //dediğimiz zaman bunları database üzerinde işliyor.
    //stateler buradaki veri ile databasedeki veri arasındaki fark gibi düşün
    //databaseden veri listeliyosan Unchanged state olur çünküğ yanı veri değiştirilmemiş.
    //veri silindikten sonra state i Detached olur çünkü öyle bir veri yok memoryde silindi gitti.

    //await _context.AddAsync(p1);
    //Console.WriteLine($"son state :{_context.Entry(p1).State}");

    //await _context.SaveChangesAsync();
    //Console.WriteLine($"save change s state :{_context.Entry(p1).State}");

    var products = await _context.Products.AsNoTracking().ToListAsync(); //veritabanından alacağımız datayı filtrelemek için bütün datayı track etmemek için AsNoTracking kullan

    products.ForEach(p =>
    {

        //var state = _context.Entry(p).State;
        Console.WriteLine($"{p.Id} :{p.Name} -{p.Price} -{p.Stock}");    //konsola yazdırma

        //bir alt satırdaki kod memory de tutulan bissürü entity olabilir milyonlarca veri olabilir.
        //rami yormamak için biz sadece bir entitye ait verileri track ederek programı daha hızlı çalışmasını sağlayabiliriz.

    });
}

void DataEkle(AppDbContext _context)
{
    _context.Products.Add(new() { Name = "Defter 1", Price = 200, Stock = 2000, Barcode = 321 });
    _context.Products.Add(new() { Name = "Defter 2", Price = 200, Stock = 2000, Barcode = 321 });
    _context.Products.Add(new() { Name = "Defter 3", Price = 200, Stock = 2000, Barcode = 321 });


    //ORTAK değer atama drumlarında mesela tarihi datetimedan alsın biz elle yazmayalım.Merkezi bir yerden yapılacak atamalar için.

    //!!! Buradaki ChangeTracker savechanges den sürekli önce  çalışacağı için saveChanges methodunu override edip bu kod bloğunu oraya taşıdık ki kod kalabalığı olmasın.!!!
    // APPDBCONTEXT classında CREATEDDATE EKLEME İŞLEMİ VAR ORAYA BAK!!!!!!!!!!


    _context.SaveChanges();
}

//< ---------------RELATIONSHIPS-------------- >
void DataAddOneToMany(AppDbContext _context)
{

    // var category = new Category() { Name = "Defterler" };

    var category = _context.Categories.First(x => x.Name == "Defterler"); //databasede hazır bulunan kategori sınıfını çekip öyle product ekleyebilirsin.

    //var product = new Product() { Name = "Defter 3", Price = 100, Stock = 200, Barcode = 123, CategoryId = category.Id };

    var product = new Product() { Name = "Kalem 1", Price = 100, Stock = 200, Barcode = 123, Category = category }; //burda gerçekleşen bizim veritabanımızda henüz category yok bu sebeple id yazamıyoruz
                                                                                                                    //                                                                                                             //yukarda instance oluşturulan category burada veriliyor.
                                                                                                                    //database kayıt yaparken bu durumda ayrıyetten _context.Category.Add(category);
                                                                                                                    //yazmamıza gerek yok çünkü bizim datamız ilişkili data EF core kendisi onu da ekliyor.
                                                                                                                    //ama instance oluşturulmuş olması gerek.

    _context.Products.Add(product);   //sadece product ekleyerek hem product hem category ekleme işlemi yaptık.PRODUCT ÜZERİNDEN CATEGORY EKLEME

    category.Products.Add(new Product() { Name = "Defterler 1", Price = 100, Stock = 200, Barcode = 123 }); //CATEGORY ÜZERİNDEN PRODUCT EKLEME en sonda category belirtmeye gerek yok çünkü  kategori üzerinden eklio
    category.Products.Add(new Product() { Name = "Defterler 2", Price = 100, Stock = 200, Barcode = 123 });
    _context.Add(category);


    var category1 = new Category() { Name = "Defterler" };

    var category2 = _context.Categories.First(x => x.Name == "Defterler"); //databasede hazır bulunan kategori sınıfını çekip öyle product ekleyebilirsin.

    var product1 = new Product() { Name = "Defter 3", Price = 100, Stock = 200, Barcode = 123, CategoryId = category2.Id };

    var product2 = new Product() { Name = "Kalem 1", Price = 100, Stock = 200, Barcode = 123, Category = category1 }; //burda gerçekleşen bizim veritabanımızda henüz category yok bu sebeple id yazamıyoruz
                                                                                                                      //yukarda instance oluşturulan category burada veriliyor.
                                                                                                                      //database kayıt yaparken bu durumda ayrıyetten _context.Category.Add(category);
                                                                                                                      //yazmamıza gerek yok çünkü bizim datamız ilişkili data EF core kendisi onu da ekliyor.
                                                                                                                      //ama instance oluşturulmuş olması gerek.

    _context.Products.Add(product);   //sadece product ekleyerek hem product hem category ekleme işlemi yaptık.PRODUCT ÜZERİNDEN CATEGORY EKLEME

    category.Products.Add(new Product() { Name = "Defterler 1", Price = 100, Stock = 200, Barcode = 123 }); //CATEGORY ÜZERİNDEN PRODUCT EKLEME en sonda category belirtmeye gerek yok çünkü  kategori üzerinden eklio
    category.Products.Add(new Product() { Name = "Defterler 2", Price = 100, Stock = 200, Barcode = 123 });
    _context.Add(category);



    _context.SaveChanges();
    //_context.SaveChanges();
}

void DataAddOneToOne(AppDbContext _context)
{
    //product => Parent   productFEature => Child

    var category = _context.Categories.First(x => x.Name == "Silgiler");  //kategorisi olmayan produt ekleyemiyceğimiz için burada category oluşturduk.
    var product = new Product()
    {
        Name = "Silgi 3",
        Price = 200,
        Stock = 200,
        Barcode = 123,
        Category = category,
        ProductFeature = new ProductFeature() { Color = "red", Height = 100, Width = 200, } //product eklerken birebir ilişkisi olan product feature da eklendi onun childi gibi düşüşn
    };


    //TAM TERSİ DURUM ÖNCE PRODUCTFEATURE OLUŞTUR ONU KAYDET ÖZELDEN GENELE GİT

    ProductFeature productFeature = new ProductFeature()
    {
        Width = 300,
        Height = 300,
        Color = "Blue",
        Product = new Product() { Name = "Silgi 4", Price = 200, Stock = 200, Barcode = 432, Category = category }
    };

    _context.ProductFeatures.Add(productFeature);  //ÖNCE PRODUCTFEATURE OLUŞTURUP KAYDETTİK ÖZELDEN GENELE daha sonra product ve category EFCore tarafından kendisi eklendi.
                                                   //_context.Products.Add(product);
    _context.SaveChanges();

    Console.WriteLine("Kaydedildi");


}

void DataAddManyToMany(AppDbContext _context)
{
    //ilk senaryoda önce öğrenci oluşturuluyor ve database öğrenci ekleniyor (_context.Add(student);) bu öğrenciye bağlı iki adet öğretmen EFcore tarafından ekleniyor.
    var student = new Student() { Name = "Ahmet", Age = 23 };
    student.Teachers.Add(new Teacher() { Name = "Ali Öğretmen" });
    student.Teachers.Add(new Teacher() { Name = "Ayşe Öğretmen" });
    _context.Add(student);
    _context.SaveChanges();

    //ikinci senaryoda tam tersi öğretmen üzerinden öğrenci ekleme

    var teacher = new Teacher()
    {
        Name = "Hasan Öğretmen",
        Students = new List<Student>()
        {
            new Student() {Name="Emir",Age=14},
            new Student() {Name="Bilal",Age=12}
        }
    };

    _context.Add(teacher);
    _context.SaveChanges();


    //üçüncü senaryoda var olan öğretmen ya da öğrenci üzerinden bir dierğini ekleme YANİ UPDATE ETME BURASI ÖNEMLİ update methodunu çağırmaya gerek yok.

    var teacher1 = _context.Teachers.First(x => x.Name == "Hasan Öğretmen");
    teacher1.Students.AddRange(new List<Student>  //birden fazla veri eklemek için AddRAnge kullan list gönderir
            {
            new Student() { Name = "Zeynep", Age = 10 },
            new Student() { Name = "Işıl", Age = 9 }
            }
    );

    _context.SaveChanges();

}

void DataDeleteBehaviors(AppDbContext _context)
{
    var category = new Category()
    {
        Name = "Kalemler",
        Products = new List<Product>()
        {

        new Product(){ Name="kalem1" ,Price=100,Stock=200,Barcode=444},
        new Product(){ Name="kalem2" ,Price=100,Stock=200,Barcode=444},
        new Product(){ Name="kalem3" ,Price=100,Stock=200,Barcode=444}

        }
    };

    _context.Add(category);
    _context.SaveChanges();

    var c = _context.Categories.First(x => x.Name == "Kalemler"); // kategoriyi sorguladık c ye atadık

    //< --------Bu kısım Restrict olduğu zaman geçerli---------- >
    var products = _context.Products.Where(x => x.CategoryId == c.Id); // davranış restrict oldğu zaman bizim kategoriyi silmemiz izin vermez önce o kategoriye bağlı products
    _context.RemoveRange(products);                     //silinmesi gerekir ondan sonra kategori silinebilir. removeRange methodu liste halinde gönderilen verileri siler.
                                                        //< --------Bu kısım Restrict olduğu zaman geçerli---------- >

    _context.Categories.Remove(c); //kategori silme
    _context.SaveChanges();

}

//< ---------------RELATED DATA LOAD-------------- >
void EagerLoading(AppDbContext _context)
{
    var category = new Category()
    {
        Name = "Defterler",
        Products = new List<Product>()
        {
            new (){Name="Defter 1",Price=100,Stock=100,Barcode=111,ProductFeature=new (){Color="Yellow",Height=12,Width=6}},
            new (){Name="Defter 2",Price=100,Stock=100,Barcode=111,ProductFeature=new (){Color="Blue",Height=12,Width=6}},
            new (){Name="Defter 3",Price=100,Stock=100,Barcode=111,ProductFeature=new (){Color="Green",Height=12,Width=6}}
        }
    };

    //ASENKRON KULLANMAYA ÇALIŞ
    _context.Add(category);
    //_context.Categories.Add(category); //bir üst satırla aynı işi yapar farketmio
    _context.SaveChanges();


    //genelden özele sorgulama
    var categoryWithProducts = _context.Categories.Include(x => x.Products).
    ThenInclude(x => x.ProductFeature).First();  //asıl eager loading kodu categorileri getirirken Include methodyla productları da ekledik sorguya.
                                                 //ThenInclude methoduyla da producttan product feature erişim sağladık.

    //özelden genele sorgulama
    var productFeatureWithProducts = _context.ProductFeatures.Include(x => x.Product).ThenInclude(x => x.Category).First();

    //ortadaki entityden sorgulama 2 tane farklı navigation property olanlar için
    var product = _context.Products.Include(x => x.ProductFeature).Include(x => x.Category).First(); //iki tane Include kullandık farkı orada.

    var categoryWithProducts1 = _context.Categories.First(); //bu kodu yazarsan products erişemezsin aşağıdaki kodda hata almazsın fakat veritabanından products verisi gelmez.

    categoryWithProducts1.Products.ForEach(product =>
    {
        Console.WriteLine($"Ürün : {categoryWithProducts1.Name} - {product.Id} - {product.Name} - {product.Price} - {product.Stock} - {product.Barcode}");
    });

    Console.WriteLine("İŞlEM BİTTİ");
}

async Task ExplicitLoadingAsync(AppDbContext _context)  //async method kllandığımız için kodda methodun kendisini de async yapmak zorundayız.
{
    var category = await _context.Categories.FirstAsync();

    if (true)
    {
        _context.Entry(category).Collection(x => x.Products).Load(); //CAtegory entitysi birden fazla product sahip olduğu için collection ile girdik sorguya.
        category.Products.ForEach(x =>
        {
            Console.WriteLine(x.Name);
        });

    }


    var product = await _context.Products.FirstAsync();

    if (true)
    {
        _context.Entry(product).Reference(x => x.ProductFeature).Load(); //productın bire bir ilişkisi olduğu için collection değil de referans ile girdik sorguya
        Console.WriteLine(product.ProductFeature.Color);
    }
}

async Task LazyLoading(AppDbContext _context)
{

    var category = await _context.Categories.FirstAsync();
    Console.WriteLine("Kategori Çekildi");
    var products = category.Products;
    foreach (var product in products)
    {
        var productFeature = product.ProductFeature;  //LazyLoading in ikinci sorgusu product bilgilerini almaktı bu satırda üçüncü sorguyu yapacak product featureları alcak
                                                      //lazy loadingin kötü olduğu nokta her bir döngüde navigation propertye gittiği için
                                                      //her yeni döngüde yeni bir sorgu yazıyor.SIKINTILI DURUM!!! performans problemi
                                                      //(N+1) PROBLEMİ DENİR BU PROBLEME !!
                                                      //DOMAIN DRIVEN DESIGN DA LAZYLOADING Açık olması tavsiye edilir.
    }

    Console.WriteLine("Lazy loading BİTTİ");
}

//< ---------------EF CORE INHERITANCE-------------- >
void TablePerHiearchy(AppDbContext _context)
{
    //AppDbContextte baseClassı Dbset olarak eklediğimiz zaman subclassların dBsetleri için tablo oluşturmaz.!!! 

    //data ekleme
    //_context.Persons.Add(new Manager() { FirstName = "m1", LastName = "M1", Age = 31, Grade = 1 });
    //_context.Persons.Add(new Employee() { FirstName = "e1", LastName = "E1", Age = 20, Salary = 3000 });

    //data okuma

    var managers = _context.Managers.ToList(); //sadece managers gelcek
    var employees = _context.Employees.ToList();   //sadece employees gelcek 

    var persons = _context.Persons.ToList();  //hem mangers hem employees gelcek çünkü parent class

    persons.ForEach(p =>
    {
        switch (p)
        {
            case Manager manager: //gelen p değeri Manager ise
                Console.WriteLine($"manager Entity : {manager.Grade}");
                break;
            case Employee employee:  //gelen p değeri employee ise
                Console.WriteLine($"employee Entity : {employee.Salary}");
                break;
            default:
                break;
        }
    });

    _context.SaveChanges();

}

void TablePerType(AppDbContext _context)
{
    //_context.Managers.Add(new Manager() { FirstName = "m1", LastName = "M1", Age = 31, Grade = 1 }); //managers üzerinden kayıt
    //_context.Employees.Add(new Employee() { FirstName = "e1", LastName = "E1", Age = 14, Salary = 2000 }); //employees üzerinden kayıt

    _context.Persons.Add(new Manager() { FirstName = "m2", LastName = "M2", Age = 31, Grade = 1 });      //iki kayıt da persons üst sınıfı üzerinde yapıldı
    _context.Persons.Add(new Employee() { FirstName = "e2", LastName = "E2", Age = 14, Salary = 2000 });

    _context.SaveChanges();

    var managers = _context.Managers.ToList(); //sadece managers gelcek
    var employees = _context.Employees.ToList();   //sadece employees gelcek 

    var persons = _context.Persons.ToList();  //hem mangers hem employees gelcek çünkü parent class

    persons.ForEach(p =>
    {
        switch (p)
        {
            case Manager manager: //gelen p değeri Manager ise
                Console.WriteLine($"manager Entity : {manager.Grade}");
                break;
            case Employee employee:  //gelen p değeri employee ise
                Console.WriteLine($"employee Entity : {employee.Salary}");
                break;
            default:
                break;
        }
    });

    Console.WriteLine("İŞLEM BİTTİ");
}

//< ---------------EF CORE MODEL-------------- >
void OwnedEntityTypes(AppDbContext _context)
{

}

void KeylessEntityTypes(AppDbContext _context)
{
    //var category = new Category() { Name = "Kalemler" };
    //category.Products.Add(new Product() { Name = "Kalem 1", Barcode = 1907, Price = 100, Stock = 10, ProductFeature = new() {Color="sarı",Height=10, Width=6} });
    //category.Products.Add(new Product() { Name = "Kalem 2", Barcode = 1907, Price = 100, Stock = 10, ProductFeature = new() { Color = "lacivert", Height = 10, Width = 6 } });
    //category.Products.Add(new Product() { Name = "Kalem 3", Barcode = 1907, Price = 100, Stock = 10, ProductFeature = new() { Color = "yeşil", Height = 10, Width = 6 } });
    //_context.Categories.Add(category);
    //_context.SaveChanges();


    var productFulls = _context.ProductFulls.FromSqlRaw(@"select c.Name'Category Name',p.Name,p.Price,pf.Height from Products p
         join ProductFeatures pf on p.Id=pf.Id
         join Categories c on p.CategoryId=c.Id").ToList();
    Console.WriteLine("İşlem Bitti  ! !");
}

void EntityProperties(AppDbContext _context)
{

}

void Indexes(AppDbContext _context)
{
    //_context.Products.Where(x => x.Name == "Kalem 1").Select(x => new { x.Name,x.Price,x.Stock }); //Sorgu Name alanı Kalem 1 olan name price ve stock bilgilerini getir.
    var category = new Category() { Name = "kağıtlar" };
    //_context.Products.Add(new Product() { Name = "kağıt1", Barcode = 1231, Price = 120, DiscountPrice = 100, Stock = 10, Url = "abab",Category=category
    //    , ProductFeature = new() { Height = 10,Color="red",Width=12}
    //});
    //indeksleme örneğin Name propu üzerinde fazla sorgu yapıyorsan bu işi kolaylaştırmak için yapılır Name alanı indekslenir.
    _context.Products.Add(new Product()
    {
        Name = "kağıt1",
        Barcode = 1231,
        Price = 120,
        DiscountPrice = 150,
        Stock = 10,
        Url = "abab",
        Category = category
,
        ProductFeature = new() { Height = 10, Color = "red", Width = 12 }
    });
    _context.SaveChanges();
    Console.WriteLine("Index Model Başarılı");
}

//< ---------------QUERY-------------- >

void ClientServerEvaluation(AppDbContext _context)
{
    //_context.People.Add(new Person() { Name = "mami", Phone = "05554443322" });
    //_context.People.Add(new Person() { Name = "ali", Phone = "05334443322" });
    //_context.SaveChanges();

    //var people = _context.People.Where(x => FormatPhone(x.Phone) == "5554443322").ToList(); // aşağdıaki method baştaki sıfırı atıyo karşılaştırcağımız telefon da 0 sız olduğu için methodu lambda içinde vermeye
    //çalıştık ama olmadı çünkü burası sql cümleciğine dönüşemez.Bunu aşmak için Client değerlendirmesi yapmamız lazım
    var people = _context.People.ToList().Where(x => FormatPhone(x.Phone) == "5554443322").ToList(); //toList eklediğimz zaman bu kod çalışıyor çünkü memory alınıyor burası server değerlendirmesi bitmiş oluyor

    Console.WriteLine("ClientServerEvaluation Sorgusu Başarılı!!!");
}

//Inner Join  iki tablo arasındaki ortak alanları almak istediğimizde kullanılan join tipi iki tablo arasında NAVIGATION PROP yoksa kullanabilirsin
//navigation propertyleri kaldırmadık data eklemek kolay olsun diye ama data okurken bunları kullanmayıp join yapısıyla çalışacağız
void InnerJoin(AppDbContext _context)
{
    //var category = new Category() { Name = "Kalemler" };
    //category.Products.Add(new Product() { Name = "Kalem 1", Barcode = 1907, Price = 100, Stock = 10, Url = "http", ProductFeature = new() { Color = "sarı", Height = 10, Width = 6 } });
    //category.Products.Add(new Product() { Name = "Kalem 2", Barcode = 1907, Price = 100, Stock = 10, Url = "http", ProductFeature = new() { Color = "lacivert", Height = 10, Width = 6 } });
    //category.Products.Add(new Product() { Name = "Kalem 3", Barcode = 1907, Price = 100, Stock = 10, Url = "http", ProductFeature = new() { Color = "yeşil", Height = 10, Width = 6 } });
    //_context.Categories.Add(category);
    //_context.SaveChanges();
    //Console.WriteLine("İŞLEM BİTTİ");

    //join yapısı linq sorgusu yazabiliriz

    var result = _context.Categories.Join(_context.Products, x => x.Id, y => y.CategoryId, (c, p) => new  //virgülden sonraki ilk kısım yani x category denk geliyor ikinci kısım ise Product
    {                                                                                   //üçüncü kısım ise result selector yani alcağımız data c category p product karşılık gelir
        CategoryName = c.Name,  //istersek burdaki gibi isimsiz bir class da oluşturabiliriz normalde böyle bir class yok böyle propertyler yok
        ProductName = p.Name,
        ProductPrice = p.Price,

    }).ToList();

    //daha sonra bu oluşturulan isimsiz classtaki propertylere erişmek için foreach ile dönebilirz.

    result.ForEach(x =>
    {
        var a = x.ProductName;
        var b = x.ProductPrice;
        var c = x.CategoryName; //yukarıda oluşturduğumuz propertyler var sadece. isimsiz class yolu  birinci yoldu.
    });

    //diğer yol ise biz sadece products almak isteyebiliriz. 2 li join
    var result1 = _context.Categories.Join(_context.Products, x => x.Id, y => y.CategoryId, (c, p) => p).ToList(); //sadece Products geldi.
    var result2 = _context.Categories.Join(_context.Products, x => x.Id, y => y.CategoryId, (c, p) => c).ToList(); //sadece category geldi.

    //SQL cümleciği gibi yazabiliriz bu ayrı br yol 2li join
    var result3 = (from c in _context.Categories  //category c olarak al products p olarak al ve joinle categorynin id si ile products un CategoryId sini kullanarak.
                   join p in _context.Products on c.Id equals p.CategoryId
                   select c).ToList();

    //3 lü join
    var result4 = _context.Categories
        .Join(_context.Products, c => c.Id, p => p.CategoryId, (c, p) => new { c, p })
        .Join(_context.ProductFeatures, x => x.p.Id, y => y.Id, (c, pf) => new //bu satırda da productId ile productfeature idsi joinlendi zaten o iki id identicaldi.
        {                                                                      //bir üst kodda (c,pf) c category yerine p product verirsen category erişimin kapanır.
                                                                               //ama category üzerinden product erişebiliyoruz.
            CategoryName = c.c.Name,
            ProductName = c.p.Name,
            ProductFeatureColor = pf.Color
        });

    //SQL cümleciği gibi yazabiliriz bu ayrı br yol 3li join BU YOL DAHA ANLAŞILIR BUNU KULLAN.!!!!!!!!

    var result5 = (from c in _context.Categories
                   join p in _context.Products on c.Id equals p.CategoryId
                   join pf in _context.ProductFeatures on p.Id equals pf.Id  //product feature joinle ne ile productın id si ile kendi id si olarak.
                   select new
                   {
                       CategoryName = c.Name,
                       ProductName = p.Name,
                       ProductFeatureColor = pf.Color
                   }).ToList();
    //tüm dataları beraber almak istiyorsan sonunu değiştir

    var result6 = (from c in _context.Categories
                   join p in _context.Products on c.Id equals p.CategoryId
                   join pf in _context.ProductFeatures on p.Id equals pf.Id  //product feature joinle ne ile productın id si ile kendi id si olarak.
                   select new
                   { c, p, pf }).ToList(); //bu şekilde bütün datalar gelcek
    result6.ForEach(x =>
    {
        var categoryName = x.c.Name;
        var productName = x.p.Name;
        var productFeatureColor = x.pf.Color;
    });
    Console.WriteLine("InnerJoin Sorgusu Başarılı!!!");
}

void LeftAndRightJoin(AppDbContext _context)
{
    //QUERY SYTNAX
    var result = (from p in _context.Products
                  join pf in _context.ProductFeatures on p.Id equals pf.Id into pflist  //bu kısımda productFEatureların bi listesii temsil ediyor.Biz bütün productları almaya çalışıyoruz şuan
                  from pf in pflist.DefaultIfEmpty() //eğer productfeature boşsa defaultunu al diyoruz 4 tane product 3 tane productfeature satırımız var o eksik olan productfeature yerine null döncek.
                  select new { p, pf }).ToList();
    var leftJoin = (from p in _context.Products
                    join pf in _context.ProductFeatures on p.Id equals pf.Id into pflist
                    from pf in pflist.DefaultIfEmpty()
                    select new
                    {
                        Product_Name = p.Name,
                        Product_Color = pf.Color, //int değerlerde bunu yaparsak uygulama patlar çünkü left join yaptık productların hepsin aldık ama bi product satırında product feature null gelecek.
                        Product_Width = (int?)pf.Width,   //her zaman dolu gelmeyeceği için patlayacak bunu engellemek için NULLABLE belirtebilirsin
                        Product_Height = (int?)pf.Height == null ? 5 : pf.Height //default atama eğer null ise 5 değerini ata deilse de kendi değerini ata.
                    }).ToList();
    //yerlerini değişince left iken right join oluyor.
    var rightJoin = (from pf in _context.ProductFeatures
                     join p in _context.Products on pf.Id equals p.Id into plist
                     from p in plist.DefaultIfEmpty()
                     select new
                     {
                         Product_Name = p.Name,
                         Product_Color = pf.Color,
                         Product_Price = (int?)p.Price,  //nullable typeları belirtsen daha iyi aslında
                         Product_Width = pf.Width,
                         Product_Height = pf.Height  //burdaki nullable ifadelerini kaldırdıkçünkü artık baz aldığı tablo productfeature tablosu oldu o tabloda 3 satır veri var
                                                     //product tablosunda 4 satır veri var kesişenleri ve productFEatue tablosundakileri alacağı için nullable yapmamıza gerek yok.
                     }).ToList();

    Console.WriteLine("LeftAndRightJoin Sorgusu Başarılı!!!");
}

void FullOuterJoin(AppDbContext _context)
{
    //QUERY SYTNAX
    var left = (from p in _context.Products
                join pf in _context.ProductFeatures on p.Id equals pf.Id into pflist
                from pf in pflist.DefaultIfEmpty().ToList()
                select new
                {
                    ID = p.Id,
                    Product_Name = p.Name,
                    Product_Color = pf.Color,
                }).ToList();

    var right = (from pf in _context.ProductFeatures
                 join p in _context.Products on pf.Id equals p.Id into plist
                 from p in plist.DefaultIfEmpty().ToList()
                 select new
                 {
                     ID = p.Id,
                     Product_Name = p.Name,
                     Product_Color = pf.Color,
                 }).ToList();

    var outerJoin = left.Union(right);

    Console.WriteLine("FullOuterJoin Sorgusu Başarılı!!!");
}

void RawSQL(AppDbContext _context)
{
    //parametresiz
    var productsSQL = _context.Products.FromSqlRaw("select * from products").ToList();

    //parametreli
    var id = 4;
    var productsSQLWithParameter = _context.Products.FromSqlRaw("select * from products where Id={0}", id).ToList(); //buradaki {0} virgülden sonraki ilk basamak demek.

    var price = 200;
    var productsSQLWithParameter1 = _context.Products.FromSqlRaw("select * from products where price>{0}", price).ToList(); //price gönderilen değişkenden büyükse getir.

    var sorgu = "select * from products where barcode!={0}";
    var barcode = 1234;
    var productsSQLWithParameter2 = _context.Products.FromSqlRaw(sorgu, barcode).ToList();

    var stock = 10;
    var productsSQLWithParameter3 = _context.Products.FromSqlInterpolated($"select * from products where stock>{stock}").ToList();
    //var deger = string.Format("ÜRÜNLER : {0} - {1}",12,"mami"); //o yerşne 12gelcek 1 yerine de mami yazack 

    //CUSTOM query yazımı
    //var productsSQLCustom = _context.Products.FromSqlRaw("select Id,Price,Name from products").ToList(); // bu şekilde yazarsan kod patlayacak çünkü _context.Products entitysi bu sorguyu karşılıyor yani
    //Products entitysindeki alanların hepsini doldurmaya çalışıyor fakat biz bütün sütunları sorguya dahil etmediğimiz için    
    //bunu yapamıyor ve hata veriyor bunun önüne geçmek içinse yeni entity oluşturup orada sorguya dahil edeceğimiz sütunarı belirtmek ve 
    //context üzerinden o sınıfı çağırmak.
    //var productSQLCustom1 = _context.ProductEssentials.FromSqlRaw("select Id,Price,Name from products").ToList(); //bu şekilde _context.ProductEssentials yaparsak hata vermeyecek çalışacak sorgu.

    //mesela biz id dönsün istemiyoruz diyelim her sorguda id sorgulaması yapmayız gerçek hayatta bunun için ise entityden Primary Key id alanını kaldırıp bunu da
    //EF.Core  attribute [Keyless] ya da FLuentAPI de modelbuilder.Entity<ProductEssentials>().HasNoKey(); şeklinde belirtmek gerekir!!!!

    var productSQLCustom2 = _context.ProductEssentials.FromSqlRaw("select Price,Name from products").ToList();

    Console.WriteLine("RawSql Sorgusu Başarılı!!!");
    //sql de yazılan inner join sorgusu kullanımı
    var productsWithFeature = _context.ProductWithFeatures.FromSqlRaw(@"select p.Id,p.Name,p.Price,pf.Color,pf.Height from Products p inner join ProductFeatures pf on p.Id=pf.Id").ToList();
    Console.WriteLine("RawSql Sorgusu Başarılı!!!");
}

void ToSqlQuery(AppDbContext _context)
{
    var products = _context.ProductEssentials.ToList(); //bu kod normalde çalışmaz çünkü bu obje db de yok okuyamaz (sorgu yok)efCore biz bunu engellemek için modelbuilderda ToSqlQueryde genel bir sorgu yazdık.
                                                        //model builderda yazdığımız sorgudan sonra hata vermeyecek ve okuma yapacak. 
    Console.WriteLine("ToSqlQuery Sorgusu Başarılı!!!");
}

void ToView(AppDbContext _context)
{
    var productsAll = _context.ProductAlls.Where(x => x.Width > 65).ToList(); //sonuna eklediğin sorgular da modelden gelen view sorgusu ile ekleniyor.

    Console.WriteLine("ToView Sorgusu Başarılı!!!");
}

void Pagination(AppDbContext _context)
{
    #region DataInsert
    //var category = new Category() { Name = "Defterler" };
    //category.Products.Add(new Product() { Name = "Defter 1", Barcode = 197, Price = 100, Stock = 10, Url = "http", ProductFeature = new() { Color = "sarı", Height = 9, Width = 6 } });
    //category.Products.Add(new Product() { Name = "Defter 2", Barcode = 907, Price = 300, Stock = 100, Url = "http", ProductFeature = new() { Color = "lacivert", Height = 8, Width = 5 } });
    //category.Products.Add(new Product() { Name = "Defter 3", Barcode = 107, Price = 200, Stock = 110, Url = "http", ProductFeature = new() { Color = "yeşil", Height = 6, Width = 2 } });
    //category.Products.Add(new Product() { Name = "Defter 4", Barcode = 207, Price = 500, Stock = 105, Url = "http", ProductFeature = new() { Color = "kırmızı", Height = 4, Width = 3 } });

    //_context.Categories.Add(category);
    //_context.SaveChanges(); 
    #endregion
    GetProducts(_context, 2, 3).ForEach(x =>
    {
        Console.WriteLine($"{x.Id} {x.Name} {x.Price} {x.CategoryId} {x.Category.Name} {x.ProductFeature.Color}");
    });

    //var productsWithPage = _context.Products.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    //BURADA PRODUCT TABLOSUNDA GEZİYORUZ SADECE PRODUCT SÜTUNLARINA ERİŞİRİZ NORMALDE AMA DİĞER İLİŞKİLİ TABLOLARIN SÜTUNLARINA ERİŞMEK İSTİYOSAN
    //GETPRODUCTS METODUNDA Include() metodunu çağırmayı da unutma
    Console.WriteLine("Pagination Sorgusu Başarılı!!!");
}

void GlobalQueryFilters(AppDbContext _context)
{
    //onmodelcreating içinde modelbuilderla yapılıyor oradaki sql sorgumuzda Product entitysinin isdeleted kısmına default false değeri atadık.
    //daha sonra modelBuilder.Entity<Product>().HasQueryFilter(x => x.IsDeleted==false); IsDeleted değeri false olanları getirmesini isteyeceğimiz bir sorguyazdık.
    
    var products = _context.Products.ToList();

    //daha sonra belki de bu query filteri kullanmak istemiycez o zaman ise aşağıdaki yolu kullanarak Ignore ediyoruz
    //var products1 = _context.Products.IgnoreQueryFilters().ToList();

    Console.WriteLine("Global Query Filter Başarılı!!!");

}

void QueryTags(AppDbContext _context)
{
    var productWithFeature = _context.Products.TagWith("Bu Query ürünler ve ürünlere bağlı özellikleri getirir") //tagleme kısmı
        .Include(x => x.ProductFeature).Where(x => x.Price > 100).ToList(); //join işlemimize Tagleme yapcaz

    Console.WriteLine("QueryTag Başarılı!!!");
}

void GlobalTracking(AppDbContext _context)
{
    //
    var product = _context.Products.First(x => x.Id == 6);  //tracking da

    product.Name = "Kalem 5";

    //_context.Update(product);   //fakat tracking kapalı olduğu zaman bu metodu yazıp EFCore belirtmemiz gerekiyor.
    //_context.Entry(product).State = EntityState.Modified; //update metoduyla aynı şeyi yapar.

    _context.SaveChanges();  //normal track ettiği zaman sadece saveChanges yapsak yeterli oluyor.
}


void StoredProcedure(AppDbContext _context)
{
    //eğer Stored Procedure geriye bir tablo dönüyorsa bunu karşılamak gerekiyo entity ile
    //örneğin insert update delete yapabilir bu procedure

    var product =  _context.Products.FromSqlRaw("exec sp_get_products").ToList();
    //önemli nokta Products entitysi için query filter koymuştuk IsDeleted false olanları getirmesi için storedprocedure o filter varken hata verdi.

    var productAll = _context.ProductAlls.FromSqlRaw("exec sp_get_products_all").ToList(); //burada procedure karşılayacağımız model var
    //bu sorguda 7 ver geldi bi üsttekinde 8 gelmişti bunun sebebi kalem 6 nın productFeature u yok, join yaptığımız için SQL procedure da kesişmeyeni getirmiyor.
    //custom modele mapleyeceksen verilerini oluşturduğun o custom modeli Keyless belirt   HasNoKey();

    //ParametreAlan StoredProcedure

    int categoryId = 3;
    decimal price = 100;
    var productAll1 = _context.ProductAlls.FromSqlInterpolated($"exec sp_get_products_all_parameters {categoryId},{price}").ToList();

    //geriye dönmeyen ya da geriye tek bir değer dönen (insert edilmiş datanın idsi)

    var productInsert = new Product()
    {
        Name = "Kalem 6",
        Price = 600,
        DiscountPrice = 429,
        Stock = 167,
        Barcode = 555,
        Url="httpsss",
        CategoryId = 3,
    };

    var newProductIdSqlParameter = new SqlParameter("@newId",System.Data.SqlDbType.Int);
    newProductIdSqlParameter.Direction = System.Data.ParameterDirection.Output;  // output türü olduğun belirtmemiz lazım sqlden geliyo bu id bilgisi bize
   
    _context.Database.ExecuteSqlInterpolated(@$"exec sp_insert_products {productInsert.Name},{productInsert.Price},{productInsert.DiscountPrice}
,{productInsert.Stock},{productInsert.Barcode},{productInsert.Url},{productInsert.CategoryId}, {newProductIdSqlParameter} out"); //burada da out belirtiyoruz.

    var newProductId = newProductIdSqlParameter.Value; //yukarda oluşturduğumuz bi sql parametresi biz bunun değerini alarak okuyabiliriz.
                                                       //aldığımız bu değer stored proceduredan gelen  producta ait Id değeri
    Console.WriteLine("StoredProcedure Başarılı!!!");
}

async Task Function(AppDbContext _context)
{
    //DBSET kullanarak BİRİNCİ YOL
    //tolist dendiğinde modelbuilderdaki function çalışacak
    //var productAll = await _context.ProductAlls.ToListAsync();

    //int categoryId = 4;
    //var productAllWithFeature = await _context.ProductAllWithFeatures.FromSqlInterpolated($"select * from fc_product_all_with_parameter({categoryId})").ToListAsync();

    //DBSET Kullanmadan İKİNCİ YOL modelBuilderda oluşturulan metot ile.
    var productAllWithFeatureSecondWay = await _context.GetProductAllWithFeatures(3).ToListAsync();
    var productAllWithFeatureSecondWay1 = await _context.GetProductAllWithFeatures(3).Where(x => x.Width<70).ToListAsync(); //storedprocedure aksine functionlarda where kullanabiliriz sql cümleciğine eklenir.
   
    //skaler değer dönenler için tanımlanan doğru fonksiyon kullanımı
    var products = await _context.Categories.Select(x => new //yeni kategori instance oluşturdu categorideki her veriyi CAtegoryName ve ProductCount olarak aldı kullandık metod.
    {
        CategoryName = x.Name,
        ProductCount = _context.GetProductAllCount(x.Id)
    }).ToListAsync();


    //SKALER TEK DEĞER DÖNEN FUNC İKİNCİ YOL MAPLEME MODEL
    int categoryId1 = 4;
    var count = _context.ProductCount.FromSqlInterpolated($"select dbo.fc_get_product_count({categoryId1}) as Count").First().Count; //zaten tek değer dönceğini biliyoruz modelde de tek prop var o yüzden
    //first yapıp o tek değeri alıp o değerin de modeldeki karşılığı olan Count u alıyoruz.!!!buraya verdiğin "Count" ismi önemli burdaki isimle karşılayacak modeldeki prop ismi aynı olmalı EFCore anlayamaz yoksa!!!
    Console.WriteLine("Function Başarılı!!!");

}

async Task Projection(AppDbContext _context)
{
    //anonymous entity

    //var product1 = await _context.Products.Include(x => x.Category).Include(y => y.ProductFeature).Select(z => new
    //{
    //    categoryname = z.Category.Name,
    //    productname = z.Name,
    //    productprice = z.Price,
    //    width =(int?)z.ProductFeature.Width
    //}).Where(x => x.width>10).ToListAsync();

    //var product2 = await _context.Categories.Select(z => new //eğer SELECT kullandıysan Include yazmana gerek yok ondanönce kaldırabilirsin.
    //{
    //    CategoryName = z.Name,
    //    ProductNames = String.Join(",", z.Products.Select(x => x.Name)), //z kategori oluyo,x kategorisindeki productlardan nameleri seç ve virgülle birleştirip anonim sınıfın  products sütununa ata
    //    TotalPrice = z.Products.Sum(x => x.Price), //kategorilerdeki productsların pricelarını topla
    //    Price = z.Products.Select(x => x.Price).Sum() //bu şekilde de sum yapabilirsin. ama daha uzun yol oluyor yukarıdaki kod daha kolay kısa.
    //}).Where(A => A.TotalPrice > 123).OrderBy(x => x.TotalPrice).ToListAsync();  //total 123den kucukler ve totalprice göre sıralama


    //DTO - VIEW MODEL

    //var product3 = await _context.Products.Include(x => x.Category).Include(y => y.ProductFeature).Select(z => new ProductDTO
    //{
    //    CategoryName = z.Category.Name,
    //    ProductName = z.Name,
    //    ProductPrice = z.Price,
    //    Width = (int?)z.ProductFeature.Width
    //}).Where(x => x.Width > 10).ToListAsync();


    //var product4 = await _context.Categories.Select(z => new ProductDTO2//eğer SELECT kullandıysan Include yazmana gerek yok ondanönce kaldırabilirsin.
    //{
    //    CategoryName = z.Name,
    //    ProductNames = String.Join(",", z.Products.Select(x => x.Name)), //z kategori oluyo,x kategorisindeki productlardan nameleri seç ve virgülle birleştirip anonim sınıfın  products sütununa ata
    //    TotalPrice = z.Products.Sum(x => x.Price), //kategorilerdeki productsların pricelarını topla
    //    Price = z.Products.Select(x => x.Price).Sum(), //bu şekilde de sum yapabilirsin. ama daha uzun yol oluyor yukarıdaki kod daha kolay kısa.
    //    TotalWidth = (int?)z.Products.Select(x => x.ProductFeature.Width).Sum() //her productun feature olmayabilir o sebeple null gelebilir width
    //}).Where(A => A.TotalPrice > 123).OrderBy(x => x.TotalPrice).ToListAsync();  //total 123den kucukler ve totalprice göre sıralama


    //DTO VIEW MODEL WITH AUTO-MAPPER
    //AutoMapper kullanırsan dbden bütün Product propertyleri gelir ama yukarıda yaptığın gibi elle yaparsan sadece {}içinde belirttiğin propertyler sql sorgusunda bulunur.
   

    //var product5 = _context.Products.ToList(); //buradakmaplemede sql sorgusunda bütün Products Propertyleri sorgulanıyor productDtoMap2 de sadece Dto da olanlar sorgulanıyor.

    //var productDtoMap = ObjectMapper.Mapper.Map<List<ProductDTOAutoMapper>>(product5);  //product5 bize liste döndüğü için burada da belirttik.

    //otomatik DTO mapleme yukardaki maplemede ütün products propertylerini çektik ondan sonra mapledik bu örnkte sadece ihtiyacı olanları alacak.

    //var productDtoMap2 = _context.Products.ProjectTo<ProductDTOAutoMapper>(ObjectMapper.Mapper.ConfigurationProvider).ToList(); // 794. satırdaki gibi propertyleri tek tek yazıp eşitlemekten kurtulduk aynı işi yapıyor.
                                                                                                                                //Kullanacağoımız entity üzerinden ProjectTo<>() metoduyla erişiyoruz   ConfigurationProvider istiyodu bizden biz de bunu  ObjectMapper.Mapper da veriyoruz

    //istersek where ve select gibi LINQ metodları kullanabiliriz bu oluşacak SQL cümleciğine eklenir
    var productDtoMap3 = _context.Products.ProjectTo<ProductDTOAutoMapper>(ObjectMapper.Mapper.ConfigurationProvider).Where(x => x.Price>100).Select(x => x.Name).ToList();
   
                                                                                                                        
    Console.Out.WriteLine("Projection Başarılı");
}

void Transactions(AppDbContext _context)
{

    using(var transaction = _context.Database.BeginTransaction())
    {
        var category = new Category() { Name = "Kılıflar" };

        _context.Categories.Add(category);
        _context.SaveChanges();  //ilk savechanges

        var product = new Product()
        {
            Name = "Kılıf 1",
            Price = 250,
            DiscountPrice = 200,
            Stock = 341,
            Barcode = 12334,            //gerçek hayatta category.Id yerine direkt yukarıda olşturduğumuz category verebiliriz. o zaman bu transaction scopuna gerek yok
            Url = "httpss",              //Id veri tabanına kaydolunca oluşacağı için committen önce o değer aslında yoktur.
            CategoryId = category.Id,  //kayıt yaparken yukarda oluşturduğmuz categorynin idsini aldık fakat burda bir problem var SaveChanges() aşağıda çağırdığımız için daha id oluşmamış olacak.
                                       //bunun önüne geçmek için producttan önce de SaveChanges() çağırmamız gerekir.
            ProductFeature = new() { Color = "blue", Height = 10, Width = 6 }
        };

        _context.Products.Add(product);
        _context.SaveChanges(); //ikinci savechanges

        using (var dbContext2 = new AppDbContext(connection))
        {
            dbContext2.Database.UseTransaction(transaction.GetDbTransaction()); 
        }

        transaction.Commit(); //transaction scopuna aldık en sonda commit ettiğimiz için istediğin kadar savechanges koy bir tanesi hata verse dahi diğerleri de iptal olacak.
        //eğer try catch bloğu kullanıyorsan bu transaction için açık bir şekilde transaction.RollBack();
    }

   

}


void IsolationLevels()
{

}
using (var _context = new AppDbContext()) //AppDbContext(1907) barcode araması yapabilirsin
{
    //<--------------DBCONTEXT-------------->

    //StatesAndTracking(_context);
    //DataEkle( _context);

    //< ---------------RELATIONSHIPS-------------- >

    //DataAddOneToMany(_context);
    //DataAddOneToOne(_context);
    //DataAddManyToMany(_context);
    //DataDeleteBehaviors(_context);

    //< ---------------RELATED DATA LOAD-------------- >

    //EagerLoading(_context);
    //ExplicitLoadingAsync(_context);
    //LazyLoading(_context);

    //< ---------------RELATED DATA LOAD-------------- >

    //< ---------------EF CORE INHERITANCE-------------- >

    //TablePerHiearchy(_context);
    //TablePerType(_context);

    //< ---------------EF CORE INHERITANCE-------------- >


    //< ---------------EF CORE MODEL-------------- >

    //OwnedEntityTypes(_context);
    //KeylessEntityTypes(_context);
    //EntityProperties(_context);
    //Indexes(_context); 
    //< ---------------EF CORE MODEL-------------- >


    //< ---------------QUERY-------------- >

    //ClientServerEvaluation(_context);
    //InnerJoin(_context);
    //LeftAndRightJoin(_context);
    //FullOuterJoin(_context);
    //RawSQL(_context);
    //ToSqlQuery(_context);
    //ToView(_context);
    //Pagination(_context);
    //GlobalQueryFilters(_context);
    //QueryTags(_context);
    //GlobalTracking(_context);
    //< ---------------QUERY-------------- >

    //< ---------------STORED PROCEDURE - FUNCTION-------------- >

    //StoredProcedure(_context);
    //await Function(_context);


    //< ---------------STORED PROCEDURE - FUNCTION-------------- >


    //< ---------------PROJECTIONS-------------- >

    //await Projection(_context);

    //< ---------------PROJECTIONS-------------- >


    //< ---------------TRANSACTIONS-------------- >

    //Transactions(_context);

    //< ---------------TRANSACTIONS-------------- >


    //< ---------------ISOLATION LEVELS-------------- >




    //< ---------------ISOLATION LEVELS-------------- >


    //<---------------DbSet METHODS------------->
    #region DBSET METHODS
    //var product1 = _context.Products.First(x => x.Id==100); //bulamazsa exception fırlatır.
    //var product2 = _context.Products.FirstOrDefault(x => x.Id == 100); //bulamazsa null döner
    //var product3 = _context.Products.FirstOrDefault(x => x.Id == 100, new Product() { Id = 1 ,Name="silgi",Price=111,Stock=1,Barcode=111}) ;   //default value de atayabiliyoruz.
    //var product4 = _context.Products.SingleAsync(x => x.Id == 7);    //databaseden birden fazla datayla sonuçlanırsa exception fırlatır. sadece tek bir data dönmesi gerek.
    //var pruduct5 = await _context.Products.Where(x=> x.Id == 11 && x.Name=="silgi").ToListAsync();  //sql sorgularaına benzer queryler yazabiliriz. ismi silgi idsi 11 olanı getir. listeye atar.
    //var product6 = await _context.Products.FindAsync(10);  // direkt primary key ile arama yapar birden fazla oprimary key varsa o tabloda parantez içinde virgülle belirtebiliriz. Null Döner bulamazsa
    //var product7 = await _context.Products.AsNoTracking().FirstAsync(x => x.Id == 7);  //update yapmayacaksak AsNoTracking kullanıp memorynin track etmemesini sağlar daha hızlı çalışma yapar
    //Console.WriteLine(_context.Entry(product7).State);  //track etmediğimiz için detached dönecek ama track etseydik veritabanıyla aynı veri olduğunu farkedip unchanged döncekti

    #endregion
    //<---------------DbSet METHODS------------->



    //<---------------CONFIGURATION-------------->

    //<---------------CONFIGURATION-------------->


}


// bu methodu query konusunda ClientServerEvaluation için oluşturduk custom sorgu içinde denemeye çalışcaz.
string FormatPhone(string phone)
{
    return phone.Substring(1, phone.Length - 1);
}


//Paginationda kullanacağımız list dönen method
List<Product> GetProducts(AppDbContext appContext, int page, int pageSize)  //list dönüyo sayfa büyüklüğü, sayısı ve AppDbContext alıyo
{
    return appContext.Products.Include(x => x.Category).Include(x => x.ProductFeature)
        .OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    //Include Category yapmazsan category name vs erişemezsin sadece product proplarına erişebilirsin.
    //aynı şekilde productfeature include etmezsen bilgilerine erişemezsin.

    //return appContext.Products.Include(x => x.Category).Include(x => x.ProductFeature).Where(x => x.CategoryId == 3)
    //.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
    //aynı zamanda burada sorgu da yapabiliyoruz mesela categoryId 3 olanları getir de diyebiliriz.
    //burada sadece categoryId==3 olanları alır listeler onun üzerinden sayfalama yapar diğerlerini katmaz.
}
