using AutoMapper;
using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Areas
{
    public class BuscarAreaPorIdCommandHandler : IRequestHandler<BuscarAreaPorIdCommand, Result<AreaDTO?, Exception>>
    {
        private readonly IAreaRepository _repository;
        private readonly IMapper _mapper;

        public BuscarAreaPorIdCommandHandler(IAreaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<AreaDTO?, Exception>> Handle(BuscarAreaPorIdCommand request, CancellationToken cancellationToken)
        {
            var area = await _repository.GetByIdAsync(request.Id);
            return _mapper.Map<AreaDTO>(area);
        }
    }
}
