using System.Collections.Generic;

namespace MVCLibrary.Models
{
  public class Author
  {
    public Author()
    {
      this.JoinEntities = new HashSet<AuthorBook>();
    }
    public int AuthorId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public virtual ICollection<AuthorBook> JoinEntities { get; set; }
  }
}