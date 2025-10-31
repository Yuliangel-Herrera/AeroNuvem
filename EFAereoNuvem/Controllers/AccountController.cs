using EFAereoNuvem.Data;
using EFAereoNuvem.Models;
using EFAereoNuvem.Services;
using EFAereoNuvem.ViewModel;
using EFAereoNuvem.ViewModel.ResponseViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace EFAereoNuvem.Controllers;

public class AccountController(TokenServices tokenServices) : Controller
{
    private readonly TokenServices _tokenServices = tokenServices;

    [HttpPost("v1/accounts/register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model, [FromServices] AppDBContext context)
    {
        if (!ModelState.IsValid)
        {
            var response = new ResponseViewModel<string>(ConstantsMessage.MODEL_INVALIDO);
            return BadRequest(response);
        }

        // Cria o hasher de senhas 
        var passwordHasher = new PasswordHasher<User>();

        // Verifica se já existe um usuário com o mesmo e-mail
        if (await context.Users.AnyAsync(u => u.Email == model.Email))
        {
            var response = new ResponseViewModel<string>(ConstantsMessage.EMAIL_JA_CADASTRADO);
            return BadRequest(response);
        }

        // Busca a Role padrão "Client"
        var clientRole = await context.Roles.FirstOrDefaultAsync(r => r.TypeRole == "Client");
        if (clientRole == null)
        {
            var response = new ResponseViewModel<string>(ConstantsMessage.ROLE_NAO_ENCONTRADO);
            return BadRequest(response);
        }

        // Cria o usuário com senha criptografada
        var user = new User
        {
            Email = model.Email,
            PasswordHash = passwordHasher.HashPassword(null, model.Password),
            Roles = new List<Role> { clientRole }
        };

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var response = new ResponseViewModel<string>(ConstantsMessage.CONTA_CRIADA_COM_SUCESSO);
            return Ok(response);
        }
        catch
        {
            var response = new ResponseViewModel<string>(ConstantsMessage.ERRO_AO_CRIAR_CONTA);
            return BadRequest(response);
        }
    }


    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model, [FromServices] AppDBContext context, [FromServices] TokenServices tokenServices)
    {
        if (!ModelState.IsValid)
        {
            var response = new ResponseViewModel<string>(ConstantsMessage.MODEL_INVALIDO);
            return BadRequest(response);
        }

        var user = await context
            .Users
            .AsNoTracking()
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
        {
            var response = new ResponseViewModel<string>(ConstantsMessage.NAO_LOCALIZADO);
            return NotFound(response); 
        }
            

        // Cria uma instância do PasswordHasher
        var passwordHasher = new PasswordHasher<User>();

        // Verifica se a senha digitada corresponde ao hash armazenado
        var verificationResult = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);

        if(verificationResult != PasswordVerificationResult.Success)
        {
            var response = new ResponseViewModel<string>(ConstantsMessage.NAO_LOCALIZADO);
            return Unauthorized(response);
        }


        try
        {
            var token = tokenServices.GenerateToken(user);

            var response = new ResponseViewModel<object>(
             new { token }, ConstantsMessage.TOKEN_GERADO_COM_SUCESSO );

            return Ok(response);
        }
        catch
        {
            return BadRequest(new ResponseViewModel<string>(ConstantsMessage.ERRO_AO_GERAR_TOKEN)); 
        }
    }
}
