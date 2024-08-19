using GerenciadorProcessos.API.Common;
using GerenciadorProcessos.Application.Commands.Areas;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GerenciadorProcessos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AreasController(IMediator mediator) : BaseController(mediator)
    {
        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(Guid id)
        {
            return await HandleCommand(new DeletarAreaCommand(id));
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromBody] AdicionarAreaCommand command)
        {
            return await HandleCommand(command);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Editar(Guid id, [FromBody] EditarAreaCommand command)
        {
            command.Id = id;
            return await HandleCommand(command);
        }

        [HttpGet]
        public async Task<IActionResult> Listar()
        {
            return await HandleCommand(new ListarAreasCommand());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> BuscarPorId(Guid id)
        {
            return await HandleCommand(new BuscarAreaPorIdCommand(id));
        }
    }
}
