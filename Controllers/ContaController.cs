using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorTarefas.MVC.Data;
using GerenciadorTarefas.MVC.Models;
using GerenciadorTarefas.MVC.ViewModels;

namespace GerenciadorTarefas.MVC.Controllers
{
    public class ContaController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<Usuario> _passwordHasher = new();

        public ContaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Conta/Registro
        [AllowAnonymous]
        public IActionResult Registro()
        {
            return View();
        }

        // POST: Conta/Registro
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registro(CadastroViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var emailJaExiste = await _context.Usuarios.AnyAsync(u => u.Email == model.Email);
            if (emailJaExiste)
            {
                ModelState.AddModelError("Email", "Este e-mail já está cadastrado.");
                return View(model);
            }

            var usuario = new Usuario
            {
                Email = model.Email
            };

            usuario.SenhaHash = _passwordHasher.HashPassword(usuario, model.Senha);

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Login));
        }

        // GET: Conta/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Conta/Login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (usuario == null)
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(model);
            }

            var resultado = _passwordHasher.VerifyHashedPassword(usuario, usuario.SenhaHash, model.Senha);
            if (resultado == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("", "E-mail ou senha inválidos.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        // POST: Conta/Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // GET: Conta/AcessoNegado
        [AllowAnonymous]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}