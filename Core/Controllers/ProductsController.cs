using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Domain.Commands;
using ProductsAPI.Domain.Commands.Products;
using ProductsAPI.Domain.Queries;

namespace ProductsAPI.Core.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ClassesController : ControllerBaseHandler
    {
        private readonly CommandsHandler _commandsHandler;
        private readonly QueriesHandler _queriesHandler;

        public ClassesController(CommandsHandler commandsHandler, QueriesHandler queriesHandler)
        {
            _commandsHandler = commandsHandler;
            _queriesHandler = queriesHandler;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProductCommand([FromBody] CreateProductCommand cmd)
        {
            var result = await _commandsHandler.Handle(cmd);
            return GetResult(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProductCommand([FromBody] UpdateProductCommand cmd)
        {
            var result = await _commandsHandler.Handle(cmd);
            return GetResult(result);
        }

        [HttpPost("remove")]
        public async Task<IActionResult> RemoveProductCommand([FromBody] RemoveProductCommand cmd)
        {
            var result = await _commandsHandler.Handle(cmd);
            return GetResult(result);
        }
    }
}
