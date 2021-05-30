using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TradeMate.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace TradeMate.Controllers
{
    public class AccountController : Controller
    {
        TradeMateContext _context;
        public AccountController(TradeMateContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                bool CheckCredentials = IsAuthenticated(user.Email, user.Password);
                if (CheckCredentials)
                {
                    ClaimsIdentity identity = new ClaimsIdentity(new[] {
                                    new Claim(ClaimTypes.Name, user.Email)
                                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                if (_context.SaveChanges() > 1)
                {
                    bool CheckCredentials = IsAuthenticated(user.Email, user.Password);
                    if (CheckCredentials)
                    {
                        ClaimsIdentity identity = new ClaimsIdentity(new[] {
                                    new Claim(ClaimTypes.Name, user.Email)
                                }, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        private bool IsAuthenticated(string username, string password)
        {
            bool Auth = false;
            try
            {
                var user = from u in _context.Users
                           where u.Email == username
                           && u.Password == password
                           select u;
                int i = user.Count();

                if (i > 0)
                {
                    Auth = true;
                    return Auth;
                }
                else
                {
                    return Auth;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace.ToString());
                return Auth;
            }
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }
        public ActionResult AccessDenied()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
    }
}
