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
using System;

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
    public ActionResult Index() //if this method incluces an async Task and await the  method should look like: public async Task<ActionResult> Index()
    {     
        // var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        // var currentUser = await _userManager.FindByIdAsync(userId);
        var userBooks = _db.Books.ToList();
        return View(userBooks);
    }
    public ActionResult Create()
    {
      ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "LastName", "AuthorId", "FirstName");
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

    public ActionResult Edit(int id)
    {
        var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "LastName");
        return View(thisBook);
    }

    [HttpPost]
    public ActionResult Edit(Book book, int AuthorId)
    {
      if (AuthorId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
      }
        _db.Entry(book).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisBook = _db.Books
        .Include(book => book.JoinEntities)
        .ThenInclude(join => join.Book)
        .FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    public ActionResult Delete(int id)
    {
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      return View(thisBook);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
    Console.WriteLine(id);
      var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
      _db.Books.Remove(thisBook);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteAuthor(int joinId)
    {
      var joinEntry = _db.AuthorBook.FirstOrDefault(entry => entry.AuthorBookId == joinId);
      _db.AuthorBook.Remove(joinEntry);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult AddAuthor(int id)
{
    var thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
    ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "LastName", "AuthorId", "FirstName");
    return View(thisBook);
}

[HttpPost]
public ActionResult AddAuthor(Book book, int AuthorId)
{
    if (AuthorId != 0)
    {
    _db.AuthorBook.Add(new AuthorBook() { AuthorId = AuthorId, BookId = book.BookId });
    }
    _db.SaveChanges();
    return RedirectToAction("Index");
}
  }

  
}