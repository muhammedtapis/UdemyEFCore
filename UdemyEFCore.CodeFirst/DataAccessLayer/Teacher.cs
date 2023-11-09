using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //<---------------MANY TO MANY oluşması için iki sınıfa da List<T> eklenir-------------->
        public List<Student> Students { get; set; }   = new List<Student>(); //null hatası almamak için init ettik

    }
}
