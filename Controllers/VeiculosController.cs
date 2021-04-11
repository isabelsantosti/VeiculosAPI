using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VeiculosAPI.Data;
using VeiculosAPI.Models;

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
        [Authorize]
        public IActionResult Post(Veiculo veiculo)
        {
            var usuarioEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var usuario = _sVTADbContext.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail);
            if (usuario == null)
                return NotFound("Usuario não encontrado");
            var veiculos = new Veiculo()
            {
                Nome = veiculo.Nome,
                Descricao = veiculo.Descricao,
                Cor = veiculo.Cor,
                Fabricante = veiculo.Fabricante,
                Condicao = veiculo.Condicao,
                DataPostagem = veiculo.DataPostagem,
                Motor = veiculo.Motor,
                Preco = veiculo.Preco,
                Modelo = veiculo.Modelo,
                Localizacao = veiculo.Localizacao,
                CategoriaId = veiculo.CategoriaId,
                isDestaque = false,
                isNovo = false,
                UsuarioId = usuario.Id
            };
            _sVTADbContext.Veiculos.Add(veiculos);
            _sVTADbContext.SaveChanges();

            return Ok(new {veiculoId = veiculos.Id, message = "Veículo adicionado com sucesso!" });
        }
    }
}
