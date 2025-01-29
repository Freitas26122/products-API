using Dapper;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Queries.Products.GetProductsByNameQuery
{
    public class GetProductByNameQuery : AbstractQuery<ProductViewModel>
    {
        public required string ProductName { get; set; }
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
                    AND [p].[name] = @ProductName
                ";

            using (var connection = queriesHandler.QueriesDbContext.Database.GetDbConnection())
            {
                var _event = await connection.QueryAsync<ProductViewModel>(sql, new { ProductName });
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
