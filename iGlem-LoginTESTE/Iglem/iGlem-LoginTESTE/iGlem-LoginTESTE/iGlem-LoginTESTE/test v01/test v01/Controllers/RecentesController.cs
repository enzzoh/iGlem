using Microsoft.AspNetCore.Mvc;
using test_v01.Repository;

namespace test_v01.Controllers
{
    public class RecentesController : Controller
    {

        private readonly SITEtccDbContext _context = new SITEtccDbContext();
        public IActionResult Index()
        {
            var documentosRecentes = _context.Documentos
                                     .OrderByDescending(d => d.datacriacao)
                                     .Take(6)
                                     .ToList();

            if (documentosRecentes != null && documentosRecentes.Count > 0)
            {
                ViewBag.DocumentosRecentes = documentosRecentes;
            }
            else
            {
                ViewBag.Message = "Nenhum documento recente encontrado.";
            }

            return View();
        }
    }
}
