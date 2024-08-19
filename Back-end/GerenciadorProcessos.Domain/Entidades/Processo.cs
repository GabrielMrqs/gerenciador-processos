using GerenciadorProcessos.Domain.Enums;

namespace GerenciadorProcessos.Domain.Entidades
{
    public class Processo
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string AreaResponsavel { get; set; } = string.Empty;
        public string? Ferramenta { get; set; }
        public TipoProcesso Tipo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public Guid? AreaId { get; set; }
        public Area? Area { get; set; }
        public Guid? ProcessoPaiId { get; set; }
        public Processo? ProcessoPai { get; set; }
        public List<Processo> Subprocessos { get; set; } = [];
    }
}
