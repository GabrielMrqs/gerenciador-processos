using GerenciadorProcessos.Domain.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GerenciadorProcessos.Infra.Mappings
{
    public class AreaMapping : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Nome).IsRequired();

            builder.Property(p => p.Descricao).IsRequired();

            builder.ToTable("Areas");
        }
    }
}
