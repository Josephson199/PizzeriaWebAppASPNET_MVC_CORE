using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public class CartSesssion
    {
        public Kund Kund { get; set; }
        public ICollection<Matratt> Maträtter { get; set; }
      
    }
}
