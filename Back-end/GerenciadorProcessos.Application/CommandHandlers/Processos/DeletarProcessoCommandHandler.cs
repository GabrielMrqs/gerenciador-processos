using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.CommandHandlers.Processos
{
    public class DeletarProcessoCommandHandler : IRequestHandler<DeletarProcessoCommand, Result<Unit, Exception>>
    {
        private readonly IProcessoRepository _repository;

        public DeletarProcessoCommandHandler(IProcessoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit, Exception>> Handle(DeletarProcessoCommand request, CancellationToken cancellationToken)
        {
            var processoExistente = await _repository.GetByIdAsync(request.Id);
            if (processoExistente is null)
            {
                return new Exception($"O processo não existe.");
            }

            try
            {
                _repository.Remove(processoExistente);
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
