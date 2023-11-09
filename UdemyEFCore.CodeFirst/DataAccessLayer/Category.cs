using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    //Ana Tablomuz Produc tablosu ile ilişkilendircez.
    //<-------------RELATIONS---------->
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }    

        public List<Product> Products { get; set;} = new List<Product>();  // Category üzerinden product eklerken nullexception hatası almamak için List<Product> initialize ettik

    }
}
