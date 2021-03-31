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
        const string mensagem400 = "Não há registros desse Id";
        const string mensagem200 = "Registro recuperado com sucesso";
        const string mensagem201 = "Registro atualizado com sucesso";

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
        public IActionResult Get(int id)
        {
            var veiculos = _sVTADbContext.Veiculos.Find(id);
            if (veiculos.Equals(null))
            {
                return NotFound(mensagem400);
            }
            return Ok(veiculos);
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
            return Ok(mensagem200);
        }

        // DELETE api/<VeiculosController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var veiculo = _sVTADbContext.Veiculos.Find(id);
            if (veiculo.Equals(null))
            {
                return NotFound("Não há registros desse Id");
            }
            _sVTADbContext.Veiculos.Remove(veiculo);
            _sVTADbContext.SaveChanges();
            return Ok(mensagem200);
        }
    }
}
