using GerenciadorProcessos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorProcessos.Infra.Mappings
{
    public class ProcessoMapping : IEntityTypeConfiguration<Processo>
    {
        public void Configure(EntityTypeBuilder<Processo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Nome).IsRequired();

            builder.Property(p => p.Descricao).IsRequired();

            builder.Property(p => p.AreaResponsavel).IsRequired();

            builder.Property(p => p.Tipo).IsRequired();

            builder.HasOne(p => p.Area)
                   .WithMany(a => a.Processos)
                   .HasForeignKey(p => p.AreaId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.ProcessoPai)
                   .WithMany(p => p.Subprocessos)
                   .HasForeignKey(p => p.ProcessoPaiId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Processos");
        }
    }

}
