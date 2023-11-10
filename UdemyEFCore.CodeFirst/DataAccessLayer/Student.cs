using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    public class Student
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        //<---------------MANY TO MANY oluşması için iki sınıfa da List<T> eklenir-------------->
        public virtual List<Teacher> Teachers { get; set; } = new List<Teacher>();  //ekleme işlemi yaparken listeye null hatası almamak için initialize ettik.

        //virtual keywordu Lazyloading yaparken EFCore propertyleri override edeceği için yapılıyor.
    }
}
