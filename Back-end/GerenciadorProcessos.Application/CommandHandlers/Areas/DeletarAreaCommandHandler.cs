using GerenciadorProcessos.Application.Commands.Areas;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Areas
{
    public class DeletarAreaCommandHandler : IRequestHandler<DeletarAreaCommand, Result<Unit, Exception>>
    {
        private readonly IAreaRepository _repository;

        public DeletarAreaCommandHandler(IAreaRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit, Exception>> Handle(DeletarAreaCommand request, CancellationToken cancellationToken)
        {
            var areaExistente = await _repository.GetByIdAsync(request.Id);
            if (areaExistente is null)
            {
                return new Exception($"A área não existe.");
            }

            try
            {
                _repository.Remove(areaExistente);
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
