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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]

    public class VeiculosController : ControllerBase
    {
        private SVTADbContext _sVTADbContext;

        public VeiculosController(SVTADbContext sVTADbContext)
        {
            _sVTADbContext = sVTADbContext;
        }
        public IActionResult Post(Veiculo veiculo)
        {
            try
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
                    DataPostagem = DateTime.Now,
                    Motor = veiculo.Motor,
                    Preco = veiculo.Preco,
                    Modelo = veiculo.Modelo,
                    Localizacao = veiculo.Localizacao,
                    CategoriaId = veiculo.CategoriaId,
                    isNovo = false,
                    isDestaque = false,
                    UsuarioId = usuario.Id
                };
                _sVTADbContext.Veiculos.Add(veiculos);
                _sVTADbContext.SaveChanges();

                return Ok(new { veiculoId = veiculos.Id, message = "Veículo adicionado com sucesso!", status = true });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Todos os métodos abaixo eu utilizei o LINQ para fazer consultas dinâmicas no banco assim é possível,
        /// Filtrar e buscar veículos, anunciar veículos recomendados etc.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        //esse método vai retornar os veiculos recomendados os quais serão definidos pelo Adm no banco
        public IActionResult RecomendadoAds()
        {
            var veiculos = from v in _sVTADbContext.Veiculos
                           where v.isNovo == true
                           select new
                           {
                               Id = v.Id,
                               Nome = v.Nome,
                               Veiculo_ImageUrl = v.Imagens.FirstOrDefault().ImageUrl
                           };
            return Ok(veiculos);
        }
        //esse método vai retornar os veiculos recomendados os quais serão definidos pelo Adm no banco
        [HttpGet("[action]")]
        public IQueryable<object> BuscarVeiculos(string busca)
        {
            var veiculos = from v in _sVTADbContext.Veiculos
                           where v.Nome.StartsWith(busca) || v.Fabricante.StartsWith(busca)
                           select new
                           {
                               Id = v.Id,
                               Nome = v.Nome,
                               Modelo = v.Modelo,
                               Fabricante = v.Fabricante
                           };
            return veiculos.Take(15);
        }
        //GET: api/veiculos?categoriaId=1
        [HttpGet]
        public IQueryable<object> GetVeiculos(int CategoriaId)
        {
            var veiculos = from v in _sVTADbContext.Veiculos
                           where v.CategoriaId.Equals(CategoriaId)
                           select new
                           {
                               Id = v.Id,
                               Nome = v.Nome,
                               Preco = v.Preco,
                               Modelo = v.Modelo,
                               Localizacao = v.Localizacao,
                               Fabricante = v.Fabricante,
                               Data = v.DataPostagem,
                               isDestaque = v.isDestaque,
                               ImageUrl = v.Imagens.FirstOrDefault().ImageUrl

                           };

            return veiculos;
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> DetalhesVeiculos(int id)
        {
            var encontraVeiculo = await _sVTADbContext.Veiculos.FindAsync(id);
            if (encontraVeiculo == null)
                return NotFound("Não foi possível encontrar o veículo");

            var veiculo = (from a in _sVTADbContext.Veiculos
                           join u in _sVTADbContext.Usuarios on a.UsuarioId equals u.Id
                           where a.Id == id
                           select new
                           {
                               Id = a.Id,
                               Nome = a.Nome,
                               Descricao = a.Descricao,
                               Preco = a.Preco,
                               Modelo = a.Modelo,
                               Fabricante = a.Fabricante,
                               Motor = a.Motor,
                               Cor = a.Cor,
                               DataPostagem = a.DataPostagem,
                               Condicao = a.Condicao,
                               Localizacao = a.Localizacao,
                               Images = a.Imagens,
                               Email = u.Email,
                               Contato = u.Telefone,
                               ImageUrl = u.ImageUrl
                           }).FirstOrDefault();

            return Ok(veiculo);
        }

        [HttpGet("[action]")]
        public IActionResult MeusAds()
        {
            var usuarioEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var usuario = _sVTADbContext.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail);
            if (usuario == null)
                return NotFound("Usuario não encontrado");
            var veiculos = from v in _sVTADbContext.Veiculos
                           where v.UsuarioId.Equals(usuario.Id)
                           orderby v.DataPostagem descending
                           select new
                           {
                               Id = v.Id,
                               Nome = v.Nome,
                               Preco = v.Preco,
                               Data = v.DataPostagem,
                               Localizacao = v.Localizacao,
                               isDestaque = v.isDestaque,
                               ImageUrl = v.Imagens.FirstOrDefault().ImageUrl
                           };

            return Ok(veiculos);
        }
        //GET api/Veiculos/FiltrarVeiculos?categoriaId=1&condicao=Novo&sort=asc&preco=20
        [HttpGet("[action]")]
        public IQueryable<object> FiltrarVeiculos(int categoriaId, string condicao, string sort, double preco)
        {
            IQueryable<object> veiculos;
            switch (sort)
            {
                case "desc":
                    veiculos = from v in _sVTADbContext.Veiculos
                               join u in _sVTADbContext.Usuarios on v.UsuarioId equals u.Id
                               join c in _sVTADbContext.Categorias on v.CategoriaId equals c.Id
                               where v.Preco >= preco && c.Id == categoriaId && v.Condicao == condicao
                               orderby v.Preco descending
                               select new
                               {
                                   Id = v.Id,
                                   Nome = v.Nome,
                                   Preco = v.Preco,
                                   Modelo = v.Modelo,
                                   Localizacao = v.Localizacao,
                                   Fabricante = v.Fabricante,
                                   DataPostagem = v.DataPostagem,
                                   isDestaque = v.isDestaque,
                                   ImageUrl = v.Imagens.FirstOrDefault().ImageUrl
                               };
                    break;

                case "asc":
                    veiculos = from v in _sVTADbContext.Veiculos
                               join u in _sVTADbContext.Usuarios on v.UsuarioId equals u.Id
                               join c in _sVTADbContext.Categorias on v.CategoriaId equals c.Id
                               where v.Preco >= preco && c.Id == categoriaId && v.Condicao == condicao
                               orderby v.Preco descending
                               select new
                               {
                                   Id = v.Id,
                                   Nome = v.Nome,
                                   Preco = v.Preco,
                                   Modelo = v.Modelo,
                                   Localizacao = v.Localizacao,
                                   Fabricante = v.Fabricante,
                                   DataPostagem = v.DataPostagem,
                                   isDestaque = v.isDestaque,
                                   ImageUrl = v.Imagens.FirstOrDefault().ImageUrl
                               };
                    break;

                default:
                    veiculos = from v in _sVTADbContext.Veiculos
                               where v.CategoriaId == categoriaId
                               select new
                               {
                                   Id = v.Id,
                                   Nome = v.Nome,
                                   Preco = v.Preco,
                                   Modelo = v.Modelo,
                                   Localizacao = v.Localizacao,
                                   Fabricante = v.Fabricante,
                                   DataPostagem = v.DataPostagem,
                                   isDestaque = v.isDestaque,
                                   ImageUrl = v.Imagens.FirstOrDefault().ImageUrl
                               };
                    break;
            }

            return veiculos;
        }
    }
}
