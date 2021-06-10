using System.Collections.Generic;

namespace MVCLibrary.Models
{
  public class Book
  {
    public Book()
    {
      this.JoinEntities = new HashSet<AuthorBook>();
    }

    public int BookId { get; set; }
    public string Title { get; set; }
    public virtual LibrarianUser User { get; set; }
    // public virtual Author Author { get; set; }
    public virtual ICollection<AuthorBook> JoinEntities { get; }
  }
}