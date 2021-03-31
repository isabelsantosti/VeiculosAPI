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
    public class VeiculosController : ControllerBase
    {
        private SVTADbContext _sVTADbContext;

        public VeiculosController(SVTADbContext sVTADbContext)
        {
            _sVTADbContext = sVTADbContext;
        }


        // GET: api/<VeiculosController>
        //metodo para retornar uma lista de veiculos
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_sVTADbContext.Veiculos);
        }

        // GET api/<VeiculosController>/5
        // retorna um veiculo por ID
        [HttpGet("{id}")]
        public Veiculos Get(int id)
        {
            var veiculos = _sVTADbContext.Veiculos.Find(id);
            return veiculos;
        }

        // POST api/<VeiculosController>
        [HttpPost]
        public IActionResult Post([FromBody] Veiculos veiculos)
        {
            //adiciona o veiculo
            _sVTADbContext.Veiculos.Add(veiculos);
            //salva dentro do banco 
            _sVTADbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        // PUT api/<VeiculosController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Veiculos veiculos)
        {
            var entidade =_sVTADbContext.Veiculos.Find(id);
            if (entidade.Equals(null))
                return NotFound("Não há registros desse Id");
            else
            {
                entidade.Nome = veiculos.Nome;
                entidade.Preco = veiculos.Preco;
            }
            _sVTADbContext.SaveChanges();
            return Ok("Registro atualizado com sucesso");
        }

        // DELETE api/<VeiculosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var veiculo = _sVTADbContext.Veiculos.Find(id);
            _sVTADbContext.Veiculos.Remove(veiculo);
            _sVTADbContext.SaveChanges();
            return Ok("Registro apagado com sucesso");
        }
    }
}
