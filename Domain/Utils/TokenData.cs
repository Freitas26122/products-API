using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ProductsAPI.Domain.Utils
{
    public class TokenData
    {
        private readonly JwtSecurityToken _token;

        public string Token { get; }
        public string UserId { get; set; }

        public TokenData(string token)
        {
            if (string.IsNullOrWhiteSpace(token) || token.Length < 15)
                throw new ArgumentException("Invalid token", nameof(token));

            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token[7..];

            if (string.IsNullOrEmpty(token) || token.Length < 10)
                throw new ArgumentException("Invalid token", nameof(token));

            Token = token;
            _token = new JwtSecurityToken(Token);

            if (_token == null)
                throw new InvalidOperationException("Failed to parse token");

            UserId = _token.Payload.ContainsKey("sub") ? _token.Payload["sub"].ToString() : null;
        }

        public static TokenData FromHttpRequest(HttpRequest request)
        {
            if (request.Headers.TryGetValue("Authorization", out var tokenHeader))
            {
                return new TokenData(tokenHeader.ToString());
            }

            return null;
        }

        public async Task<bool> ValidateAsync() => (
            await Task.Run(() =>
                {
                    if (_token == null)
                        throw new InvalidOperationException("Token is not initialized");

                    var now = DateTime.UtcNow;
                    if (now < _token.ValidFrom || now > _token.ValidTo)
                        return false;

                    var symmetricKey = Encoding.UTF8.GetBytes(Keys.ACCESS_TOKEN_SIGNATURE);
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(symmetricKey),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };

                    var tokenHandler = new JwtSecurityTokenHandler();
                    try
                    {
                        tokenHandler.ValidateToken(Token, tokenValidationParameters, out var validatedToken);
                        return validatedToken != null;
                    }
                    catch
                    {
                        return false;
                    }
                })
            );

        public static string GetValue(string token, string key)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var jwtToken = new JwtSecurityToken(token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase) ? token[7..] : token);
            return jwtToken.Payload.TryGetValue(key, out var value) ? value.ToString() : null;
        }
    }
}