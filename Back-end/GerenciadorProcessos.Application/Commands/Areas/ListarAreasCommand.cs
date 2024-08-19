using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Areas
{
    public class ListarAreasCommand : IRequest<Result<IEnumerable<AreaDTO>, Exception>>
    {
    }
}
