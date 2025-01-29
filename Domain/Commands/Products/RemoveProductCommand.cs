using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain.Listeners;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Commands.Products
{
    public class RemoveProductCommand : ICommand
    {
        public required string Id { get; set; }

        public async Task<CommandResult> GetErrorAsync(CommandsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Id))
                return await Task.FromResult(new CommandResult(ErrorCode.InvalidParameters, "É necessário informar o ID do produto!"));

            return await Task.FromResult(new CommandResult(ErrorCode.None, "Produto válido para remoção"));
        }

        public async Task<bool> HasPermissionAsync(CommandsHandler handler)
        {
            return await Task.FromResult(true);
        }

        public async Task<CommandResult> ExecuteAsync(CommandsHandler handler)
        {
            var existingProduct = await handler.DbContext.Product
                .FirstOrDefaultAsync(p => p.Id == Id);

            if (existingProduct == null)
                return await Task.FromResult(new CommandResult(ErrorCode.NotFound, "Produto não encontrado!"));

            existingProduct.Remove();

            var rows = await handler.DbContext.SaveChangesAsync();
            return await Task.FromResult(new CommandResult(rows));
        }

        public EventListenerType GetEvent()
        {
            return EventListenerType.None;
        }
    }
}