using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using test_v01.Repository;
using test_v01.Repository.Models;

namespace test_v01.Controllers
{
    public class DocumentosController : Controller
    {
        private readonly SITEtccDbContext _context = new SITEtccDbContext();


        // GET: Documentos


       
        public async Task<IActionResult> Index()
        {
            var sITEtccDbContext = _context.Documentos.Include(d => d.IdusuarioNavigation);
            return View(await sITEtccDbContext.ToListAsync());
          
        }

        // GET: Documentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            var documento = await _context.Documentos
                .Include(d => d.IdusuarioNavigation)
                .FirstOrDefaultAsync(m => m.Documentoid == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // GET: Documentos/Create
        public IActionResult Create()
        {
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario");
            return View();
        }

        // POST: Documentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Documentoid,Caminhodocumento,Documentonome,Idusuario,FileData")] Documento documento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(documento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario", documento.Idusuario);
            return View(documento);
        }

        // GET: Documentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario", documento.Idusuario);
            return View(documento);
        }

        // POST: Documentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Documentoid,Caminhodocumento,Documentonome,Idusuario,FileData")] Documento documento)
        {
            if (id != documento.Documentoid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(documento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentoExists(documento.Documentoid))
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
            ViewData["Idusuario"] = new SelectList(_context.Usuarios, "Idusuario", "Idusuario", documento.Idusuario);
            return View(documento);
        }

        // GET: Documentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            var documento = await _context.Documentos
                .Include(d => d.IdusuarioNavigation)
                .FirstOrDefaultAsync(m => m.Documentoid == id);
            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        // POST: Documentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documentos == null)
            {
                return Problem("Entity set 'SITEtccDbContext.Documentos'  is null.");
            }
            var documento = await _context.Documentos.FindAsync(id);
            if (documento != null)
            {
                _context.Documentos.Remove(documento);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentoExists(int id)
        {
          return (_context.Documentos?.Any(e => e.Documentoid == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file, int? idusuario)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Nenhum arquivo enviado.");
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileData = memoryStream.ToArray();

                var documento = new Documento
                {
                    Caminhodocumento = file.FileName,
                    Documentonome = file.FileName,
                    Idusuario = idusuario,
                    FileData = fileData
                };

                _context.Documentos.Add(documento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index"); // ou outra ação de sua escolha
        }

        public async Task<IActionResult> Download(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null || documento.FileData == null)
            {
                return NotFound();
            }

            return File(documento.FileData, "application/octet-stream", documento.Caminhodocumento);
        }
    }


}


