// See https://aka.ms/new-console-template for more information


using Microsoft.EntityFrameworkCore;
using UdemyEFCore.DatabaseFirst.DataAccessLayer;

// usingkullanmanın sebebi işlem bittiğinde dispose olsun memoryde yer kaplıo çok.
// database işlemi yapcaksak AppDbContext instance örneği oluşturmamız laızm.

DbContextInitializer.Build();   // optionsbuilderdaki Build Methodunu çağırmazsak db çalışmaz DbContextOptionsBuilder ayarları onun içinde
                                //methodu stati oluşturduğumuz için Direkt sınıf üzerinden nesne oluşturmadan ulaşabiliyoruz.!!!!
                                //method bir kere çalışacak uygulama ayakta olduğu süre boyunca!! static yaptık o yüzden.

using (var _context = new AppDbContext())     //new AppDbContext(DbContextInitializer.OptionsBuilder.Options) farklı db kullancağın zaman ayarlarını böyle vermen gerek
                                                                                         //AppDbContextteki ctor parametre olarak DbContextOptionsnesnesi alıyo burada onu verdik.
                                                                                         //Bu parantez içi sürekli farklı ayar göndermek istediğiniz zaman böyle kullanıyoruz sade ctor olduğu zaman
                                                                                         //appdbcontext içinde override ettiğimiz bir method var. yani tek bşr veritabanına bağlancaksan projende
                                                                                         //Boş ctor çalıştır burada AppDbContext() sonra da AppDbContextte OnConfiguring methodunu override et.
{
    var products  = await _context.Products.ToListAsync();

    foreach (var item in products)
    {
        Console.WriteLine($"{item.Id} : {item.Name}");
    }

    Console.WriteLine("-----------------");

    // üsttekiyle aynı kod
    products.ForEach(p => 
    {
        Console.WriteLine($"{p.Id} : {p.Name} - {p.Price} - {p.Stock}");
    });
}
