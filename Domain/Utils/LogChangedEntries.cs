using System.Collections.Generic;

namespace ProductsAPI.Domain.Utils
{
    public class LogChangedEntry
    {
        public required string Id { get; set; }
        public required string EntityName { get; set; }
        public required string EntityState { get; set; }
        public List<LogChangedProperty> Properties { get; set; }

        public LogChangedEntry()
        {
            Properties = [];
        }
    }

    public class LogChangedProperty
    {
        public required string PropertyName { get; set; }
        public required dynamic OriginalValue { get; set; }
        public required dynamic CurrentValue { get; set; }
    }
}