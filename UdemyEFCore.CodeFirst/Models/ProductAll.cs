using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.Models
{
    public class ProductAll
    {
        public int Id { get; set; }  //AppDbContext içinde FluentAPI ile bunun primary key olmadığını belirttik.
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

    }
}
