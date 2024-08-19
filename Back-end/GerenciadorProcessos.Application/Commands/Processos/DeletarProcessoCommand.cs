using FluentValidation;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Processos
{
    public class DeletarProcessoCommand : IRequest<Result<Unit, Exception>>
    {
        public Guid Id { get; set; }

        public DeletarProcessoCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeletarProcessoCommandValidator : AbstractValidator<DeletarProcessoCommand>
    {
        public DeletarProcessoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do processo é obrigatório.")
                .Must(id => id != Guid.Empty).WithMessage("O ID do processo não pode ser vazio.");
        }
    }
}
