﻿using Microsoft.EntityFrameworkCore;
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
        
        [Precision(18, 2)]
        public decimal  Price { get; set; }
        public int? Width { get; set; }  // bu tabloda null gelebilcek alanları olabilir left right joinden sonra ona dikkat et.
        public int? Height { get; set; }

    }
}
