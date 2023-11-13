using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    //[Owned] owned entity type diğer clsslarda ortak olan özellikleri topladığın sınıf ama inherit alma durumu yok.
    public class BasePerson
    {
        //Miras Alma Durumunda EFCORE davranışlarını inceliycez bu sebeple oluşturduk bu sınıfı TPH TPT

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
    }
}
