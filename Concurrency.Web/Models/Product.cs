using System.ComponentModel.DataAnnotations;

namespace Concurrency.Web.Models
{
    public class Product
    {
        public int Id { get; set; }
        public  string  Name  { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        //[Timestamp] //zaman damgası ile değişiklliği tutabiliriz. bu birinci yol biz fluentAPI de yaptık
        public byte[] RowVersion { get; set; }  //update işlemi yapınca efcoreun değişiklibilgisini tutacağı prop.


    }
}
