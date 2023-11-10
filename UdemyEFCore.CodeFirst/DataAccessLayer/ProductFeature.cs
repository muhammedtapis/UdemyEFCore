using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    public class ProductFeature
    {
        [ForeignKey("Product")]    //burda attr. ile productFeature idsini productın foreign key valuesu olarak atadık. One to One için
        public int Id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Color { get; set; }

        //<-----------ONE TO ONE RELATION----------->
        //public int ProductId { get; set; }   //EFcore bu classın child Productın parent olduğunu belirtiyoruz burada bunun sebebi foreign key childda tutuluyor .
        public virtual Product Product { get; set; } //child entity yani productfeature üzerinden product eklenmesin istiyorsak bu alanı kaldırıyoruz.Kod hata vermez
                                             //bunu yaptığında lazyloading veya eagerloadingi kapatır yani productFEature üzerinden Product erişimi yapılamaz.
                                             //Domain Driven Design daki yaklaşımlardan bir tanesine örnektir bu durum.!!!!!!!!!

        //<-----------ONE TO ONE RELATION----------->
    }
}
