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
  public class AuthorsController : Controller
  {
    private readonly MVCLibraryContext _db;
    private readonly UserManager<LibrarianUser> _userManager;

    public AuthorsController(UserManager<LibrarianUser> userManager, MVCLibraryContext db)
    {
      _userManager = userManager;
      _db = db;
    }
    public async Task<ActionResult> Index()
    {
        var userAuthors = _db.Authors.ToList();
        return View(userAuthors);
    }
    public ActionResult Create()
    {
      //remove this viewbag and selectlist on view if we want to be able to create an author without a book from drop down. then create AddBook get and post methods and views
      ViewBag.BookId = new SelectList(_db.Books, "BookId", "Title");
      return View();
    }
    [HttpPost]
    public async Task<ActionResult> Create(Author author, int BookId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      author.User = currentUser;
      _db.Authors.Add(author);
      _db.SaveChanges();
      if (BookId != 0)
      {
          _db.AuthorBook.Add(new AuthorBook() { BookId = BookId, AuthorId = author.AuthorId });
      }
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Edit(int id)
    {
        var thisAuthor = _db.Authors.FirstOrDefault(Author => Author.AuthorId == id);
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "LastName");
        return View(thisAuthor);
    }

    [HttpPost]
    public ActionResult Edit(Author author, int BookId)
    {
      if (BookId != 0)
      {
        _db.AuthorBook.Add(new AuthorBook() { BookId = BookId, AuthorId = author.AuthorId });
      }
        _db.Entry(author).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var thisAuthor = _db.Authors
        .Include(Author => Author.JoinEntities)
        .ThenInclude(join => join.Author)
        .FirstOrDefault(Author => Author.AuthorId == id);
      return View(thisAuthor);
    }

    public ActionResult Delete(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(Author => Author.AuthorId == id);
      return View(thisAuthor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var thisAuthor = _db.Authors.FirstOrDefault(Author => Author.AuthorId == id);
      _db.Authors.Remove(thisAuthor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}