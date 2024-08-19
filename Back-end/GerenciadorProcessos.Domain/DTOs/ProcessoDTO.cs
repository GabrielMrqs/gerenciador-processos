using GerenciadorProcessos.Domain.Enums;

namespace GerenciadorProcessos.Domain.DTOs
{
    public class ProcessoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public TipoProcesso Tipo { get; set; }
        public string AreaResponsavel { get; set; }
        public string Ferramenta { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }
        public ICollection<ProcessoDTO>? Subprocessos { get; set; } = [];
    }
}
