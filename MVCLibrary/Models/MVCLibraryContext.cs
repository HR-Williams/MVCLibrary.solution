using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MVCLibrary.Models
{
  public class MVCLibraryContext : IdentityDbContext<LibrarianUser>

  {
    public virtual DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<AuthorBook> AuthorBook { get; set; }
    public DbSet<Copy> Copy { get; set; }
    public MVCLibraryContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseLazyLoadingProxies();
    }
  }
}