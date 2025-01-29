using EducaPlayAPI.Domain.Commands;
using EducaPlayAPI.Domain.Utils;

namespace EducaPlayAPI.Domain.Listeners
{
    public class EventListener(string id, EventListenerType type, Func<CommandsHandler, EventListenerType, ICommand, CommandResult, Task> method = null)
    {
        public string Id { get; protected set; } = id ?? RandomId.New();
        public EventListenerType Type { get; } = type;
        protected Func<CommandsHandler, EventListenerType, ICommand, CommandResult, Task> Method { get; set; } = method;

        public virtual async Task Run(CommandsHandler handler, EventListenerType type, ICommand command, CommandResult result)
        {
            if (Method != null)
                await Method(handler, type, command, result);
        }
    }
}