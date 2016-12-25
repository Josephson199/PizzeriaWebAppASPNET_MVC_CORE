using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzeriaWebAppASPNET_MVC_CORE.Controllers
{
    [Route("Cart")]
    public class ShoppingCartController : Controller
    {
        // GET: /<controller>/
        [Route("")]
        public IActionResult ShoppingCart()
        {
            return View();
        }
    }
}
