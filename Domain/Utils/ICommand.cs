using System.Threading.Tasks;
using EducaPlayAPI.Domain.Commands;
using EducaPlayAPI.Domain.Listeners;
using EducaPlayAPI.Domain.Utils;

namespace EducaPlayAPI.Domain.Utils
{
    public interface ICommand
    {
        Task<CommandResult> GetErrorAsync(CommandsHandler handler);
        Task<bool> HasPermissionAsync(CommandsHandler handler);
        Task<CommandResult> ExecuteAsync(CommandsHandler handler);
        EventListenerType GetEvent();
    }
}