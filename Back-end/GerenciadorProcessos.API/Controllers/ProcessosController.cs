using GerenciadorProcessos.API.Common;
using GerenciadorProcessos.Application.Commands.Processos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorProcessos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessosController(IMediator mediator) : BaseController(mediator)
    {
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            return await HandleCommand(new DeletarProcessoCommand(id));
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarProcessoCommand command)
        {
            return await HandleCommand(command);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(Guid id, [FromBody] EditarProcessoCommand command)
        {
            command.Id = id;
            return await HandleCommand(command);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return await HandleCommand(new ListarProcessosCommand());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            return await HandleCommand(new BuscarProcessoPorIdCommand(id));
        }
    }
}
