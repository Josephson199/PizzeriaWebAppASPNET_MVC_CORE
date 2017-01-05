using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public class CartSesssion
    {
        public ICollection<Matratt> Maträtter { get; set; }
    }
}
