﻿using Microsoft.AspNetCore.Authorization;
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
                isDestaque = false,
                isNovo = false,
                UsuarioId = usuario.Id
            };
            _sVTADbContext.Veiculos.Add(veiculos);
            _sVTADbContext.SaveChanges();

            return Ok(new { veiculoId = veiculos.Id, message = "Veículo adicionado com sucesso!", status=true });
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
        public IActionResult DetalhesVeiculos(int id)
        {
            var encontraVeiculo = _sVTADbContext.Veiculos.FindAsync(id);
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
    }
}
