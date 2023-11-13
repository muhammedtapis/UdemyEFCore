// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using System.Security.Cryptography;
using UdemyEFCore.CodeFirst;
using UdemyEFCore.CodeFirst.DataAccessLayer;


DbContextInitializer.Build();


using (var _context = new AppDbContext())
{


    //<---------------DATA-ADD ONE-TO-MANY-------------->

    //void DataAddOneToMany()
    //{

    //    // var category = new Category() { Name = "Defterler" };

    //    var category = _context.Categories.First(x => x.Name == "Defterler"); //databasede hazır bulunan kategori sınıfını çekip öyle product ekleyebilirsin.

    //    //var product = new Product() { Name = "Defter 3", Price = 100, Stock = 200, Barcode = 123, CategoryId = category.Id };

    //    var product = new Product() { Name = "Kalem 1", Price = 100, Stock = 200, Barcode = 123, Category = category }; //burda gerçekleşen bizim veritabanımızda henüz category yok bu sebeple id yazamıyoruz
    //    //                                                                                                             //yukarda instance oluşturulan category burada veriliyor.
    //    //database kayıt yaparken bu durumda ayrıyetten _context.Category.Add(category);
    //    //yazmamıza gerek yok çünkü bizim datamız ilişkili data EF core kendisi onu da ekliyor.
    //    //ama instance oluşturulmuş olması gerek.

    //    _context.Products.Add(product);   //sadece product ekleyerek hem product hem category ekleme işlemi yaptık.PRODUCT ÜZERİNDEN CATEGORY EKLEME

    //    category.Products.Add(new Product() { Name = "Defterler 1", Price = 100, Stock = 200, Barcode = 123 }); //CATEGORY ÜZERİNDEN PRODUCT EKLEME en sonda category belirtmeye gerek yok çünkü  kategori üzerinden eklio
    //    category.Products.Add(new Product() { Name = "Defterler 2", Price = 100, Stock = 200, Barcode = 123 });
    //    _context.Add(category);


    //    var category = new Category() { Name = "Defterler" };

    //    var category = _context.Categories.First(x => x.Name == "Defterler"); //databasede hazır bulunan kategori sınıfını çekip öyle product ekleyebilirsin.

    //    var product = new Product() { Name = "Defter 3", Price = 100, Stock = 200, Barcode = 123, CategoryId = category.Id };

    //    var product = new Product() { Name = "Kalem 1", Price = 100, Stock = 200, Barcode = 123, Category = category }; //burda gerçekleşen bizim veritabanımızda henüz category yok bu sebeple id yazamıyoruz
    //                                                                                                                    //yukarda instance oluşturulan category burada veriliyor.
    //                                                                                                                    //database kayıt yaparken bu durumda ayrıyetten _context.Category.Add(category);
    //                                                                                                                    //yazmamıza gerek yok çünkü bizim datamız ilişkili data EF core kendisi onu da ekliyor.
    //                                                                                                                    //ama instance oluşturulmuş olması gerek.

    //    _context.Products.Add(product);   //sadece product ekleyerek hem product hem category ekleme işlemi yaptık.PRODUCT ÜZERİNDEN CATEGORY EKLEME

    //    category.Products.Add(new Product() { Name = "Defterler 1", Price = 100, Stock = 200, Barcode = 123 }); //CATEGORY ÜZERİNDEN PRODUCT EKLEME en sonda category belirtmeye gerek yok çünkü  kategori üzerinden eklio
    //    category.Products.Add(new Product() { Name = "Defterler 2", Price = 100, Stock = 200, Barcode = 123 });
    //    _context.Add(category);



    //    _context.SaveChanges();
    //    //_context.SaveChanges();
    //}




    //<---------------DATA-ADD ONE-TO-MANY-------------->




    //< ---------------DATA - ADD ONE - TO - ONE-------------- >

    //product => Parent   productFEature => Child

    //var category = _context.Categories.First(x => x.Name=="Silgiler");  //kategorisi olmayan produt ekleyemiyceğimiz için burada category oluşturduk.
    //var product = new Product() { Name = "Silgi 3", Price = 200, Stock = 200, Barcode = 123, Category = category,
    //    ProductFeature = new ProductFeature() { Color = "red", Height = 100, Width = 200,} //product eklerken birebir ilişkisi olan product feature da eklendi onun childi gibi düşüşn
    //};


    ////TAM TERSİ DURUM ÖNCE PRODUCTFEATURE OLUŞTUR ONU KAYDET ÖZELDEN GENELE GİT

    //ProductFeature productFeature = new ProductFeature() { Width = 300, Height = 300, Color = "Blue", 
    //    Product = new Product() { Name = "Silgi 4", Price = 200,Stock = 200, Barcode = 432, Category = category } }; 

    //_context.ProductFeatures.Add(productFeature);  //ÖNCE PRODUCTFEATURE OLUŞTURUP KAYDETTİK ÖZELDEN GENELE daha sonra product ve category EFCore tarafından kendisi eklendi.
    ////_context.Products.Add(product);
    //_context.SaveChanges();

    //Console.WriteLine("Kaydedildi");



    //< ---------------DATA - ADD ONE - TO - ONE-------------- >




    //< ---------------DATA - ADD MANY - TO - MANY-------------- >



    //ilk senaryoda önce öğrenci oluşturuluyor ve database öğrenci ekleniyor (_context.Add(student);) bu öğrenciye bağlı iki adet öğretmen EFcore tarafından ekleniyor.
    //var student = new Student() { Name = "Ahmet", Age = 23 };
    //student.Teachers.Add(new Teacher() { Name = "Ali Öğretmen" });
    //student.Teachers.Add(new Teacher() { Name = "Ayşe Öğretmen" });
    //_context.Add(student);
    //_context.SaveChanges();

    //ikinci senaryoda tam tersi öğretmen üzerinden öğrenci ekleme

    //var teacher = new Teacher() { Name="Hasan Öğretmen",Students = new List<Student>()
    //{
    //    new Student() {Name="Emir",Age=14},
    //    new Student() {Name="Bilal",Age=12}
    //}
    //};

    //_context.Add(teacher) ;
    //_context.SaveChanges();


    //üçüncü senaryoda var olan öğretmen ya da öğrenci üzerinden bir dierğini ekleme YANİ UPDATE ETME BURASI ÖNEMLİ update methodunu çağırmaya gerek yok.

    //var teacher =_context.Teachers.First(x => x.Name == "Hasan Öğretmen");
    //teacher.Students.AddRange( new List<Student>  //birden fazla aynı yerde eklemek için
    //    {   
    //    new Student() { Name = "Zeynep", Age = 10 },
    //    new Student() { Name = "Işıl", Age = 9 }
    //    }
    //);

    //_context.SaveChanges();



    //< ---------------DATA - ADD MANY - TO - MANY-------------- >



    //< ---------------DATA - DELETE - BEHAVIORS-------------- >



    //var category = new Category()
    //{
    //    Name = "Kalemler",
    //    Products = new List<Product>()
    //{

    //new Product(){ Name="kalem1" ,Price=100,Stock=200,Barcode=444},
    //new Product(){ Name="kalem2" ,Price=100,Stock=200,Barcode=444},
    //new Product(){ Name="kalem3" ,Price=100,Stock=200,Barcode=444}

    //}
    //};

    //_context.Add(category);
    //_context.SaveChanges();

    //var c = _context.Categories.First(x => x.Name == "Kalemler"); // kategoriyi sorguladık c ye atadık

    //<--------Bu kısım Restrict olduğu zaman geçerli---------->
    //var products = _context.Products.Where(x => x.CategoryId == c.Id); // davranış restrict oldğu zaman bizim kategoriyi silmemiz izin vermez önce o kategoriye bağlı products
    //_context.RemoveRange(products);                     //silinmesi gerekir ondan sonra kategori silinebilir. removeRange methodu liste halinde gönderilen verileri siler.
    //<--------Bu kısım Restrict olduğu zaman geçerli---------->

    //_context.Categories.Remove(c); //kategori silme
    //_context.SaveChanges();



    //< ---------------DATA - DELETE - BEHAVIORS-------------- >

    //< ---------------RELATED DATA LOAD-------------- >


    //<-----------EAGER LOADING----------->

    //var category = new Category() { Name = "Defterler",Products =new List<Product>() 
    //{
    //    new (){Name="Defter 1",Price=100,Stock=100,Barcode=111,ProductFeature=new (){Color="Yellow",Height=12,Width=6}},
    //    new (){Name="Defter 2",Price=100,Stock=100,Barcode=111,ProductFeature=new (){Color="Blue",Height=12,Width=6}},
    //    new (){Name="Defter 3",Price=100,Stock=100,Barcode=111,ProductFeature=new (){Color="Green",Height=12,Width=6}}
    //} 
    //};

    ////ASENKRON KULLANMAYA ÇALIŞ
    //await _context.AddAsync(category);
    ////_context.Categories.Add(category); //bir üst satırla aynı işi yapar farketmio
    //await _context.SaveChangesAsync();


    //genelden özele sorgulama
    //var categoryWithProducts = _context.Categories.Include(x => x.Products).
    //ThenInclude(x => x.ProductFeature).First();  //asıl eager loading kodu categorileri getirirken Include methodyla productları da ekledik sorguya.
    //ThenInclude methoduyla da producttan product feature erişim sağladık.
    //özelden genele sorgulama
    //var productFeatureWithProducts = _context.ProductFeatures.Include(x => x.Product).ThenInclude(x => x.Category).First();
    //ortadaki entityden sorgulama 2 tane farklı navigation property olanlar için
    //var product = _context.Products.Include(x => x.ProductFeature).Include(x => x.Category).First(); //iki tane Include kullandık farkı orada.

    //var categoryWithProducts = _context.Categories.First(); //bu kodu yazarsan products erişemezsin aşağıdaki kodda hata almazsın fakat veritabanından products verisi gelmez.

    //categoryWithProducts.Products.ForEach(product =>
    //{
    //    Console.WriteLine($"Ürün : {categoryWithProducts.Name} - {product.Id} - {product.Name} - {product.Price} - {product.Stock} - {product.Barcode}");
    //});

    //Console.WriteLine("İŞlEM BİTTİ");

    //<-----------EAGER LOADING----------->

    //<-----------EXPLICIT LOADING----------->

    //var category = _context.Categories.First();

    //if (true)
    //{
    //    _context.Entry(category).Collection(x => x.Products).Load(); //CAtegory entitysi birden fazla product sahip olduğu için collection ile girdik sorguya.
    //    category.Products.ForEach(x =>
    //    {
    //        Console.WriteLine(x.Name);
    //    });

    //}


    //var product = await _context.Products.FirstAsync();

    //if (true)
    //{
    //    _context.Entry(product).Reference(x => x.ProductFeature).Load(); //productın bire bir ilişkisi olduğu için collection değil de referans ile girdik sorguya
    //    Console.WriteLine(product.ProductFeature.Color);
    //}


    //<-----------EXPLICIT LOADING----------->


    //< ---------------LAZY LOADING-------------- >

    //var category = await _context.Categories.FirstAsync();
    //Console.WriteLine("Kategori Çekildi");
    //var products  = category.Products;
    //foreach (var product in products) 
    //{
    //    var productFeature = product.ProductFeature;  //LazyLoading in ikinci sorgusu product bilgilerini almaktı bu satırda üçüncü sorguyu yapacak product featureları alcak
    //                                                  //lazy loadingin kötü olduğu nokta her bir döngüde navigation propertye gittiği için
    //                                                  //her yeni döngüde yeni bir sorgu yazıyor.SIKINTILI DURUM!!! performans problemi
    //                                                  //(N+1) PROBLEMİ DENİR BU PROBLEME !!
    //                                                  //DOMAIN DRIVEN DESIGN DA LAZYLOADING Açık olması tavsiye edilir.
    //}

    //Console.WriteLine("İŞLEM BİTTİ");

    //< ---------------LAZY LOADING-------------- >


    //< ---------------RELATED DATA LOAD-------------- >

    //< ---------------EF CORE INHERITANCE-------------- >

    //< ---------------TPH TABLE PER HIERARCHY-------------- >

    void TablePerHiearchy()
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
    //TablePerHiearchy();

    //< ---------------TPH TABLE PER HIERARCHY-------------- >

    //< ---------------TPT TABLE PER TYPE-------------- >

    void TablePerType()
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
    //TablePerType();

    //< ---------------TPT TABLE PER TYPE-------------- >

    //< ---------------EF CORE INHERITANCE-------------- >


    //< ---------------EF CORE MODEL-------------- >

    void OwnedEntityTypes()
    {

    }
    //OwnedEntityTypes();

    void KeylessEntityTypes()
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
    //KeylessEntityTypes();

    void EntityProperties()
    {

    }
    //EntityProperties

    void Indexes()
    {
        //_context.Products.Where(x => x.Name == "Kalem 1").Select(x => new { x.Name,x.Price,x.Stock }); //Sorgu Name alanı Kalem 1 olan name price ve stock bilgilerini getir.
        var category = new Category() { Name="kağıtlar"};
        //_context.Products.Add(new Product() { Name = "kağıt1", Barcode = 1231, Price = 120, DiscountPrice = 100, Stock = 10, Url = "abab",Category=category
        //    , ProductFeature = new() { Height = 10,Color="red",Width=12}
        //});
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
        Console.WriteLine("BAŞARILI");
    }
    //Indexes(); 
    //< ---------------EF CORE MODEL-------------- >

    //< ---------------QUERY-------------- >

    void ClientServerEvaluation()
    {
        //_context.People.Add(new Person() { Name = "mami", Phone = "05554443322" });
        //_context.People.Add(new Person() { Name = "ali", Phone = "05334443322" });
        //_context.SaveChanges();

        //var people = _context.People.Where(x => FormatPhone(x.Phone) == "5554443322").ToList(); // aşağdıaki method baştaki sıfırı atıyo karşılaştırcağımız telefon da 0 sız olduğu için methodu lambda içinde vermeye
        //çalıştık ama olmadı çünkü burası sql cümleciğine dönüşemez.Bunu aşmak için Client değerlendirmesi yapmamız lazım
        var people = _context.People.ToList().Where(x => FormatPhone(x.Phone) == "5554443322").ToList(); //toList eklediğimz zaman bu kod çalışıyor çünkü memory alınıyor burası server değerlendirmesi bitmiş oluyor

        Console.WriteLine("İşlem tamamlandı");
    }
    //ClientServerEvaluation();

    // Inner Join  iki tablo arasındaki ortak alanları almak istediğimizde kullanılan join tipi iki tablo arasında NAVIGATION PROP yoksa kullanabilirsin
    //navigation propertyleri kaldırmadık data eklemek kolay olsun diye ama data okurken bunları kullanmayıp join yapısıyla çalışacağız
    void InnerJoin()
    {
        //var category = new Category() { Name = "Kalemler" };
        //category.Products.Add(new Product() { Name = "Kalem 1", Barcode = 1907, Price = 100, Stock = 10, Url = "http", ProductFeature = new() { Color = "sarı", Height = 10, Width = 6 } });
        //category.Products.Add(new Product() { Name = "Kalem 2", Barcode = 1907, Price = 100, Stock = 10, Url = "http", ProductFeature = new() { Color = "lacivert", Height = 10, Width = 6 } });
        //category.Products.Add(new Product() { Name = "Kalem 3", Barcode = 1907, Price = 100, Stock = 10, Url = "http", ProductFeature = new() { Color = "yeşil", Height = 10, Width = 6 } });
        //_context.Categories.Add(category);
        //_context.SaveChanges();
        //Console.WriteLine("İŞLEM BİTTİ");

        //join yapısı linq sorgusu yazabiliriz

        var result = _context.Categories.Join(_context.Products,x => x.Id,y => y.CategoryId, (c, p) => new  //virgülden sonraki ilk kısım yani x category denk geliyor ikinci kısım ise Product
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
            .Join(_context.ProductFeatures, x => x.p.Id, y => y.Id, (c, pf) =>  new //bu satırda da productId ile productfeature idsi joinlendi zaten o iki id identicaldi.
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
                       {c,p,pf}).ToList(); //bu şekilde bütün datalar gelcek
        result6.ForEach(x =>
        {
            var categoryName = x.c.Name;
            var productName = x.p.Name;
            var productFeatureColor = x.pf.Color;
        });
        Console.WriteLine("İŞLEM BİTTİ");
    }
    //InnerJoin();

    void LeftAndRightJoin()
    {
        var result = (from p in _context.Products
                      join pf in _context.ProductFeatures on p.Id equals pf.Id into pflist  //bu kısımda productFEatureların bi listesii temsil ediyor.Biz bütün productları almaya çalışıyoruz şuan
                      from pf in pflist.DefaultIfEmpty() //eğer productfeature boşsa defaultunu al diyoruz 4 tane product 3 tane productfeature satırımız var o eksik olan productfeature yerine null döncek.
                      select new { p, pf }).ToList();
        var result1 = (from p in _context.Products
                      join pf in _context.ProductFeatures on p.Id equals pf.Id into pflist  
                      from pf in pflist.DefaultIfEmpty() 
                      select new 
                      {
                          Product_Name = p.Name,
                          Product_Color = pf.Color, //int değerlerde bunu yaparsak uygulama patlar çünkü left join yaptık productların hepsin aldık ama bi product satırında product feature null gelecek.
                          Product_Width = (int?)pf.Width   //her zaman dolu gelmeyeceği için patlayacak bunu engellemek için NULLABLE belirtebilirsin
                          Product_Height = (int?)pf.Height == null ? 5: pf.Height //default atama eğer null ise 5 değerini ata deilse de kendi değerini ata.
                      }).ToList();

        Console.WriteLine("İŞLEM BİTTİ");
    }
    LeftAndRightJoin();

    //< ---------------QUERY-------------- >



    //<---------------DbSet METHODS------------->

    //var product1 = _context.Products.First(x => x.Id==100); //bulamazsa exception fırlatır.
    //var product2 = _context.Products.FirstOrDefault(x => x.Id == 100); //bulamazsa null döner
    //var product3 = _context.Products.FirstOrDefault(x => x.Id == 100, new Product() { Id = 1 ,Name="silgi",Price=111,Stock=1,Barcode=111}) ;   //default value de atayabiliyoruz.
    //var product4 = _context.Products.SingleAsync(x => x.Id == 7);    //databaseden birden fazla datayla sonuçlanırsa exception fırlatır. sadece tek bir data dönmesi gerek.
    //var pruduct5 = await _context.Products.Where(x=> x.Id == 11 && x.Name=="silgi").ToListAsync();  //sql sorgularaına benzer queryler yazabiliriz. ismi silgi idsi 11 olanı getir. listeye atar.
    //var product6 = await _context.Products.FindAsync(10);  // direkt primary key ile arama yapar birden fazla oprimary key varsa o tabloda parantez içinde virgülle belirtebiliriz. Null Döner bulamazsa
    //var product7 = await _context.Products.AsNoTracking().FirstAsync(x => x.Id == 7);  //update yapmayacaksak AsNoTracking kullanıp memorynin track etmemesini sağlar daha hızlı çalışma yapar
    //Console.WriteLine(_context.Entry(product7).State);  //track etmediğimiz için detached dönecek ama track etseydik veritabanıyla aynı veri olduğunu farkedip unchanged döncekti


    //<---------------DbSet METHODS------------->



    //<---------------CONFIGURATION-------------->



    //<---------------CONFIGURATION-------------->

    //products.ForEach(p =>
    //{
    //    Console.WriteLine($"{p.name}");
    //});


    // var p1 = new Product { Name = "Kalem 4", Price = 400, Stock = 40, Barcode = 456 };

    //var p1 = await _context.Products.FirstAsync();

    //p1 context.Add öncesi state
    //Console.WriteLine($"ilk state :{_context.Entry(p1).State}");

    // p1.Stock = 1000;
    //_context.Remove(p1);
    // _context.Entry(p1).State = EntityState.Deleted;  //bir üst satırdaki kod ile aynı işi yapıyor çünkü EF statelerle çalışıyor memory state atıyor daha sonra save changes
    //dediğimiz zaman bunları database üzerinde işliyor.
    //stateler buradaki veri ile databasedeki veri arasındaki fark gibi düşün
    //databaseden veri listeliyosan Unchanged state olur çünküğ yanı veri değiştirilmemiş.
    //veri silindikten sonra state i Detached olur çünkü öyle bir veri yok memoryde silindi gitti.

    // await _context.AddAsync(p1);
    //Console.WriteLine($"son state :{_context.Entry(p1).State}");

    //await _context.SaveChangesAsync();
    //Console.WriteLine($"save change s state :{_context.Entry(p1).State}");

    // var products = await _context.Products.AsNoTracking().ToListAsync(); //veritabanından alacağımız datayı filtrelemek için bütün datayı track etmemek için AsNoTracking kullan

    //products.ForEach(p =>
    //{

    //    //var state = _context.Entry(p).State;
    //    Console.WriteLine($"{p.Id} :{p.Name} -{p.Price} -{p.Stock}");    //konsola yazdırma

    //    //bir alt satırdaki kod memory de tutulan bissürü entity olabilir milyonlarca veri olabilir.
    //    //rami yormamak için biz sadece bşir entitye ait verileri track ederek programı daha hızlı çalışmasını sağlayabiliriz.

    //});



    //DATA EKLEMEK

    //_context.Products.Add(new() { Name ="Defter 1", Price = 200, Stock = 2000, Barcode = 321});
    //_context.Products.Add(new() { Name = "Defter 2", Price = 200, Stock = 2000, Barcode = 321 });
    //_context.Products.Add(new() { Name = "Defter 3", Price = 200, Stock = 2000, Barcode = 321 });


    //ORTAK değer atama drumlarında mesela tarihi datetimedan alsın biz elle yazmayalım.Merkezi bir yerden yapılacak atamalar için.

    //!!! Buradaki ChangeTracker savechanges den sürekli önce  çalışacağı için saveChanges methodunu override edip bu kod bloğunu oraya taşıdık ki kod kalabalığı olmasın.!!!
    // APPDBCONTEXT classında CREATEDDATE EKLEME İŞLEMİ VAR ORAYA BAK!!!!!!!!!!


    //_context.SaveChanges();




}


// bu methodu query konusundaClient server Evaluation için oluşturduk custom sorgu içinde denemeye çalışcaz.
string FormatPhone(string phone)
{
    return phone.Substring(1,phone.Length-1);
}
