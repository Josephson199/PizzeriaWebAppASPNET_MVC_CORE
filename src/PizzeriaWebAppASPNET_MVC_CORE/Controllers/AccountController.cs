using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebAppASPNET_MVC_CORE.Extensions;
using PizzeriaWebAppASPNET_MVC_CORE.Models;
using PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzeriaWebAppASPNET_MVC_CORE.Controllers
{
    [Route("Account")]
    
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EFDatabaseRepo _repository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public AccountController(
                UserManager<ApplicationUser> userManager, 
                SignInManager<ApplicationUser> signInManager,
                EFDatabaseRepo repo,
                IPasswordHasher<ApplicationUser> passwordHash)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repo;
            _passwordHasher = passwordHash;

        }

      
        [Route("")]
        [Route("Register")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            var kund = new Kund();
            var model = new RegisterViewModel()
            {
                Kund = kund,
            };


            return View(model);
        }



        [Route("Register")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
       public async Task<IActionResult> Register(RegisterViewModel model)
        {
            ViewBag.Success = "false";

            if (ModelState.IsValid)
            {
                try
                {
                    
                    var newUser = new ApplicationUser() {UserName = model.AspNetUser.UserName };
                    var result = await _userManager.CreateAsync(newUser, model.Password);
                    var result2 = await _userManager.AddToRoleAsync(newUser,
                       "RegularUser");

                    if (result.Succeeded && result2.Succeeded)
                    {
                        await _signInManager.SignInAsync(newUser, isPersistent: false);
                            model.Kund.UserId = newUser.Id;

                            _repository.SaveUser(model.Kund);
                            ViewBag.Success = "true";

                       return RedirectToAction("Meny", "Home");

                    }

                    
                    return View(model);
                }
                catch (Exception)
                {
                    
                    throw ;
                }
                
            }

            return View(model);
        }



        [Route("Login")]
        [HttpGet]
        
        public async Task<IActionResult> Login(string username, string password)
        {
           
                 var result = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

                 if (result.Succeeded)
                 {
                       return RedirectToAction("Meny", "Home");
                 }
            

            return RedirectToAction("Meny", "Home");
            
          
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {

            HttpContext.Session.SetObjectAsJson("CartSession",new CartSesssion() {Maträtter = new List<Matratt>()});

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Route("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser()
        {
            ViewBag.Updated = "false";

            var appUser = await _userManager.GetUserAsync(HttpContext.User);

            var kund = _repository.GetKund(appUser.Id);

            var model = new UpdateUserViewModel()
            {
                Kund = kund,
                AspNetUser = appUser
            };

            return View(model);
        }

        [Route("UpdateUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserViewModel model)
        {

            ViewBag.Updated = "false";

            if (ModelState.IsValid)
            {

                _repository.UpdateUser(model.Kund);

                var appUser = await _userManager.GetUserAsync(HttpContext.User);

                if (appUser != null)
                {
                    appUser.PasswordHash = _passwordHasher.HashPassword(appUser,
                           model.Password);
                    appUser.UserName = model.AspNetUser.UserName;

                    var updateResult = await _userManager.UpdateAsync(appUser);

                    if (updateResult.Succeeded)
                    {
                        ViewBag.Updated = "true";

                        RedirectToAction("UpdateUser");
                    }
                }

            }
           

           

          
            return View(model);
        }

    }
}
