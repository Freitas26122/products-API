using System;

namespace EducaPlayAPI.Domain.Listeners
{
    // Caso queira adicionar um listener novo, é só ir acrescentando no enum!
    [Flags]
    public enum EventListenerType
    {
        None = 0,
        All = 1
    }
}