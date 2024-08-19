using GerenciadorProcessos.Application.Commands.Processos;
using GerenciadorProcessos.Domain.Entidades;
using GerenciadorProcessos.Domain.Repositorios;
using GerenciadorProcessos.Domain.Shared;
using MediatR;
using System.Linq.Expressions;

namespace GerenciadorProcessos.Application.CommandHandlers.Processos
{
    public class EditarProcessoCommandHandler : IRequestHandler<EditarProcessoCommand, Result<Unit, Exception>>
    {
        private readonly IProcessoRepository _repository;

        public EditarProcessoCommandHandler(IProcessoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<Unit, Exception>> Handle(EditarProcessoCommand request, CancellationToken cancellationToken)
        {
            var processo = await _repository.GetByIdAsync(request.Id);
            if (processo is null)
            {
                return new Exception($"Processo {request.Id} não existe.");
            }

            Expression<Func<Processo, bool>> nomeIgualEIdDiferente = a => a.Nome.ToUpper().Equals(request.Nome.ToUpper()) && a.Id != request.Id;
            var processoExistente = await _repository.AnyAsync(nomeIgualEIdDiferente);
            if (processoExistente)
            {
                return new Exception($"Processo '{request.Nome}' já existente.");
            }

            processo.Subprocessos.Clear();
            if (request.Subprocessos is not null)
            {
                var subprocessoIds = request.Subprocessos.Distinct();

                if (processo.ProcessoPai is not null)
                {
                    if (subprocessoIds.Contains(processo.ProcessoPai.Id))
                    {
                        return new Exception("Não é possível vincular um processo pai como subprocesso.");
                    }
                }

                var subprocessos = await _repository.FindAllByIds(subprocessoIds);
                if (subprocessos.Any(x => x.ProcessoPaiId != null && x.ProcessoPaiId != processo.Id))
                {
                    return new Exception("Um ou mais processos especificados já estão atrelados a outros processos.");
                }
                if (subprocessos.Count() != subprocessoIds.Count())
                {
                    return new Exception("Um ou mais subprocessos especificados não existem.");
                }

                processo.Subprocessos.AddRange(subprocessos);
            }

            AtualizarProcesso(request, processo);

            try
            {
                _repository.Update(processo);
                await _repository.SaveChangesAsync();
                return Unit.Value;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }

        private static void AtualizarProcesso(EditarProcessoCommand request, Processo processo)
        {
            processo.Nome = request.Nome;
            processo.Descricao = request.Descricao;
            processo.Ferramenta = request.Ferramenta;
            processo.Tipo = request.Tipo;
            processo.DataUltimaAlteracao = DateTime.UtcNow;
        }
    }
}
