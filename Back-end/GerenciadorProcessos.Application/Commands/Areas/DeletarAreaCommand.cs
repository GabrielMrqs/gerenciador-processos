using FluentValidation;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Areas
{
    public class DeletarAreaCommand : IRequest<Result<Unit, Exception>>
    {
        public Guid Id { get; set; }

        public DeletarAreaCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeletarAreaCommandValidator : AbstractValidator<DeletarAreaCommand>
    {
        public DeletarAreaCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID da área é obrigatório.")
                .Must(id => id != Guid.Empty).WithMessage("O ID da área não pode ser vazio.");
        }
    }
}
