using Dapper;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Queries.Products.GetProductsByNameQuery
{
    public class GetOrganizerByIdQuery : AbstractQuery<ProductViewModel>
    {
        public required string ProductName { get; set; }
        public override async Task<QueryResult<ProductViewModel>> ExecuteAsync(QueriesHandler queriesHandler)
        {

            var sql = @"
                SELECT
                    [id],
                    [name],
                    [description],
                    [faq],
                    [notes],
                    [courses],
                    [quotas]
                FROM [dbo].[organizer] [o] WITH(NOLOCK)
                WHERE [id] = @OrganizerId
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
