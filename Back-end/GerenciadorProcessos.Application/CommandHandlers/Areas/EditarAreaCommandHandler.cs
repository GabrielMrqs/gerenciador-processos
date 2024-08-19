using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;
using System.Linq.Expressions;

namespace GerenciadorProcessos.Application.CommandHandlers.Areas
{
    public class EditarAreaCommandHandler : IRequestHandler<EditarAreaCommand, Result<Unit, Exception>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IProcessoRepository _processoRepository;

        public EditarAreaCommandHandler(IAreaRepository areaRepository, IProcessoRepository processoRepository)
        {
            _areaRepository = areaRepository;
            _processoRepository = processoRepository;
        }

        public async Task<Result<Unit, Exception>> Handle(EditarAreaCommand request, CancellationToken cancellationToken)
        {
            var area = await _areaRepository.GetByIdAsync(request.Id);
            if (area is null)
            {
                return new Exception($"Área {request.Id} não existe.");
            }

            Expression<Func<Area, bool>> nomeIgualEIdDiferente = a => a.Nome.ToUpper().Equals(request.Nome.ToUpper()) && a.Id != request.Id;
            var areaExistente = await _areaRepository.AnyAsync(nomeIgualEIdDiferente);
            if (areaExistente)
            {
                return new Exception($"Área '{request.Nome}' já existente.");
            }

            area.Nome = request.Nome;
            area.Descricao = request.Descricao;
            area.Processos.Clear();

            if (request.Processos is not null)
            {
                var processos = await _processoRepository.FindAllByIds(request.Processos);
                if (processos.Any(x => x.AreaId != null && x.AreaId != area.Id))
                {
                    return new Exception("Um ou mais processos especificados já estão atrelados a outras áreas.");
                }
                if (processos.Count() != request.Processos?.Count())
                {
                    return new Exception("Um ou mais processos especificados não existem.");
                }

                area.Processos = processos.ToList();
            }

            try
            {
                _areaRepository.Update(area);
                await _areaRepository.SaveChangesAsync();
                return Unit.Value;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
