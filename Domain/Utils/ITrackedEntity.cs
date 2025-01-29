using System;

namespace ProductsAPI.Domain.Utils
{
    public interface ITrackedEntity
    {
        DateTimeOffset? LastModified { get; set; }
        bool IsOfflineCommand { get; set; }
    }
}