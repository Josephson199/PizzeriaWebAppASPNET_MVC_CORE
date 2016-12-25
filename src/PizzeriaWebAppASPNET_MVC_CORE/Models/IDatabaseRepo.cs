using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public interface IDatabaseRepo
    {
        IEnumerable<Produkt> GetProdukter();

        IEnumerable<Matratt> GetMatratter();

    }
}
