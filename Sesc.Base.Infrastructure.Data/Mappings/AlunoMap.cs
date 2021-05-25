using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sesc.Base.Domain.Entities;

namespace Sesc.Base.Infrastructure.Data.Mappings
{
    public class AlunoMap : IEntityTypeConfiguration<Aluno>
    {


        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Alunos");

            builder.HasKey(o => o.Id).HasName("Id");

            builder.Property(c => c.Nome)
                .HasColumnName("Nome")
                .HasColumnType("varchar(100)")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.Documento)
                .HasColumnName("Documento")
                .HasColumnType("varchar(11)")
                .HasMaxLength(11)
                .IsRequired();
        }
    }
}
