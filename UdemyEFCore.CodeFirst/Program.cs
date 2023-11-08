// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using UdemyEFCore.CodeFirst;
using UdemyEFCore.CodeFirst.DataAccessLayer;


DbContextInitializer.Build();

using(var _context = new AppDbContext())
{

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

    _context.Products.Add(new() { Name ="Defter 1", Price = 200, Stock = 2000, Barcode = 321});
    _context.Products.Add(new() { Name = "Defter 2", Price = 200, Stock = 2000, Barcode = 321 });
    _context.Products.Add(new() { Name = "Defter 3", Price = 200, Stock = 2000, Barcode = 321 });


    //ORTAK değer atama drumlarında mesela tarihi datetimedan alsın biz elle yazmayalım.Merkezi bir yerden yapılacak atamalar için.

    //!!! Buradaki ChangeTracker savechanges den sürekli önce  çalışacağı için saveChanges methodunu override edip bu kod bloğunu oraya taşıdık ki kod kalabalığı olmasın.!!!
    // APPDBCONTEXT classında CREATEDDATE EKLEME İŞLEMİ VAR ORAYA BAK!!!!!!!!!!
  

    _context.SaveChanges();

}
