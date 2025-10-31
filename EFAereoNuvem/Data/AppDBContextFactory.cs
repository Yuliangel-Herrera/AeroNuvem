using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EFAereoNuvem.Data;
public class AppDBContextFactory : IDesignTimeDbContextFactory<AppDBContext>
{
    public AppDBContext CreateDbContext(string[] args)
    {
        // Carrega o appsettings.json (mesmo em tempo de design)
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Recupera a connection string 
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        var optionsBuilder = new DbContextOptionsBuilder<AppDBContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AppDBContext(optionsBuilder.Options);
    }
}
