using LaboratorioIV.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaboratorioIV.ViewsModels
{
    public class LibrosViewModels
    {
        public List<Libro> Libros { get; set; }
        public SelectList Autores { get; set; }
        public SelectList Generos { get; set; }
        public string Titulo { get; set; }
        public string Resumen { get; set; }
        public DateTime FechaDePublicacion { get; set; }
        public string ImagenTapa { get; set; }
    }
}
