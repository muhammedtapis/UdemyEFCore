using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.DatabaseFirst.DataAccessLayer
{
    public class Product
    {
        //veritabanındaki Product tablosuna karşılık oluşturuldu.
        public int Id { get; set; }     
        public string Name { get; set; }
        public decimal Price { get; set; }

        public int? Stock { get; set; }
    }
}
