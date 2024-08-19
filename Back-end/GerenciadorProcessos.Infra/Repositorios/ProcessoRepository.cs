using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Infra.Shared;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorProcessos.Infra.Repositorios
{
    public class ProcessoRepository : BaseRepository<Processo>, IProcessoRepository
    {
        public ProcessoRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Processo?> GetByIdAsync(Guid id)
        {
            var processo = await _dbSet.Include(x => x.ProcessoPai).FirstOrDefaultAsync(x => x.Id == id);

            if (processo is not null)
            {
                await LoadSubprocessosRecursivamente(processo);
            }

            return processo;
        }

        public override async Task<IEnumerable<Processo>> GetAllAsync()
        {
            return await _dbSet.Include(x => x.Subprocessos).ToListAsync();
        }

        public async Task<IEnumerable<Processo>> FindAllByIds(IEnumerable<Guid> subprocessoIds)
        {
            return await _dbSet.Where(x => subprocessoIds.Contains(x.Id)).ToListAsync();
        }

        private async Task LoadSubprocessosRecursivamente(Processo processo)
        {
            await _context.Entry(processo)
                          .Collection(p => p.Subprocessos)
                          .LoadAsync();

            foreach (var subprocesso in processo.Subprocessos)
            {
                await LoadSubprocessosRecursivamente(subprocesso);
            }
        }
    }
}
