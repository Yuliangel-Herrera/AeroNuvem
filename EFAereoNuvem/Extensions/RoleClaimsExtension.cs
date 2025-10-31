using EFAereoNuvem.Models;
using System.Security.Claims;

namespace EFAereoNuvem.Extensions;
public static class RoleClaimsExtension
{
    // Transforma as Roles do usuário em Claims
    public static IEnumerable<Claim> GetClaims(this User user)
    {
        var result = new List<Claim>
        {
            new(ClaimTypes.Name, user.Email)
        };
        result.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.TypeRole)));

        return result;
    }
}
