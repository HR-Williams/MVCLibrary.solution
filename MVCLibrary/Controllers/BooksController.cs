using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using MVCLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MVCLibrary.Controllers
{
  [Authorize]
  public class BooksController : Controller
  {
    private readonly MVCLibraryContext _db;
    private readonly UserManager<LibrarianUser> _userManager;

    public BooksController(UserManager<LibrarianUser> userManager, MVCLibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public async Task<ActionResult> Index()
    {
        // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // var currentUser = await _userManager.FindByIdAsync(userId);
        var userBooks = _db.Books.ToList();
        return View(userBooks);
    }
    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "LastName");
      return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(Book book, int AuthorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      book.User = currentUser;
      _db.Books.Add(book);
      _db.SaveChanges();
      if (AuthorId != 0)
      {
          _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}