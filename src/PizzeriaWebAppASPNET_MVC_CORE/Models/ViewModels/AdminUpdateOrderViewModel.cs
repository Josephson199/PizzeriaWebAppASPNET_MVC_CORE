using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels
{
    public class AdminUpdateOrderViewModel
    {
        public Kund Kund { get; set; }
        public IEnumerable<Bestallning> Bestallningar { get; set; }
    }
}
