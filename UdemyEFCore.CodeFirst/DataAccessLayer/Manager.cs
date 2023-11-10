using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
     public class Manager :BasePerson
    {
        //Miras Alma Durumunda EFCORE davranışlarını inceliycez bu sebeple oluşturduk bu sınıfı TPH TPT
        public int Grade { get; set; }
    }
}
