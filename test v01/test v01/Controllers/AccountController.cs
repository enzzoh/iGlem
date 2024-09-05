using Microsoft.AspNetCore.Mvc;

namespace test_v01.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {

            return View();
        }
    }
}
