﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UdemyEFCore.CodeFirst.DataAccessLayer;

namespace UdemyEFCore.CodeFirst
{
    public class DbContextInitializer
    {

        public static IConfigurationRoot Configuration;  //bu Interface üzerinde appsettings.json dosyasındakileri okuyabilmek için.

        public static DbContextOptionsBuilder<AppDbContext> OptionsBuilder;  //veritabanı ile ilgili optionsları belirteceğimiz yer.

        public static void Build()
        {
            
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true); //önce appsettings dosyasını al.
            //bu işleleri yapmamızın sebebi connection stringi appsettings.json dosyası oluşturup onun içinde belirttik şimdi de oradan okuyacağız.
            Configuration = builder.Build();  //okuyabileceğimiz appsettings.json dosyasını hazır hale getirdik.


        }
    }
}
