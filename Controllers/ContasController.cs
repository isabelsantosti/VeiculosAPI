using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public ContasController(SVTADbContext dbContext)
        {
            _svtaDbContext = dbContext;
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
                Senha = usuario.Senha,
            };
            _svtaDbContext.Usuarios.Add(objetoUsuario);
            _svtaDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
