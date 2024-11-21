using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using test_v01.Models;
using test_v01.Repository;
using test_v01.Repository.Models;

namespace AccountContoller.Controllers
{
    public class AccountController : Controller
    {
        private readonly SITEtccDbContext _context;

        public AccountController(SITEtccDbContext context)
        {
            _context = context;
        }

        // Exibe a tela de login
        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Homelist", "ListaDocumentos");
            }
            return View();
        }

        // Processa o login
        // Processa o login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string senha, bool rememberMe)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                ModelState.AddModelError(string.Empty, "Preencha todos os campos.");
                return View();
            }

            // Verifique se o usuário existe no banco de dados
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Emailusuario == email && u.Senhausuario == senha);

            if (usuario != null)
            {
                // Crie a lista de claims do usuário
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nomeusuario),
            new Claim(ClaimTypes.Email, usuario.Emailusuario),
            new Claim("Idusuario", usuario.Idusuario.ToString()),
            new Claim("IsAdmin", usuario.IsAdmin.ToString())
        };

                // Configure a autenticação com as propriedades de persistência
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe, // Persistente se 'rememberMe' estiver marcado
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(30) : (DateTimeOffset?)null
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Homelist", "ListaDocumentos");
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
                var existingUser = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Emailusuario == usuario.Emailusuario);

                if (existingUser != null)
                {
                    ModelState.AddModelError("Emailusuario", "Este email já está cadastrado.");
                    return View(usuario);
                }

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
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
