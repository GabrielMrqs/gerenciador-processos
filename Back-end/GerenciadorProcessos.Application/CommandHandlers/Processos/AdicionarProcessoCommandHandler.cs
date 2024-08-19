using AutoMapper;
using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Processos
{
    public class AdicionarProcessoCommandHandler : IRequestHandler<AdicionarProcessoCommand, Result<Unit, Exception>>
    {
        private readonly IProcessoRepository _repository;
        private readonly IMapper _mapper;

        public AdicionarProcessoCommandHandler(IProcessoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<Unit, Exception>> Handle(AdicionarProcessoCommand request, CancellationToken cancellationToken)
        {
            var processoExistente = await _repository.AnyAsync(p => p.Nome.Equals(request.Nome));
            if (processoExistente)
            {
                return new Exception($"Processo '{request.Nome}' já existente.");
            }

            var processo = _mapper.Map<Processo>(request);

            if (request.Subprocessos is not null)
            {
                var subprocessoIds = request.Subprocessos.Distinct();
                var subprocessos = await _repository.FindAllByIds(subprocessoIds);
                if (subprocessos.Any(x => x.ProcessoPaiId is not null))
                {
                    return new Exception("Um ou mais processos especificados já estão atrelados a outros processos.");
                }
                if (subprocessos.Count() != subprocessoIds.Count())
                {
                    return new Exception("Um ou mais subprocessos especificados não existem.");
                }

                processo.Subprocessos.AddRange(subprocessos);
            }

            try
            {
                await _repository.AddAsync(processo);
                await _repository.SaveChangesAsync();
                return Unit.Value;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}