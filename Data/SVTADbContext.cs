using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VeiculosAPI.Models;

namespace VeiculosAPI.Data
{
    public class SVTADbContext : DbContext
    {
        public SVTADbContext(DbContextOptions<SVTADbContext> options) : base(options)
        {

        }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Imagem> Imagens { get; set; }
    }
}
