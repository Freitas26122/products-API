using Microsoft.AspNetCore.Mvc;
using ProductsAPI.Domain.Commands;
using ProductsAPI.Domain.Commands.Products;
using ProductsAPI.Domain.Queries;
using ProductsAPI.Domain.Queries.Products.GetAllProductsQuery;
using ProductsAPI.Domain.Queries.Products.GetProductByIdQuery;
using ProductsAPI.Domain.Queries.Products.GetProductsByNameQuery;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Core.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBaseHandler
    {
        private readonly CommandsHandler _commandsHandler;
        private readonly QueriesHandler _queriesHandler;

        public ProductsController(CommandsHandler commandsHandler, QueriesHandler queriesHandler)
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

        [HttpGet("management/product")]
        public async Task<ActionResult<CommandResult>> GetAllProductsQuery([FromQuery] GetAllProductsQuery query)
        {
            return GetResult(await _queriesHandler.RunQuery(query));
        }

        [HttpGet("management/product/{ProductId}")]
        public async Task<ActionResult<CommandResult>> GetProductByIdQuery([FromRoute] GetProductByIdQuery query)
        {
            return GetResult(await _queriesHandler.RunQuery(query));
        }

        [HttpGet("management/product/name/{ProductName}")]
        public async Task<ActionResult<CommandResult>> GetProductByNameQuery([FromRoute] GetProductByNameQuery query)
        {
            return GetResult(await _queriesHandler.RunQuery(query));
        }
    }
}
