// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using UdemyEFCore.CodeFirst;
using UdemyEFCore.CodeFirst.DataAccessLayer;


DbContextInitializer.Build();

using(var _context = new AppDbContext())
{

    //<---------------DATA-ADD ONE-TO-MANY-------------->

    // var category = new Category() { Name = "Defterler" };

    //var category = _context.Categories.First(x => x.Name=="Defterler"); //databasede hazır bulunan kategori sınıfını çekip öyle product ekleyebilirsin.

    //var product = new Product() { Name = "Defter 3", Price = 100, Stock = 200, Barcode = 123, CategoryId = category.Id };

    //var product = new Product() { Name = "Kalem 1", Price = 100, Stock = 200, Barcode = 123 ,Category=category}; //burda gerçekleşen bizim veritabanımızda henüz category yok bu sebeple id yazamıyoruz
    //                                                                                                             //yukarda instance oluşturulan category burada veriliyor.
    //database kayıt yaparken bu durumda ayrıyetten _context.Category.Add(category);
    //yazmamıza gerek yok çünkü bizim datamız ilişkili data EF core kendisi onu da ekliyor.
    //ama instance oluşturulmuş olması gerek.

    //_context.Products.Add(product);   //sadece product ekleyerek hem product hem category ekleme işlemi yaptık.PRODUCT ÜZERİNDEN CATEGORY EKLEME

    //category.Products.Add(new Product() { Name = "Defterler 1", Price = 100, Stock = 200, Barcode = 123}); //CATEGORY ÜZERİNDEN PRODUCT EKLEME en sonda category belirtmeye gerek yok çünkü  kategori üzerinden eklio
    //category.Products.Add(new Product() { Name = "Defterler 2", Price = 100, Stock = 200, Barcode = 123 });
    //_context.Add(category);



    //_context.SaveChanges();


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

    var teacher =_context.Teachers.First(x => x.Name == "Hasan Öğretmen");
    teacher.Students.AddRange( new List<Student>  //birden fazla aynı yerde eklemek için
        {   
        new Student() { Name = "Zeynep", Age = 10 },
        new Student() { Name = "Işıl", Age = 9 }
        }
    );

    _context.SaveChanges();

    Console.WriteLine("Kaydedildi");

    //< ---------------DATA - ADD MANY - TO - MANY-------------- >



    
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
