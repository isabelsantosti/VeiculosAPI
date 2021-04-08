using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VeiculosAPI.Data;
using VeiculosAPI.Models;

namespace VeiculosAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContasController : ControllerBase
    {
        private SVTADbContext _svtaDbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public ContasController(IConfiguration configuration, SVTADbContext dbContext)
        {
            _svtaDbContext = dbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }
        [HttpPost]
        //rota api/contas/registro
        //Método de registro de usuários
        public IActionResult Registro([FromBody] Usuario usuario)
        {
            var userSameEmail = _svtaDbContext.Usuarios.Where(u => u.Email == usuario.Email).SingleOrDefault();
            if (userSameEmail != null)
                return BadRequest("Um usuário com o mesmo e-mail já existe");
            var objetoUsuario = new Usuario()
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = SecurePasswordHasherHelper.Hash(usuario.Senha),
                DataInclusao = DateTime.Now
            };
            _svtaDbContext.Usuarios.Add(objetoUsuario);
            _svtaDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost]
        public IActionResult Login([FromBody] Usuario usuario)
        {
            //usando o LINQ para fazer a consulta no email do usuário
            var userEmail = _svtaDbContext.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);
            if (userEmail == null)
            {
                return NotFound();
            }
            //verifica se o hash na senha do usuario é falso 
            if (!SecurePasswordHasherHelper.Verify(usuario.Senha, userEmail.Senha))
            {
                return Unauthorized();
            }
            var claims = new[]
            {
               new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
               new Claim(ClaimTypes.Email, usuario.Email),
            };
            var token = _auth.GenerateAccessToken(claims);
            //conteúdo que será retorno ao fazermos nossa requisição da API, essa requisições serão utilizadas no projeto do app para exibir nome do usuário
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userEmail.Id
            });
        }

        public IActionResult TrocarSenha([FromBody] )
    }
}
