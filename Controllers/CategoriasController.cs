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

        // GET api/<CategoriasController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CategoriasController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CategoriasController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoriasController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
