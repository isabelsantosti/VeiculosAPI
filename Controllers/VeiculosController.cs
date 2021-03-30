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
        public IEnumerable<Veiculos> Get()
        {
            return _sVTADbContext.Veiculos;
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
        public void Post([FromBody] Veiculos veiculos)
        {
            //adiciona o veiculo
            _sVTADbContext.Veiculos.Add(veiculos);
            //salva dentro do banco 
            _sVTADbContext.SaveChanges();
        }

        // PUT api/<VeiculosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Veiculos veiculos)
        {
            var entidade =_sVTADbContext.Veiculos.Find(id);
            entidade.Nome = veiculos.Nome;
            entidade.Preco = veiculos.Preco;
            _sVTADbContext.SaveChanges();
        }

        // DELETE api/<VeiculosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var veiculo = _sVTADbContext.Veiculos.Find(id);
            _sVTADbContext.Veiculos.Remove(veiculo);
            _sVTADbContext.SaveChanges();
        }
    }
}
