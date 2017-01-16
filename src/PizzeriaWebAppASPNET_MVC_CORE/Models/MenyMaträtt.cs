using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public class MenyMaträtt
    {
        public Matratt Matratt { get; set; }
        public IEnumerable<Produkt> Produkter { get; set; }
    }
}
