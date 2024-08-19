namespace GerenciadorProcessos.Domain.Entidades
{
    public class Area
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<Processo> Processos { get; set; } = [];
    }
}
