using FluentValidation;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Processos
{
    public class BuscarProcessoPorIdCommand : IRequest<Result<ProcessoDTO?, Exception>>
    {
        public BuscarProcessoPorIdCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }

    public class BuscarProcessoPorIdCommandValidator : AbstractValidator<BuscarProcessoPorIdCommand>
    {
        public BuscarProcessoPorIdCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID do processo é obrigatório.")
                .Must(id => id != Guid.Empty).WithMessage("O ID do processo não pode ser vazio.");
        }
    }
}
