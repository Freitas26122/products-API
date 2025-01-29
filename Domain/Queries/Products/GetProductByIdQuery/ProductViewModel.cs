

using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Queries.Products.GetProductByIdQuery
{
    public class ProductViewModel : IViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}