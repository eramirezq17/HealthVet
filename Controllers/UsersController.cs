using HealthVet.Models;
using HealthVet.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HealthVet.Controllers
{
    public class UsersController : Controller
    {
        [Authorize(Roles = "cliente")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult ProcessRegisterUser(UsersModel user)
        {
            UsersDAO users = new UsersDAO();
            users.Insert(user);
            return Redirect("/Users/Login");
        }

        [HttpGet("/Users/Login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("/Users/Login")]
        public async Task<IActionResult> ValidateUser(string email, string password, string returnUrl)
        {
            UsersDAO users = new UsersDAO();
            UsersModel ValidatedUser = users.ValidateCredentials(email, password);
            if (ValidatedUser != null)
            {
                TempData["ValidatedUserId"] = ValidatedUser.id;
                HttpContext.Session.SetInt32("id", ValidatedUser.id);
                HttpContext.Session.SetString("email", ValidatedUser.email);
                var claims = new List<Claim>();
                claims.Add(new Claim("username", ValidatedUser.email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ValidatedUser.email));
                claims.Add(new Claim(ClaimTypes.Name, ValidatedUser.name));
                claims.Add(new Claim(ClaimTypes.Email, ValidatedUser.email));
                claims.Add(new Claim(ClaimTypes.Role, ValidatedUser.rol));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    if (ValidatedUser.rol == "cliente")
                    {
                        ViewData["isCliente"] = "yes";
                        return Redirect("/Users/Index");
                    }
                    else
                    {
                        return Redirect("/Home/Index");
                    }
                }
            }
            TempData["Error"] = "Error. Usuario o contraseña incorrectos";
            return View("Login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Home");
        }

        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
