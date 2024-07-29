using AutoMapper;
using MyAPINetCore6.Data;
using MyAPINetCore6.Models;

namespace MyAPINetCore6.Helpers
{
    public class ApplicationMapper: Profile // Profile của thằng thư viện automapper
    {
        public ApplicationMapper() {
            CreateMap<Book, BookModel>().ReverseMap();
            // Notification 

        }
    }
}
