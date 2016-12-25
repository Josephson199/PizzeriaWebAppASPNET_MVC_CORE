using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebAppASPNET_MVC_CORE.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace PizzeriaWebAppASPNET_MVC_CORE.Controllers
{
    [Route("")]
    [Route("Home")]
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
            return View(_repository.GetMatratter());
        }

    }
}
