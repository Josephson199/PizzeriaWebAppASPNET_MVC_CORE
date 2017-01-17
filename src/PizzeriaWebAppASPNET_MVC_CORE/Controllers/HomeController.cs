using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebAppASPNET_MVC_CORE.Models;
using PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzeriaWebAppASPNET_MVC_CORE.Controllers
{
    [Route("")]
    [Route("Home")]
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly EFDatabaseRepo _repository;

        public HomeController(EFDatabaseRepo repo)
        {
            _repository = repo;
        }
        // GET: /<controller>/
        
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("Meny")]
        public IActionResult Meny()
        {
            var maträtter = _repository.GetMatratter();
            var maträttProdukter = _repository.GetMatratterProdukter();

            var listMenyMaträtter = new List<MenyMaträtt>();

            foreach (var maträtt in maträtter)
            {
                var menyMaträtt = new MenyMaträtt();
                var products = new List<Produkt>();
                menyMaträtt.Matratt = maträtt;

                foreach (var prod in maträttProdukter)
                {

                    if (prod.MatrattId == maträtt.MatrattId)
                    {
                        products.Add(prod.Produkt);
                    }
                    
                }

                menyMaträtt.Produkter = products;
                listMenyMaträtter.Add(menyMaträtt);
            }

      

            var model = new MenyViewModel() {MenyMaträtter = listMenyMaträtter};

            return View(model);
        }
        
       
    }
}
