using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.Models
{
    public class ProductAllWithFeature
    {
        //function kısmında data karşılamak için oluşturduk
        public int Id { get; set; }
        public string Name { get; set; }
        public int? Width { get; set; }  // bu tabloda null gelebilcek alanları olabilir left right joinden sonra ona dikkat et.
        public int? Height { get; set; }
    }
}
