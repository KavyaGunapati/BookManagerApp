using BookManagerApp.Data;
using BookManagerApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

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
        public async Task<IActionResult> List(string sortOrder, string SearchTerm, int? page)
        {
            var books = await _context.Books.ToListAsync();
            if (books == null)
            {
                return NotFound("No book found");
            }
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                books = books.Where(b => b.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || b.Author.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            if (sortOrder == "title_desc")
            {
                books = books.OrderByDescending(b => b.Title).ToList();
            }
            else if (sortOrder == "author_asc")
            {
                books = books.OrderBy(b => b.Author).ToList();
            }
            else if (sortOrder == "price_desc")
            {
                books = books.OrderByDescending(b => b.Price).ToList();
            }
            else
            {
                books = books.OrderBy(b => b.Title).ToList();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(books.ToPagedList(pageNumber, pageSize));
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
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book == null)
            {
                return NotFound("Book not found");
            }
            return View(book);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var book = await _context.Books.FindAsync(id);
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
