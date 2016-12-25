using System;
using System.Collections.Generic;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public partial class MatrattProdukt
    {
        public int MatrattId { get; set; }
        public int ProduktId { get; set; }

        public virtual Matratt Matratt { get; set; }
        public virtual Produkt Produkt { get; set; }
    }
}
