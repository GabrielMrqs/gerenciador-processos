using GerenciadorProcessos.Domain.Entidades;

namespace GerenciadorProcessos.Domain.Repositorios
{
    public interface IProcessoRepository : IBaseRepository<Processo>
    {
        Task<IEnumerable<Processo>> FindAllByIds(IEnumerable<Guid> subprocessoIds);
    }
}
