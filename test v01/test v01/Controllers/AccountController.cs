using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;
using System.Threading.Tasks;
using test_v01.Models;
using test_v01.Repository;
using test_v01.Repository.Models; 

namespace AccountContoller.Controllers
{
    public class AccountController : Controller
    {
        private readonly SITEtccDbContext _context = new SITEtccDbContext();

      

        // Exibe o formulário de login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Processa o login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                ModelState.AddModelError(string.Empty, "Preencha todos os campos.");
                return View();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Emailusuario == email && u.Senhausuario == senha);

            if (usuario != null)
            {
                // Cria as claims do usuário
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nomeusuario),
            new Claim(ClaimTypes.Email, usuario.Emailusuario),
            new Claim("Idusuario", usuario.Idusuario.ToString()), // Adiciona o ID do usuário como claim
            new Claim("IsAdmin", usuario.IsAdmin.ToString()) // Adiciona uma claim para saber se é admin
        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                // Autentica o usuário com cookies
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("homelist", "Listadocumentos");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
                return View();
            }
        }

        // Exibe o formulário de cadastro
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Processa o cadastro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Idusuario,Emailusuario,Senhausuario,Nomeusuario,IsAdmin")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction("Homelist", "ListaDocumentos");
            }
            return View(usuario);
        }

        // Logout
        public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Clear(); // Limpa a sessão
			return RedirectToAction("Login");
		}
	}
}
