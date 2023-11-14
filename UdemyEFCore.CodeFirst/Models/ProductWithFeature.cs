using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.Models
{
    public class ProductWithFeature
    {
        public int Id { get; set; }  //AppDbContext içinde FluentAPI ile bunun primary key olmadığını belirttik.
        public string Name { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int  Height { get; set; }
        public string Color { get; set; }
    }
}
