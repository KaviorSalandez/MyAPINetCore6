using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyAPINetCore6.Models;
using MyAPINetCore6.Repositories;

namespace MyAPINetCore6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IBookRepository _bookRepo;

        public ProductsController(IBookRepository repo) 
        {
            _bookRepo = repo;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllBook() 
        {
            try
            {
                return Ok(await _bookRepo.GetAllBookAsync()); 
            }
            catch
            {
                return BadRequest();
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
                var book = await _bookRepo.GetBookByIdAsync(id);
                return book == null? NotFound() : Ok(book);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddNewBook(BookModel model)
        {
            try
            {
                var newBookId = await _bookRepo.AddBookAsync(model);
                var book = await _bookRepo.GetBookByIdAsync(newBookId);
                return book == null ? NotFound() : Ok(book);
            }
            catch
            {
                return BadRequest(); 
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateBook(int id,BookModel model)
        {
            try
            {
                if (id != model.Id)
                {
                    return NotFound();
                }
                await _bookRepo.UpdateBookAsync(id,model);
                return Ok();
            }
            catch
            {

                return BadRequest();
            }
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var bookDelete = await _bookRepo.GetBookByIdAsync(id);
                if(bookDelete == null)
                {
                    return NotFound();
                }
                else
                {
                    await _bookRepo.DeleteBookAsync(id);
                    return Ok(bookDelete);
                }
               
            }
            catch
            {
                return BadRequest();
            }
        }


    }
}
