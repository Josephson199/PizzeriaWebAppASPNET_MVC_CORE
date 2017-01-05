using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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



        private readonly EFDatabaseRepo _repository;


        public ShoppingCartController(EFDatabaseRepo repo)
        {
            _repository = repo;
        }
        // GET: /<controller>/
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

                var newCartSesssion = new CartSesssion()
                {
                    Maträtter = matList
                };
                HttpContext.Session.SetObjectAsJson("CartSession", newCartSesssion);
            }
            else
            {
                cartSession.Maträtter.Add(maträtt);
                HttpContext.Session.SetObjectAsJson("CartSession", cartSession);
            }

            return RedirectToAction("Meny", "Home");
        }
        [Route("Checkout")]
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
