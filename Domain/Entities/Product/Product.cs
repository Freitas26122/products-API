using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Entities.Class
{
    public class ProductEntity : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime CreatedAt { get; set; }

        public ProductEntity(
            string name,
            string description,
            decimal price,
            int stockQuantity,
            DateTime createdAt
        )
        {
            Id = RandomId.New();
            Name = name;
            Description = description;
            Price = price;
            StockQuantity = stockQuantity;
            CreatedAt = createdAt;
        }
    }
}