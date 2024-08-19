namespace GerenciadorProcessos.Domain.DTOs
{
    public class AreaDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<ProcessoDTO> Processos { get; set; } = [];
    }
}
