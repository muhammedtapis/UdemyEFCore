using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdemyEFCore.CodeFirst.DataAccessLayer
{
    public class Employee:BasePerson //hiyerarşi örneği
    {
        //Miras Alma Durumunda EFCORE davranışlarını inceliycez bu sebeple oluşturduk bu sınıfı  TPH TPT

        [Precision(18,2)]
        public decimal Salary { get; set; }
    }
}
