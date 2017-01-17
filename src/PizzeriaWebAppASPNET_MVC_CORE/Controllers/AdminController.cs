using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PizzeriaWebAppASPNET_MVC_CORE.Infrastructure;
using PizzeriaWebAppASPNET_MVC_CORE.Models;
using PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels;


namespace PizzeriaWebAppASPNET_MVC_CORE.Controllers
{
    [Route("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EFDatabaseRepo _repository;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AdminController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            EFDatabaseRepo repo,
            IPasswordHasher<ApplicationUser> passwordHash,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _repository = repo;
            _passwordHasher = passwordHash;
            _roleManager = roleManager;

        }


        [Route("Index")]
        public ViewResult Roles()
        {
            var roles = _roleManager.Roles;

            var model = new AdminViewModel()
            {
                Roles = roles
            };
            return View(model);
        }




        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result
                    = await _roleManager.CreateAsync(new IdentityRole(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    //TODO LOG
                }
            }
            return RedirectToAction("Roles");
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    //LOG
                }
            }
            else
            {
                ModelState.AddModelError("", "No role found");
            }
            return RedirectToAction("Roles");
        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit(AdminViewModel.RoleModificationModel model)
        {


            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] {})
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.AddToRoleAsync(user,
                            model.RoleName);
                        if (!result.Succeeded)
                        {
                            //todo LOG
                        }
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] {})
                {
                    ApplicationUser user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await _userManager.RemoveFromRoleAsync(user,
                            model.RoleName);
                        if (!result.Succeeded)
                        {
                            //todo LOG
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Roles));
            }
            else
            {
                return RedirectToAction(nameof(Roles));
            }
        }



        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            List<ApplicationUser> members = new List<ApplicationUser>();
            List<ApplicationUser> nonMembers = new List<ApplicationUser>();
            foreach (ApplicationUser user in _userManager.Users)
            {
                var list = await _userManager.IsInRoleAsync(user, role.Name)
                    ? members
                    : nonMembers;
                list.Add(user);
            }
            return View(new AdminViewModel.RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [Route("Users")]
        public IActionResult Users()
        {
            var kunder = _repository.GetKunder();

            var model = new UserViewModel() {Kunder = kunder};


            return View(model);
        }

        [Route("EditUserPartial")]
        public async Task<IActionResult> EditUserPartial(int kundId)
        {

            var kund = _repository.GetKund(kundId);

            var aspNetUser = await _userManager.FindByIdAsync(kund.UserId);

            if (aspNetUser != null)
            {

                var model = new AdminUpdateUserViewModel()
                {
                    Kund = kund,
                    AspNetUser = aspNetUser
                };

                return PartialView("_PartialEditUser", model);

            }

            return RedirectToAction("Users");

        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(AdminUpdateUserViewModel model)
        {


            if (ModelState.IsValid)
            {

                var appUser = await _userManager.FindByIdAsync(model.AspNetUser.Id);

                if (appUser != null)
                {
                    _repository.UpdateUser(model.Kund);

                    appUser.UserName = model.AspNetUser.UserName;

                    var updateResult = await _userManager.UpdateAsync(appUser);

                    if (updateResult.Succeeded)
                    {
                        return PartialView("_PartialUpdateSuccess");
                    }
                }

            }
            return PartialView("_PartialEditUser", model);
        }

        [Route("UpdatePassword")]
        [HttpGet]
        [ValidateActionParameters]
        public async Task<IActionResult> UpdatePassword(
            [Required] [StringLength(8, ErrorMessage = "Minst 8 karaktärer..")] string password, string aspNetUserId,
            int kundId)
        {
            ViewBag.Updated = "false";
            if (ModelState.IsValid)
            {

                var appUser = await _userManager.FindByIdAsync(aspNetUserId);

                appUser.PasswordHash = _passwordHasher.HashPassword(appUser,
                    password);

                ViewBag.UpdatedPassword = "true";

                return PartialView("_PartialUpdateSuccess");
            }


            return RedirectToAction("Users");
        }



        [Route("EditOrderPartial")]
        public async Task<IActionResult> EditOrderPartial(int kundId)
        {

            var kund = _repository.GetKund(kundId);

            var beställningar = _repository.GetBeställningar(kundId);

            var aspNetUser = await _userManager.FindByIdAsync(kund.UserId);

            if (aspNetUser != null)
            {

                var model = new AdminUpdateOrderViewModel()
                {
                    Kund = kund,
                    Bestallningar = beställningar
                };

                return PartialView("_PartialOrderUser", model);

            }

            return RedirectToAction("Users");

        }

        [Route("Deliver")]
        public async Task<IActionResult> Deliver(int bestId, int kundId)
        {

            _repository.DeliverOrder(bestId);


            var kund = _repository.GetKund(kundId);

            var beställningar = _repository.GetBeställningar(kundId);

            var aspNetUser = await _userManager.FindByIdAsync(kund.UserId);

            if (aspNetUser != null)
            {

                var model = new AdminUpdateOrderViewModel()
                {
                    Kund = kund,
                    Bestallningar = beställningar
                };

                
                return PartialView("_PartialOrderUser", model);

                
            }

            return RedirectToAction("Users");
        }
        [Route("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int bestId, int kundId)
        {
            _repository.DeleletOrder(bestId);

            var kund = _repository.GetKund(kundId);

            var beställningar = _repository.GetBeställningar(kundId);

            var aspNetUser = await _userManager.FindByIdAsync(kund.UserId);

            if (aspNetUser != null)
            {

                var model = new AdminUpdateOrderViewModel()
                {
                    Kund = kund,
                    Bestallningar = beställningar
                };


                return PartialView("_PartialOrderUser", model);


            }

            return RedirectToAction("Users");
        }



        [Route("Foods")]
        [HttpGet]
        public IActionResult Foods()
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




            var produkter = _repository.GetProdukter();
            
           var matratTyper = _repository.GetMatrattTyper();

            var model = new AdminFoodsViewModel.CreateFood();
            model.Produkter = produkter;
            model.MatrattTyper = matratTyper;
            model.MenyMaträtter = listMenyMaträtter;
            
        
            return View(model);
        }




        [Route("Foods")]
        [HttpPost]
        public IActionResult Foods(AdminFoodsViewModel.CreateFood model)
        {


           _repository.SaveMatratt(model.Matratt);

           var matrattWithId = _repository.GetLatestSavedMatratt();

            var matProdList = new List<MatrattProdukt>();


            foreach (var prodId in model.ProduktIdsToAdd)
            {
                var matrattProd = new MatrattProdukt()
                {
                    ProduktId = prodId,
                    MatrattId = matrattWithId.MatrattId
                    
                };
                
                matProdList.Add(matrattProd);

            }

            _repository.SaveMatrattProdList(matProdList);


            return RedirectToAction("Foods");
        }



        [Route("FoodEditPartial")]
        [HttpPost]
        public IActionResult FoodEditPartial(int matrattId)
        {
            var matrattTyper = _repository.GetMatrattTyper();
            var produkterInMatratt = _repository.GetProdukterInMatratt(matrattId);

            var model = new AdminFoodsViewModel.CreateFood();

            model.Matratt = _repository.GetMatratt(matrattId);
            model.ProdukterInMatratt = produkterInMatratt;
            model.MatrattTyper = matrattTyper;
            model.Produkter = _repository.GetProdukter();

            return PartialView("_PartialEditFood", model);
        }

        public IActionResult AddIngredient(string ingredientName)
        {

            _repository.AddIngredient(ingredientName);

          return RedirectToAction("Foods");
        }


        [Route("UpdateFood")]
        [HttpPost]
        public IActionResult UpdateFood(AdminFoodsViewModel.CreateFood model)
        {

            _repository.ReplaceProdukterInMatratt(model.Matratt.MatrattId, model.ProduktIdsToAdd);

            _repository.UpdateMatratt(model.Matratt);




            return PartialView("_PartialUpdateSuccessFood");

          


        }
    }


    
}
