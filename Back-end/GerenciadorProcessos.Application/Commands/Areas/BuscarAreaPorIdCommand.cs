using FluentValidation;
using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Areas
{
    public class BuscarAreaPorIdCommand : IRequest<Result<AreaDTO?, Exception>>
    {
        public Guid Id { get; set; }

        public BuscarAreaPorIdCommand(Guid id)
        {
            Id = id;
        }
    }

    public class BuscarAreaPorIdCommandValidator : AbstractValidator<BuscarAreaPorIdCommand>
    {
        public BuscarAreaPorIdCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O ID da área é obrigatório.")
                .Must(id => id != Guid.Empty).WithMessage("O ID da área não pode ser vazio.");
        }
    }
}
