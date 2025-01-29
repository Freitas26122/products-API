using System;

namespace EducaPlayAPI.Domain.Utils
{
    public class RequestData
    {
        public string UserId { get; private set; }
        public string AccessToken { get; private set; }
        public string RemoteIpAddress { get; private set; }
        public string Url { get; private set; }
        public string Method { get; private set; }

        public RequestData() { }

        public RequestData(string userId, string accessToken, string remoteIpAddress, string method, string url)
        {
            UserId = userId;
            RemoteIpAddress = remoteIpAddress;
            Method = method;
            Url = url;
            AccessToken = accessToken != null && accessToken.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ?
                accessToken.Remove(0, 7) :
                accessToken;
        }

        public RequestData(string remoteIpAddress, string method, string url)
        {
            RemoteIpAddress = remoteIpAddress;
            Method = method;
            Url = url;
        }
    }
}