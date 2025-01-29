using ProductsAPI.Domain.Entities.Class;
using ProductsAPI.Domain.Listeners;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Commands.Classes
{
    public class CreateClassCommand : ICommand
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }

        public async Task<CommandResult> GetErrorAsync(CommandsHandler handler)
        {
            if (string.IsNullOrWhiteSpace(Name))
                return await Task.FromResult(new CommandResult(ErrorCode.InvalidParameters, "É necessário dar um nome para o produto!"));
            if (string.IsNullOrWhiteSpace(Description))
                return await Task.FromResult(new CommandResult(ErrorCode.InvalidParameters, "É necessário adicionar uma descrição ao produto!"));
            if (Price <= 0)
                return await Task.FromResult(new CommandResult(ErrorCode.InvalidParameters, "O preço deve ser maior que zero!"));
            if (StockQuantity < 0)
                return await Task.FromResult(new CommandResult(ErrorCode.InvalidParameters, "A quantidade em estoque não pode ser negativa!"));

            return await Task.FromResult(new CommandResult(ErrorCode.None, "Produto válido"));
        }

        public async Task<bool> HasPermissionAsync(CommandsHandler handler)
        {
            return await Task.FromResult(true);
        }

        public async Task<CommandResult> ExecuteAsync(CommandsHandler handler)
        {
            var newProduct = new ProductEntity(
                 name: Name,
                 description: Description,
                 price: Price,
                 stockQuantity: StockQuantity
            );

            await handler.DbContext.Product.AddAsync(newProduct);

            var rows = await handler.DbContext.SaveChangesAsync();
            return await Task.FromResult(new CommandResult(rows));
        }

        public EventListenerType GetEvent()
        {
            return EventListenerType.None;
        }
    }
}