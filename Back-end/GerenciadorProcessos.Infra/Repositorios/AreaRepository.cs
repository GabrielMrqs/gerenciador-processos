using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Infra.Shared;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorProcessos.Infra.Repositorios
{
    public class AreaRepository : BaseRepository<Area>, IAreaRepository
    {
        public AreaRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<Area?> GetByIdAsync(Guid id)
        {
            var area = await _dbSet.Include(x => x.Processos)
                                   .FirstOrDefaultAsync(x => x.Id == id);

            if (area is not null)
            {
                foreach (var processo in area.Processos)
                {
                    await LoadSubprocessosAsync(processo);
                }
            }

            return area;
        }

        private async Task LoadSubprocessosAsync(Processo processo)
        {
            await _context.Entry(processo).Collection(p => p.Subprocessos).LoadAsync();

            foreach (var subprocesso in processo.Subprocessos)
            {
                await LoadSubprocessosAsync(subprocesso);
            }
        }
    }
}
