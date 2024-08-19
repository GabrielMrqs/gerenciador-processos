using AutoMapper;
using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Processos
{
    public class BuscarProcessoPorIdCommandHandler : IRequestHandler<BuscarProcessoPorIdCommand, Result<ProcessoDTO?, Exception>>
    {
        private readonly IProcessoRepository _repository;
        private readonly IMapper _mapper;

        public BuscarProcessoPorIdCommandHandler(IProcessoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<ProcessoDTO?, Exception>> Handle(BuscarProcessoPorIdCommand request, CancellationToken cancellationToken)
        {
            var processo = await _repository.GetByIdAsync(request.Id);
            var processoDTO = _mapper.Map<ProcessoDTO>(processo);
            return processoDTO;
        }
    }
}
