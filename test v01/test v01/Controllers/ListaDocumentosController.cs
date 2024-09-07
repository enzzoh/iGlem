using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using test_v01.Models;
using test_v01.Repository;
using test_v01.Repository.Models;

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
    }
}
