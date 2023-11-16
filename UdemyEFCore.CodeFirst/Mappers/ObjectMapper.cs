using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using UdemyEFCore.CodeFirst.DataAccessLayer;
using UdemyEFCore.CodeFirst.DTOs;

namespace UdemyEFCore.CodeFirst.Mappers
{

    //AUTO MAPPER İÇİN OLUŞTURULDU CONSOLE UYGULAMASI OLDUĞU İÇİN KENDİMİZ OLUŞTURDUK API YA DA MVC UYGULAMASINDA STARTUP YA DA PROGRAM.CS dosyasında ekleyebiliriz
    internal class ObjectMapper
    {
         //yükleme işlemini LazyLoading ile yapıcaz yani ihtiyaç olduğunda yüklensin
         private static Lazy<IMapper> lazy = new Lazy<IMapper>(() => //normalde statik tipler uygulama ayağa kalkar kalkmaz memory yüklenir ancak biz burada lazy yaptığımız için 
                                                                    // ObjectMapper.Mapper olarak çağırdığımzda  burası yüklenecek.
         {
             var config = new MapperConfiguration(cfg =>
             {
                 cfg.AddProfile<CustomMapping>();  //her ayrı mapping dosyası eklenecek bu şekilde.
             });
             return config.CreateMapper();
         });

        public static IMapper Mapper => lazy.Value;  //yukarıdaki methoda ObjectMapper uzerinden erişemezsin private ona erişebilmek için bu methodu oluşturduk.


    }

    internal class CustomMapping:Profile //automapperdan gelen Profile miras alıyor, kimi kime mapleyeceğimizi belirttiğimiz mapleme classı farklı farklı mappingler için farklı classlar oluşturan gerek
    {
        public CustomMapping()
        {
            CreateMap<ProductDTOAutoMapper, Product>().ReverseMap(); //iki yönlü de mapleme yaptık.
        }
    }
}
