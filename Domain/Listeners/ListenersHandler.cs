using System;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsAPI.Domain.Commands;
using ProductsAPI.Domain.Utils;
using Microsoft.IdentityModel.Tokens;

namespace ProductsAPI.Domain.Listeners
{
    public class ListenersHandler
    {
        private static ListenersHandler _instance;
        private static readonly object LockObject = new();
        private readonly ConcurrentDictionary<string, EventListener> _listeners;
        private readonly string _secretKey;

        public ListenersHandler(string secretKey)
        {
            _listeners = new ConcurrentDictionary<string, EventListener>();
            _secretKey = secretKey;
        }

        public static ListenersHandler GetInstance(string secretKey)
        {
            lock (LockObject)
            {
                _instance ??= new ListenersHandler(secretKey);
            }
            return _instance;
        }

        private EventListener[] GetListeners(EventListenerType type)
        {
            return _listeners.Values
                .Where(l => l.Type.HasFlag(type))
                .ToArray();
        }

        public void Subscribe(EventListener listener)
        {
            if (!_listeners.TryAdd(listener.Id, listener))
                throw new Exception("Duplicate event listener ID");
        }

        public bool Unsubscribe(string id)
        {
            return _listeners.TryRemove(id, out _);
        }

        public async Task HandleEvent(CommandsHandler handler, ICommand command, CommandResult result)
        {
            var ev = command.GetEvent();
            if (ev == EventListenerType.None)
                return;

            var lstListeners = GetListeners(ev);
            foreach (var listener in lstListeners)
                await listener.Run(handler, ev, command, result);
        }

        public async Task<bool> ValidateTokenAsync(string token) => (
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(token))
                    return false;

                var now = DateTime.UtcNow;
                var tokenHandler = new JwtSecurityTokenHandler();

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                try
                {
                    tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                    return validatedToken != null;
                }
                catch
                {
                    return false;
                }
            })
        );
    }
}