using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioIV.Models
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder OptionBuilder)
        {
            OptionBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Libros-LaboratorioIV;Trusted_Connection=True;MultipleActiveResultSets=True");
        }
        public DbSet<Genero> generos { get; set; }
        public DbSet<Libro> libros { get; set; }
        public DbSet<Autor> autores { get; set; }
    }
}
