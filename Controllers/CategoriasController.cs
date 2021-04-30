using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeiculosAPI.Data;
using VeiculosAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VeiculosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : ControllerBase
    {

        private SVTADbContext _sVTADbContext;

        public CategoriasController(SVTADbContext sVTADbContext)
        {
            _sVTADbContext = sVTADbContext;
        }
        // GET: api/<CategoriasController>
        [HttpGet]
        public IActionResult Get()
        {
            var categorias = _sVTADbContext.Categorias;
            return Ok(categorias);
        }
        [HttpPost]
        public IActionResult Post([FromBody] Categoria categoriaModel)
        {
            var categoria = new Categoria()
            {
                Tipo = categoriaModel.Tipo,
            };
            _sVTADbContext.Categorias.Add(categoria);
            _sVTADbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

    }
}
