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
        public void Registro([FromBody] Usuario usuario)
        {
            _svtaDbContext.Usuarios.Where(u => u.Email == usuario.Email).SingleOrDefault();
        }
    }
}
