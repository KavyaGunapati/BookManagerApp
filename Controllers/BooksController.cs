using BookManagerApp.Data;
using BookManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagerApp.Controllers
{
    public class BooksController : Controller
    {
        private readonly AppDbContext _context;
        public BooksController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
              await  _context.Books.AddAsync(book);
              await  _context.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(book);
        }
        [HttpGet]
        public async Task<IActionResult> List()
        {
            var books =await _context.Books.ToListAsync();
            if(books == null)
            {
                return NotFound("No book found");
            }
            return View( books);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b=>b.Id==id);
            if (book==null)
            {
                return NotFound("Book not found");
            }
            return View(book);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Book book)
        {
           
            if (ModelState.IsValid)
            {
                _context.Books.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(book);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Book book)
        {
            var bookExist = await _context.Books.FindAsync(book.Id);
            if (book == null)
            {
                return NotFound("Book not found");
            }
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction("List");
        }
    }
}
