using Dapper;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Queries.Products.GetAllProductsQuery
{
    public class GetAllProductsQuery : AbstractQuery<ProductViewModel>
    {
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
                ";

            using (var connection = queriesHandler.QueriesDbContext.Database.GetDbConnection())
            {
                var _event = await connection.QueryAsync<ProductViewModel>(sql);
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
