using System;

namespace EducaPlayAPI.Domain.Utils
{
    public interface ITrackedEntity
    {
        DateTimeOffset? LastModified { get; set; }
        bool IsOfflineCommand { get; set; }
    }
}