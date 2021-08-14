using LaboratorioLopez.Pages;
using LaboratorioLopez.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LaboratorioLopez.Controllers
{    
    public class CuentaController : Controller
    {
        private readonly UserManager<IdentityUser<int>> userManager;
        private readonly SignInManager<IdentityUser<int>> signInManager;

        public CuentaController(UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Route("/Cuenta/Logout")]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }

        [Route("/Cuenta/Registro")]
        [HttpGet]
        public IActionResult Registro() => View();

        [Route("/Cuenta/Registro")]
        [HttpPost]
        public async Task<IActionResult> Registro(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = new IdentityUser<int> { UserName = model.Usuario };
                var resultado = await userManager.CreateAsync(usuario, model.Password);

                if (resultado.Succeeded)
                {
                    await signInManager.SignInAsync(usuario, isPersistent: false);
                    return RedirectToPage("/Index");
                }

                foreach (var error in resultado.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [Route("/Cuenta/Acceso")]
        [HttpGet]
        public IActionResult Acceso() => View();

        [Route("/Cuenta/Acceso")]
        [HttpPost]
        public async Task<IActionResult> Acceso(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {               
                var resultado = await signInManager.PasswordSignInAsync(model.Usuario, model.Password,model.RememberMe,false);

                if (resultado.Succeeded)
                {
                    return RedirectToPage("/ListaPaciente/Index");
                }

                ModelState.AddModelError(string.Empty, "Inicio de Sección Invalido.");
                
            }
            return View(model);
        }

        [Route("/Cuenta/CambiarContraseña")]
        [HttpGet]
        public async Task<IActionResult> CambiarContraseña()
        {
            var user = await userManager.GetUserAsync(User);

            var userHasPassword = await userManager.HasPasswordAsync(user);

            if (!userHasPassword)
            {
                return RedirectToAction("AddPassword");
            }
            return View();
        }

        [Route("/Cuenta/CambiarContraseña")]
        [HttpPost]
        public async Task<IActionResult> CambiarContraseña(CambiarContraseñaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return RedirectToAction("Login");
                }
                
                var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);                                
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View();
                }
                await signInManager.RefreshSignInAsync(user);
                return RedirectToPage("/ListaPaciente/Index");
            }
            return View(model);
        }
    }
}