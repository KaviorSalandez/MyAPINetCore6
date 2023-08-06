using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyAPINetCore6.Data;
using MyAPINetCore6.Models;

namespace MyAPINetCore6.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BookStoreContext _context;
        private readonly IMapper _mapper;

        public BookRepository(BookStoreContext context,IMapper mapper) {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> AddBookAsync(BookModel model)
        {
            var newBook = _mapper.Map<Book>(model);
            _context.Books!.Add(newBook);
            await _context.SaveChangesAsync();
            return newBook.Id;
        }

        public async Task DeleteBookAsync(int id)
        {
            var deleteBook = await _context.Books!.SingleOrDefaultAsync(x => x.Id == id);
               if(deleteBook != null)
                {
                    _context.Remove(deleteBook);
                    await _context.SaveChangesAsync();
                }
        }

        public async Task<List<BookModel>> GetAllBookAsync()
        {
            var books = await _context.Books!.ToListAsync();
            //ánh xạ từ danh sách books sang danh sách BookModel
            return _mapper.Map<List<BookModel>>(books);
        }

        public async Task<BookModel> GetBookByIdAsync(int id)
        {
            var book = await _context.Books!.FindAsync(id);
            return _mapper.Map<BookModel>(book);
        }
        //đáng lẽ ra phải Find sau gắn riêng từng thuộc tính bằng tay
        public async Task UpdateBookAsync(int id, BookModel model)
        {
           if(id == model.Id)
            {
                var updateBook = _mapper.Map<Book>(model);
                _context.Books!.Update(updateBook);
                await _context.SaveChangesAsync();
            }     
        }
    }
}
