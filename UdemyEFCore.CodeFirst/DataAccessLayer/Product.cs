using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    //<---------------CONFIGURATION-------------->

   // [Table("ProductTable",Schema ="products")]   //tablo ve şema ismi burada verilebilir.  Validation Kısmı için yapılabilir.

    //<---------------CONFIGURATION-------------->
    public class Product
    {
        public int Id { get; set; }

        //<---------------CONFIGURATION--------------> 
        //[Key]  // alttaki propun key olduğunu belirtir. aşağıdaki yazım şeklini EF key olarak algılamaz.
        //public int Product_Id { get; set; }

        //  [Column("name2",Order = 3)]  //column ismi değiştir veritabanı için orderin çalışabilmesi için tablonun sıfırdan oluşması gerekir.
        //<---------------CONFIGURATION-------------->
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        //yeni eklenen prop tekrar migration oluşturmak gerekiyor.
        public int Barcode { get; set; }

        //  public DateTime? CreatedDate { get; set; } //nullable yaptık


        //<--------RElATIONS NAVIGATION PROPERTIES------------->

        //<-----------ONE TO MANY RELATION----------->
        public int CategoryId { get; set; }  //yazım şekli olarak ilişki kuracağımız (bir üst Entity)Id şeklinde yazarsak bunu EFCore tanıyabiliyor.

      
        //public int Category_Id { get; set; }  //böyle yazdığımızda bunu foreign key olarak tanıyamaz EFCore ona tanıtmamız gerekir. AppDbContext de OnModelCreating de yazılır.
       
        //Bir diğer yöntem ise data annotation attr.
       // [ForeignKey("Category_Id")]  // bu tablo Category tablosuyla ilişkilendirceği için Category türündeki propun üstüne yazdık ve yukardaki Category_Id foreign key verdik. 
        public Category Category { get; set; }  //foreign key


        //<-----------ONE TO MANY RELATION----------->


        //<-----------ONE TO ONE RELATION----------->
        public ProductFeature ProductFeature { get; set; } //direkt böyle yazdığımzda efcore aralarındaki ilişkiyi algılayamıyor.Bire çokta algılayabilir.çünkü listeleme yapıyoruz.

        //<-----------ONE TO ONE RELATION----------->

    }
}
