using Concurrency.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Concurrency.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public async Task <IActionResult> Update(int id)  //update işlemi yapcaz
        {
            var product = await _context.Products.FindAsync(id);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Product product)
        {
            try
            {
                _context.Products.Update(product);  //iki user update edicek bi satırı birincisi update ettikten sonra ikinci update etmeye çalışırsa
                await _context.SaveChangesAsync();  //row version kullandığımız için EFCORE DBCONCURRENCY() exception fırlatıcak!!!!

                return RedirectToAction(nameof(List)); //listeleme sayfasına geri dön updateden sonra
            }
            catch (DbUpdateConcurrencyException exception) //dbupdateconcurrency gerçekleşirse burayı çalıştır exception at
            {
                var exceptionEntry = exception.Entries.First(); //concurrency hatasından ilk etkilenen satırı aldık.

                var currentProduct = exceptionEntry.Entity as Product; //hatanın gerçekleştiği entity anlık değer kullanıcının gönderdiği değerler

                var databaseValues = exceptionEntry.GetDatabaseValues();  //databasedeki güncel değerler property list dönüyo          
                
                var clientValues = exceptionEntry.CurrentValues;   //clientdeki veri anlık


                if (databaseValues == null)  //eğer bu veri databasede yoksa silinmiş demektir yine hata mesajı döncez.
                {
                    ModelState.AddModelError(string.Empty,"Bu ürün başka bir kullanıcı tarafından silindi!!!");
                }
                else
                {
                    var databaseProduct = databaseValues.ToObject() as Product;  //databasedeki güncel değerleri objeye çevir ve product ata ,databasevalues değeri null dönebilir o yzden burada olcak bu kod

                    ModelState.AddModelError(string.Empty, "Bu ürün başka bir kullanıcı tarafından güncellendi!!!");

                    ModelState.AddModelError(string.Empty, $"Güncellenen Değer ; Name : {databaseProduct.Name}, Price : {databaseProduct.Price}, Stock : {databaseProduct.Stock}");
                }
                return View(product); //hata mesajından sonra tekrar ekranı göster ve product nesnesini dön ki update sayfasındaki alanlar o entity bilgileriyle dolsun
            }


        }

        public async Task<IActionResult> List()
        {
           return View(await _context.Products.ToListAsync());
        }
    }
}
