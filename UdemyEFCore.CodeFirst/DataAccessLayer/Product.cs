using Microsoft.EntityFrameworkCore;
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

    //INDEKSLEME
    //[Index(nameof(Name))]  //tip güvemli bir şekilde indeksleme yapacağımız prop. tanımı.
    //[Index(nameof(Name),nameof(Price))]   //composed index denir buna bu türde oluşturulan indekste iki propertynin de sütunları bulunur.daha çok ikisini bir sorguda kullanıyorsan mantıklı.
    public class Product
    {
        public int Id { get; set; }

        //<---------------CONFIGURATION--------------> 
        //[Key]  // alttaki propun key olduğunu belirtir. aşağıdaki yazım şeklini EF key olarak algılamaz.
        //public int Product_Id { get; set; }

        //  [Column("name2",Order = 3)]  //column ismi değiştir veritabanı için orderin çalışabilmesi için tablonun sıfırdan oluşması gerekir.
        //<---------------CONFIGURATION-------------->
        public string? Name { get; set; }

        [Precision(18,2)] // migration eklerken aldığın sarı uyarı için bu decimal verdin ama kaç karakter virgülden sonra kaç karakter var onu belirt diyor.
        public decimal Price { get; set; }

        [Precision(9, 2)]
        public decimal DiscountPrice { get; set; }
        public int Stock { get; set; }


        //[NotMapped]//bu alanı db de tutmak istemiyorsak maplemeyi kapatıcaz.Entity Properties özelliği.Fluent API ile yaptık.
        public int Barcode { get; set; }

        //[Column(TypeName ="varchar(500)")]  //veritabanındaki alanın tipini belirtebiliyoruz burda URL için 
        public string? Url { get; set; }

        //  public DateTime? CreatedDate { get; set; } //nullable yaptık

        //<--------RElATIONS NAVIGATION PROPERTIES------------->

        //<-----------ONE TO MANY RELATION----------->
        public int CategoryId { get; set; }  //yazım şekli olarak ilişki kuracağımız (bir üst Entity)Id şeklinde yazarsak bunu EFCore tanıyabiliyor.

        //delete behaviors SetNull uygularken categoryId nullable olması gerek (?) eğer categoryId nullable yaparsan aşağıdaki category de nullable yapman gerekicek
      
        //public int Category_Id { get; set; }  //böyle yazdığımızda bunu foreign key olarak tanıyamaz EFCore ona tanıtmamız gerekir. AppDbContext de OnModelCreating de yazılır.
       
        //Bir diğer yöntem ise data annotation attr.
       // [ForeignKey("Category_Id")]  // bu tablo Category tablosuyla ilişkilendirceği için Category türündeki propun üstüne yazdık ve yukardaki Category_Id foreign key verdik. 
        public virtual Category Category { get; set; }  //foreign key


        //<-----------ONE TO MANY RELATION----------->


        //<-----------ONE TO ONE RELATION----------->

        public virtual ProductFeature ProductFeature { get; set; } //direkt böyle yazdığımzda efcore aralarındaki ilişkiyi algılayamıyor.Bire çokta algılayabilir.çünkü listeleme yapıyoruz.

        //<-----------ONE TO ONE RELATION----------->

        //< ---------------DATABASE - GENERATED - ATTRIBUTES-------------- >

        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // IDENTITY attr İÇİN !! Datetime propu sadece insert edildiğinde çalışacak. update edildiğinde çalışmayacak bu onu belirtiyor.
        //public DateTime? CreatedDate { get; set; } = DateTime.Now;

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)] //COMPUTED ATTR İÇİN !! bu alanda insert işlemini sql bırakacağımız için AppDbContext onMOdelCreating belirtmemiz gerek.
        //public decimal PriceStock {  get; set; }    

        //< ---------------DATABASE - GENERATED - ATTRIBUTES-------------- >

        //< ---------------GLOBAL QUERY FILTER-------------- >
        public bool IsDeleted { get; set; }

        //< ---------------GLOBAL QUERY FILTER-------------- >

    }
}
