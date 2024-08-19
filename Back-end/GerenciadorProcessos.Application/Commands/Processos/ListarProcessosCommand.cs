using GerenciadorProcessos.Domain.DTOs;
using GerenciadorProcessos.Domain.Shared;
using MediatR;

namespace GerenciadorProcessos.Application.Commands.Processos
{
    public class ListarProcessosCommand : IRequest<Result<IEnumerable<ProcessoDTO>, Exception>>
    {
    }
}
