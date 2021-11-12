using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaboratorioIV.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace LaboratorioIV.Controllers
{
    public class AutorsController : Controller
    {
        private readonly AppDbContext _context;

        //public AutorsController(AppDbContext context)
        //{
        //    _context = context;
        //}
        private readonly IWebHostEnvironment env;
        public AutorsController(IWebHostEnvironment env)
        {
            _context = new AppDbContext();
            this.env = env;
        }

        // GET: Autors
        public async Task<IActionResult> Index()
        {
            return View(await _context.autores.ToListAsync());
        }

        // GET: Autors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.autores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // GET: Autors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Autors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Apellido,Nombres,Biografia,Foto")] Autor autor)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFoto = archivos[0];
                    if (archivoFoto.Length > 0)
                    {
                        var pathDestino = Path.Combine(env.WebRootPath, "images\\autores");
                        // generar nombre aleatorio de fotografia
                        var archivoDestino = Guid.NewGuid().ToString();
                        archivoDestino = archivoDestino.Replace("-", "");
                        archivoDestino += Path.GetExtension(archivoFoto.FileName);
                        var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                        using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                        {
                            archivoFoto.CopyTo(filestream);
                            autor.Foto = archivoDestino;
                        };
                    }
                }


                _context.Add(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(autor);
        }

        // GET: Autors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.autores.FindAsync(id);
            if (autor == null)
            {
                return NotFound();
            }
            return View(autor);
        }

        // POST: Autors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Apellido,Nombres,Biografia,Foto")] Autor autor)
        {
            if (id != autor.Id)
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
                            var pathDestino = Path.Combine(env.WebRootPath, "images\\autores");
                            // generar nombre aleatorio de fotografia
                            var archivoDestino = Guid.NewGuid().ToString();
                            archivoDestino = archivoDestino.Replace("-", "");
                            archivoDestino += Path.GetExtension(archivoFoto.FileName);
                            var rutaDestino = Path.Combine(pathDestino, archivoDestino);

                            using (var filestream = new FileStream(rutaDestino, FileMode.Create))
                            {
                                archivoFoto.CopyTo(filestream);
                                autor.Foto = archivoDestino;
                            };
                        }
                    }



                    _context.Update(autor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AutorExists(autor.Id))
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
            return View(autor);
        }

        // GET: Autors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var autor = await _context.autores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (autor == null)
            {
                return NotFound();
            }

            return View(autor);
        }

        // POST: Autors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var autor = await _context.autores.FindAsync(id);
            _context.autores.Remove(autor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AutorExists(int id)
        {
            return _context.autores.Any(e => e.Id == id);
        }
    }
}
