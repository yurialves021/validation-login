﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using validation_login.ViewModels;

namespace validation_login.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.UserName };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    this.ModelState.AddModelError("Registro", "Falha ao registrar usuário");
                }
            }
            return View(model);
        }


        public IActionResult Login(string returnUrl)
        {
            //enviar o usuario para onde ele estava quando tentou realizar o login
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            var user = await _userManager.FindByNameAsync(loginVM.UserName); //verificando o nome do usuário

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(loginVM.ReturnUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    return Redirect(loginVM.ReturnUrl); //enviar o usuario para onde ele estava quando tentou realizar o login
                }
            }

            ModelState.AddModelError(string.Empty, "Falha ao realizar o login!!");
            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.User = null;
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
