using EFAereoNuvem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFAereoNuvem.Data.Mapping;

public class ClientMap : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clients");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Cpf)
            .HasColumnType("varchar(11)")
            .HasMaxLength(11)
            .IsRequired();

        // Índice único para CPF
        builder.HasIndex(c => c.Cpf)
            .IsUnique();

        builder.Property(c => c.Name)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(c => c.Email)
            .HasColumnType("varchar(100)")
            .HasMaxLength(100);

        builder.Property(c => c.Phone)
            .HasColumnType("varchar(15)")
            .HasMaxLength(15);

        builder.Property(c => c.BornDate)
            .IsRequired();

        // Relacionamento 1:N com Reservation
        builder.HasMany(c => c.Reservations)
            .WithOne(r => r.Client)
            .HasForeignKey(r => r.ClientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Endereço atual (obrigatório) - CORRETO
        builder.HasOne(c => c.CurrentAdress)
            .WithMany()
            .HasForeignKey(c => c.CurrentAdressId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        // Endereço futuro (opcional) - CORRIGIDO
        builder.HasOne(c => c.FutureAdress)
            .WithMany()
            .HasForeignKey(c => c.FutureAdressId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); // ✅ AGORA ESTÁ CORRETAMENTE OPCIONAL
    }
}