using EFAereoNuvem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFAereoNuvem.Data.Mapping;

public class AdressMap : IEntityTypeConfiguration<Adress>
{
    public void Configure(EntityTypeBuilder<Adress> builder)
    {
        builder.ToTable("Adresses");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd();

        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)")
            .HasColumnName("Street");

        builder.Property(a => a.Number)
            .HasColumnType("varchar(10)")
            .HasColumnName("Number");

        builder.Property(a => a.Complement)
            .HasColumnType("varchar(50)")
            .HasColumnName("Complement");

        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(60)
            .HasColumnType("varchar(60)")
            .HasColumnName("City");

        builder.Property(a => a.State)
            .IsRequired()
            .HasMaxLength(2)
            .HasColumnType("char(2)")
            .HasColumnName("State");

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("varchar(50)")
            .HasColumnName("Country");

        builder.Property(a => a.Cep)
            .IsRequired()
            .HasMaxLength(9)
            .HasColumnType("varchar(9)")
            .HasColumnName("Cep");
    }
}
