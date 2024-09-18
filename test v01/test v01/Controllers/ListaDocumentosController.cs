using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using test_v01.Models;
using test_v01.Repository;
using test_v01.Repository.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace test_v01.Controllers
{
    public class ListaDocumentosController : Controller
    {
        private readonly SITEtccDbContext _context = new SITEtccDbContext();


        public async Task<IActionResult> HomeList()
        {
            // Obtém o ID do usuário autenticado a partir das claims
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Converte o valor da claim para int
            int userId = int.Parse(userIdClaim.Value);

            // Busca os documentos associados ao usuário logado
            var documentos = await _context.Documentos
                .Where(d => d.Idusuario == userId) // Filtra pelos documentos do usuário logado
                .ToListAsync();

            return documentos != null ?
                View(documentos) :
                Problem("Nenhum documento encontrado para este usuário.");
        }


        public async Task<IActionResult> Create()
        {
            return View();
      
        }


        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            // Verifica se o usuário está autenticado e obtém o ID do usuário a partir das claims
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Converte o valor da claim para int
            int userId = int.Parse(userIdClaim.Value);
            
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
                    Idusuario = userId, // Usa o ID do usuário logado
                    FileData = fileData
                };

                _context.Documentos.Add(documento);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("HomeList");
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

        private bool DocumentoExists(int id)
        {
            return (_context.Documentos?.Any(e => e.Documentoid == id)).GetValueOrDefault();
        }




        public async Task<IActionResult> Edit(int? id)
        {
            // Verifica se o usuário está autenticado e obtém o ID do usuário
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            if (id == null || _context.Documentos == null)
            {
                return NotFound();
            }

            // Encontra o documento pelo ID e verifica se o documento pertence ao usuário
            var documento = await _context.Documentos
                .FirstOrDefaultAsync(d => d.Documentoid == id && d.Idusuario == userId);

            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }

        public async Task<IActionResult> Delete(int id)
        {
            // Verifica se o usuário está autenticado e obtém o ID do usuário
            var userIdClaim = User.FindFirst("Idusuario");
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Account");
            }

            int userId = int.Parse(userIdClaim.Value);

            // Encontra o documento pelo ID e verifica se o documento pertence ao usuário
            var documento = await _context.Documentos
                .FirstOrDefaultAsync(d => d.Documentoid == id && d.Idusuario == userId);

            if (documento == null)
            {
                return NotFound();
            }

            return View(documento);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Documentoid, Caminhodocumento, Documentonome, Idusuario, FileData")] Documento documento)
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
                return RedirectToAction(nameof(HomeList));
            }
            return View(documento);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var documento = await _context.Documentos.FindAsync(id);
            if (documento == null)
            {
                return NotFound();
            }

            _context.Documentos.Remove(documento);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(HomeList));
        }




    }
}
