using AutoMapper;
using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Processos
{
    public class ListarProcessosCommandHandler : IRequestHandler<ListarProcessosCommand, Result<IEnumerable<ProcessoDTO>, Exception>>
    {
        private readonly IProcessoRepository _repository;
        private readonly IMapper _mapper;

        public ListarProcessosCommandHandler(IProcessoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<ProcessoDTO>, Exception>> Handle(ListarProcessosCommand request, CancellationToken cancellationToken)
        {
            var processos = await _repository.GetAllAsync();
            var processosDTO = _mapper.Map<List<ProcessoDTO>>(processos);
            return processosDTO;
        }
    }
}
