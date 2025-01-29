using System.Threading.Tasks;
using ProductsAPI.Domain.Commands;
using ProductsAPI.Domain.Listeners;
using ProductsAPI.Domain.Utils;

namespace ProductsAPI.Domain.Utils
{
    public interface ICommand
    {
        Task<CommandResult> GetErrorAsync(CommandsHandler handler);
        Task<bool> HasPermissionAsync(CommandsHandler handler);
        Task<CommandResult> ExecuteAsync(CommandsHandler handler);
        EventListenerType GetEvent();
    }
}