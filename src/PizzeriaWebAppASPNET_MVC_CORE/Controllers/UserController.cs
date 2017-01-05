using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebAppASPNET_MVC_CORE.Extensions;
using PizzeriaWebAppASPNET_MVC_CORE.Models;
using PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzeriaWebAppASPNET_MVC_CORE.Controllers
{
    [Route("User")]
    public class UserController : Controller
    {
        
       

        private readonly EFDatabaseRepo _repository;
       

        public UserController(EFDatabaseRepo repo)
        {
            _repository = repo;
        }
        // GET: /<controller>/
        [Route("")]
        [Route("Register")]
        [HttpGet]
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
        public IActionResult Register(RegisterViewModel model)
        {
            ViewBag.Success = false;

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.SaveUser(model.Kund);
                    ViewBag.Success = true;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(BaseViewModel login)
        {
           var kund = _repository.ValidateUserLogin(login);

            if (kund != null)
            {
               HttpContext.Session.SetObjectAsJson("Kund", kund);
               HttpContext.Session.SetString("Status", "active");

            }

           return RedirectToAction("Meny", "Home");
        }
        [HttpPost]
        [Route("Logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("Status", "inactive");

            return RedirectToAction("Meny", "Home");
        }
        [HttpGet]
        [Route("UpdateUser")]
        public IActionResult UpdateUser()
        {
            var kund = HttpContext.Session.GetObjectFromJson<Kund>("Kund");

            var model = new UpdateUserViewModel()
            {
                Kund = kund
            };

            return View(model);
        }
        
        [Route("UpdateUser")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateUser(Kund kund)
        {

            _repository.UpdateUser(kund);

            ViewBag.Success = true;

           var model = new UpdateUserViewModel()
            {
                Kund = kund
            };

            return View(model);
        }

    }
}
