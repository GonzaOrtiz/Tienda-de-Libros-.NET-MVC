using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaboratorioIV.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;


namespace LaboratorioIV.Controllers
{
    public class LibrosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment env;

        public LibrosController(IWebHostEnvironment env)
        {
            //_context = context;
            _context = new AppDbContext();
            this.env = env;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.libros.Include(l => l.Autor).Include(l => l.Genero);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.libros
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["AutorId"] = new SelectList(_context.autores, "Id", "Nombres");
            ViewData["GeneroId"] = new SelectList(_context.generos, "Id", "Nombre");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Resumen,FechaDePublicacion,ImagenTapa,GeneroId,AutorId")] Libro libro)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "images\\libros");
                        // generar nombre aleatorio de fotografia
                        var archivoDestino = Guid.NewGuid().ToString();
                        archivoDestino = archivoDestino.Replace("-", "");
                        archivoDestino += Path.GetExtension(archivoFoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            libro.ImagenTapa = archivoDestino;
                        };
                    }
                }

                _context.Add(libro);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AutorId"] = new SelectList(_context.autores, "Id", "Id", libro.AutorId);
            ViewData["GeneroId"] = new SelectList(_context.generos, "Id", "Id", libro.GeneroId);
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            ViewData["AutorId"] = new SelectList(_context.autores, "Id", "Nombres", libro.AutorId);
            ViewData["GeneroId"] = new SelectList(_context.generos, "Id", "Nombre", libro.GeneroId);

            return View(libro);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Resumen,FechaDePublicacion,ImagenTapa,GeneroId,AutorId")] Libro libro)
        {
            if (id != libro.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var archivos = HttpContext.Request.Form.Files;
                    if (archivos != null && archivos.Count > 0)
                    {
                        var archivoFoto = archivos[0];
                        if (archivoFoto.Length > 0)
                        {
                            var pathDestino = Path.Combine(env.WebRootPath, "images\\libros");
                            // generar nombre aleatorio de fotografia
                            var archivoDestino = Guid.NewGuid().ToString();
                            archivoDestino = archivoDestino.Replace("-", "");
                            archivoDestino += Path.GetExtension(archivoFoto.FileName);
                            var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                            using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                            {
                                archivoFoto.CopyTo(filestream);
                                libro.ImagenTapa = archivoDestino;
                            };
                        }
                    }

                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["AutorId"] = new SelectList(_context.autores, "Id", "Nombres", libro.AutorId);
            ViewData["GeneroId"] = new SelectList(_context.generos, "Id", "Nombre", libro.GeneroId);
            return View(libro);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.libros
                .Include(l => l.Autor)
                .Include(l => l.Genero)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.libros.FindAsync(id);
            _context.libros.Remove(libro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return _context.libros.Any(e => e.Id == id);
        }
    }
}
