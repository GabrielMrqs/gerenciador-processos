using FluentValidation;
using GerenciadorProcessos.Domain.Enums;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Processos
{
    public class EditarProcessoCommand : IRequest<Result<Unit, Exception>>
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public TipoProcesso Tipo { get; set; }
        public string? Ferramenta { get; set; }
        public IEnumerable<Guid>? Subprocessos { get; set; }
    }

    public class EditarProcessoCommandValidator : AbstractValidator<EditarProcessoCommand>
    {
        public EditarProcessoCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do processo é obrigatório.")
                .Must(id => id != Guid.Empty).WithMessage("O ID do processo não pode ser vazio.");

            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do processo é obrigatório.");

            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("A descrição do processo é obrigatória.");

            RuleFor(x => x.Tipo)
                .IsInEnum().WithMessage("O tipo de processo deve ser válido.");

            RuleFor(x => x.Subprocessos)
                .Must(subprocessos => subprocessos == null || subprocessos.All(id => id != Guid.Empty))
                .WithMessage("Os IDs dos subprocessos não podem ser vazios.");
        }
    }
}
