using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PizzeriaWebAppASPNET_MVC_CORE.Extensions;
using PizzeriaWebAppASPNET_MVC_CORE.Models;
using PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzeriaWebAppASPNET_MVC_CORE.Controllers
{
    [Route("Cart")]
    public class ShoppingCartController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EFDatabaseRepo _repository;


        public ShoppingCartController(EFDatabaseRepo repo,
                SignInManager<ApplicationUser> signInManager,
                UserManager<ApplicationUser> userManager 
           )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repo;
        }
       




        [Route("")]
        [Route("Cart")]
        public IActionResult ShoppingCart()
        {
            var cartSession = HttpContext.Session.GetObjectFromJson<CartSesssion>("CartSession");

            if (cartSession != null)
            {
                var model = new ShoppingCartViewModel() { CartSesssion = cartSession };
                return View(model);
            }
            else
            {
                var model = new ShoppingCartViewModel() { CartSesssion = new CartSesssion()};
                return View(model);
            }
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart(int maträttId)
        {
            var maträtt = _repository.GetMatratt(maträttId);
            
            var cartSession = HttpContext.Session.GetObjectFromJson<CartSesssion>("CartSession");

            

            if (cartSession == null)
            {
                
                
                var matList = new List<Matratt>() {maträtt};

                var cartSesssion = new CartSesssion()
                {
                    Maträtter = matList
                   
                };
                HttpContext.Session.SetObjectAsJson("CartSession", cartSesssion);
            }
            else
            {
            
                
                cartSession.Maträtter.Add(maträtt);
                HttpContext.Session.SetObjectAsJson("CartSession", cartSession);
            }

            return RedirectToAction("Meny", "Home");
        }





        [Route("Checkout")]
        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cartSession = HttpContext.Session.GetObjectFromJson<CartSesssion>("CartSession");

            var appUser = await _userManager.GetUserAsync(HttpContext.User);

            var kund = _repository.GetKund(appUser.Id);


            var matratter = cartSession.Maträtter;

            var beställning = new Bestallning
            {
                Kund = kund,
                BestallningDatum = DateTime.Now,
                Totalbelopp = matratter.Sum(x => x.Pris)
            };

            _repository.SaveBeställning(beställning);

            var latestBest = _repository.GetLatestBest();

            foreach (var matratt in matratter.GroupBy(x => x.MatrattId))
            {
                var bestMatratt = new BestallningMatratt()
                {
                    BestallningId = latestBest.BestallningId,
                    MatrattId = matratt.Key,
                    Antal = matratt.Count()
                    
                };
                _repository.SaveBestMatratt(bestMatratt);
               
            }

            var matratterToShowAtCheckout = cartSession.Maträtter;

            HttpContext.Session.SetString("CartSession", "");

            var model = new CheckOutViewModel()
            {
                Matratter = matratterToShowAtCheckout,
                Kund = kund
                
            };

            return View(model);
        }



        [HttpGet]
        public IActionResult RemoveFromCart(int id)
        {

            var cartSession = HttpContext.Session.GetObjectFromJson<CartSesssion>("CartSession");

            cartSession.Maträtter.Remove(cartSession.Maträtter.FirstOrDefault(x => x.MatrattId == id));
            
            HttpContext.Session.SetObjectAsJson("CartSession", cartSession);

            return RedirectToAction("ShoppingCart");
        }
    }
}
