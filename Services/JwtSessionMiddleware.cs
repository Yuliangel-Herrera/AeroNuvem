using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EFAereoNuvem.Services;

public class JwtSessionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _jwtKey;

    public JwtSessionMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _jwtKey = configuration["JwtKey"];
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Session.GetString("AuthToken");

        if (!string.IsNullOrEmpty(token))
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims;
                var identity = new ClaimsIdentity(claims, "jwt");
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;
            }
            catch
            {
                // Token inválido ou expirado — ignora e segue sem usuário
            }
        }

        await _next(context);
    }
}

