using EFAereoNuvem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFAereoNuvem.Data.Mapping;

public class FlightMap : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("Flights");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Id)
            .ValueGeneratedOnAdd();

        builder.Property(f => f.CodeFlight)
            .HasColumnName("CodeFlight")
            .HasColumnType("varchar(10)")
            .HasMaxLength(10)
            .IsRequired();

        // Origem
        builder.Property(f => f.Origin)
            .HasColumnName("Origin")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        // Destino
        builder.Property(f => f.Destination)
            .HasColumnName("Destination")
            .HasColumnType("varchar(50)")
            .HasMaxLength(50)
            .IsRequired();

        // Datas e horários
        builder.Property(f => f.DateFlight)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(f => f.Arrival)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(f => f.Departure)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(f => f.RealArrival)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(f => f.RealDeparture)
            .HasColumnType("datetime")
            .IsRequired();

        builder.Property(f => f.ExistScale)
            .HasColumnType("bit")
            .IsRequired();

        // Duração do voo
        builder.Property(f => f.Duration)
            .HasColumnType("float")
            .IsRequired();

        // Relacionamento 1:N com Reservation
        builder.HasMany(f => f.Reservations)
            .WithOne(r => r.Flight)
            .HasForeignKey(r => r.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento 1:N com Scale
        builder.HasMany(f => f.Scales)
            .WithOne(s => s.Flight)
            .HasForeignKey(s => s.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacionamento N:1 com Airplane
        builder.HasOne(f => f.Airplane)
            .WithMany(a => a.Flights)
            .HasForeignKey(f => f.AirplaneId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
