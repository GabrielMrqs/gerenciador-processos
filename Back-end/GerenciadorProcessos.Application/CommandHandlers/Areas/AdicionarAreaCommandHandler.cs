using AutoMapper;
using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Areas
{
    public class AdicionarAreaCommandHandler : IRequestHandler<AdicionarAreaCommand, Result<Unit, Exception>>
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IProcessoRepository _processoRepository;
        private readonly IMapper _mapper;

        public AdicionarAreaCommandHandler(IAreaRepository areaRepository, IMapper mapper, IProcessoRepository processoRepository)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
            _processoRepository = processoRepository;
        }

        public async Task<Result<Unit, Exception>> Handle(AdicionarAreaCommand request, CancellationToken cancellationToken)
        {
            var areaExistente = await _areaRepository.AnyAsync(p => p.Nome.ToUpper().Equals(request.Nome.ToUpper()));
            if (areaExistente)
            {
                return new Exception($"Área '{request.Nome}' já existente.");
            }

            var area = _mapper.Map<Area>(request);

            if (request.Processos is not null)
            {
                var processos = await _processoRepository.FindAllByIds(request.Processos);
                if (processos.Any(x => x.AreaId is not null))
                {
                    return new Exception("Um ou mais processos especificados já estão atrelados a outras áreas.");
                }
                if (processos.Count() != request.Processos?.Count())
                {
                    return new Exception("Um ou mais processos especificados não existem.");
                }

                area.Processos = processos.ToList();
            }

            await _areaRepository.AddAsync(area);

            await _areaRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
