using EFAereoNuvem.Data.Mapping;
using EFAereoNuvem.Models;
using Microsoft.EntityFrameworkCore;

namespace EFAereoNuvem.Data;
public class AppDBContext : DbContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Adress> Adresses { get; set; }
    public DbSet<Flight> Flights { get; set; }
    public DbSet<Airplane> Airplanes { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Armchair> Armchairs { get; set; }
    public DbSet<Airport> Airports { get; set; }
    public DbSet<Scale> Scale { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new ClientMap());
        modelBuilder.ApplyConfiguration(new AdressMap());
        modelBuilder.ApplyConfiguration(new FlightMap());
        modelBuilder.ApplyConfiguration(new ReservationMap());

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), TypeRole = "Admin" },
            new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), TypeRole = "Client" }
        );


        // Configuração da tabela Adress
        modelBuilder.Entity<Adress>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.Street)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(a => a.Number)
                  .HasMaxLength(10);

            entity.Property(a => a.Complement)
                  .HasMaxLength(50);

            entity.Property(a => a.City)
                  .HasMaxLength(60)
                  .IsRequired();

            entity.Property(a => a.State)
                  .HasMaxLength(2)
                  .IsRequired();

            entity.Property(a => a.Country)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(a => a.Cep)
                  .HasMaxLength(9)
                  .IsRequired();
        });

        // Configuração para Flight
        modelBuilder.Entity<Flight>(entity =>
        {
            entity.Property(f => f.CodeFlight)
                  .HasMaxLength(10)
                  .IsRequired();

            entity.Property(f => f.Origin)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(f => f.Destination)
                  .HasMaxLength(50)
                  .IsRequired();
        });

        // Relacionamento entre User e Role (Muitos para Muitos)
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("UserRoles"));
    }
}