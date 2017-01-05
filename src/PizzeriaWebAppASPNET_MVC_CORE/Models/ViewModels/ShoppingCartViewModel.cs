using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels
{
    public class ShoppingCartViewModel : BaseViewModel
    {
        public IEnumerable<Matratt> Matratter { get; set; }
        public CartSesssion CartSesssion { get; set; }


    }
}
