using Dapper;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Queries.Products.GetProductByIdQuery
{
    public class GetProductByIdQuery : AbstractQuery<ProductViewModel>
    {
        public required string ProductId { get; set; }
        public override async Task<QueryResult<ProductViewModel>> ExecuteAsync(QueriesHandler queriesHandler)
        {
            var sql = @"
                SELECT 
                    [p].[id],
                    [p].[name],
                    [p].[description],
                    [p].[price],
                    [p].[stock_quantity],
                    [p].[created_at]
                FROM [dbo].[products] [p] WITH(NOLOCK)
                WHERE [p].[removed] = 0
                    AND [p].[id] = @ProductId
            ";

            using (var connection = queriesHandler.QueriesDbContext.Database.GetDbConnection())
            {
                var _event = await connection.QueryAsync<ProductViewModel>(sql, new { ProductId });
                return new QueryResult<ProductViewModel>(_event);
            }
        }

        public override async Task<bool> HasPermissionAsync(QueriesHandler queriesHandler)
        {
            return await Task.FromResult(true);
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
