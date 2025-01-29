using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace ProductsAPI.Domain.Queries
{
    public class QueriesDbContext(DbContextOptions<QueriesDbContext> options) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder) { }

        public async Task<(bool CanConnect, string ErrorMessage)> TryConnectionAsync()
        {
            try
            {
                var canConnect = await this.Database.CanConnectAsync();
                if (canConnect)
                    return (true, null);
                await this.Database.OpenConnectionAsync();
                this.Database.CloseConnection();
                return (false, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}