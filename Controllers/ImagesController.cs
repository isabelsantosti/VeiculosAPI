using ImageUploader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using VeiculosAPI.Data;
using VeiculosAPI.Models;

namespace VeiculosAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private SVTADbContext _svtaDbContext;

        public ImagesController(SVTADbContext sVTADbContext)
        {
            _svtaDbContext = sVTADbContext;
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody]Imagem Imagens)
        {
            var usuarioEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value;
            var usuario = _svtaDbContext.Usuarios.FirstOrDefault(u => u.Email == usuarioEmail);
            if (usuario == null)
                return NotFound("Usuario não encontrado");
            var stream = new MemoryStream(Imagens.ImageArray);
            //globally unique identifier = GUID serve para dar um id unico para o arquivo, evitando sua duplicação
            var guid = Guid.NewGuid().ToString();
            var arquivo = $"{guid}.jpg";
            var pasta = "wwwroot/Images";
            var fullPath = $"{arquivo}/{pasta}";
            var imageFullPath = fullPath.Remove(0, 7);
            var resposta = FilesHelper.UploadImage(stream, pasta, arquivo);
            if (!resposta)
                return BadRequest();
            else
            {
                Imagens.ImageUrl = imageFullPath;
                var imageObj = new Imagem()
                {
                    ImageUrl = Imagens.ImageUrl,
                    VeiculoId = Imagens.VeiculoId
                };
                _svtaDbContext.Imagens.Add(imageObj);
                _svtaDbContext.SaveChanges();
                return StatusCode(StatusCodes.Status201Created);
            }
        }

    }
}
