using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using test_v01.Models;
using test_v01.Repository;
using test_v01.Repository.Models;

namespace test_v01.Controllers
{
    public class HomeController : Controller
    {

        private readonly SITEtccDbContext _context = new SITEtccDbContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Pagina()
        {

            return View();
        }

        public IActionResult Suporte()
        {

            return View();
        }

        public IActionResult Notificacao()
        {

            return View();
        }
        
        public IActionResult entrada()
        {


            return View();
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> usuario()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Idusuario");

            if (userIdClaim != null)
            {
                int userId = int.Parse(userIdClaim.Value);
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Idusuario == userId);

                if (usuario != null)
                {
                    return View(new List<Usuario> { usuario });
                }
            }

            return Problem("Usuário não encontrado.");
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}