using AutoMapper;
using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Areas
{
    public class ListarAreasCommandHandler : IRequestHandler<ListarAreasCommand, Result<IEnumerable<AreaDTO>, Exception>>
    {
        private readonly IAreaRepository _repository;
        private readonly IMapper _mapper;

        public ListarAreasCommandHandler(IAreaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<AreaDTO>, Exception>> Handle(ListarAreasCommand request, CancellationToken cancellationToken)
        {
            var areas = await _repository.GetAllAsync();
            return _mapper.Map<List<AreaDTO>>(areas);
        }
    }
}
