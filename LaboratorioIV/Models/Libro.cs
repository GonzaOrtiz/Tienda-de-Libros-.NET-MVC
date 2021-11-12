using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioIV.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Resumen { get; set; }
        public  DateTime FechaDePublicacion { get; set; }
        public string ImagenTapa { get; set; }
        public int GeneroId { get; set; }
        public int AutorId { get; set; }
        public Genero Genero { get; set; }
        public Autor Autor { get; set; }
    }
}
