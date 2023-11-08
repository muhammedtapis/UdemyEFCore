using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    public class Product
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        //yeni eklenen prop tekrar migration oluşturmak gerekiyor.
        public int Barcode { get; set; }  

        public DateTime? CreatedDate { get; set; } //nullable yaptık
    }
}
