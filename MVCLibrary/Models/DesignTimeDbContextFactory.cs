using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MVCLibrary.Models
{
  public class MVCLibraryContextFactory : IDesignTimeDbContextFactory<MVCLibraryContext>
  {

    MVCLibraryContext IDesignTimeDbContextFactory<MVCLibraryContext>.CreateDbContext(string[] args)
    {
      IConfigurationRoot configuration = new ConfigurationBuilder()
          .SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json")
          .Build();

      var builder = new DbContextOptionsBuilder<MVCLibraryContext>();

      builder.UseMySql(configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"]));

      return new MVCLibraryContext(builder.Options);
    }
  }
}
