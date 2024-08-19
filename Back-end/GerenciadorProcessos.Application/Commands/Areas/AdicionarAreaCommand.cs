using FluentValidation;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Areas
{
    public class AdicionarAreaCommand : IRequest<Result<Unit, Exception>>
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public IEnumerable<Guid>? Processos { get; set; }
    }

    public class AdicionarAreaCommandValidator : AbstractValidator<AdicionarAreaCommand>
    {
        public AdicionarAreaCommandValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome da área é obrigatório.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição da área é obrigatória.");

            RuleFor(x => x.Processos)
                .Must(x => x == null || x.All(id => id != Guid.Empty))
                .WithMessage("Os IDs dos processos não podem ser vazios.");
        }
    }
}
