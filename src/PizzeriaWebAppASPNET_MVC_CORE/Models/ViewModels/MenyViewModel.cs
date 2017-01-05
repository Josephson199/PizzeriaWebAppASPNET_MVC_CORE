using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels
{
    public class MenyViewModel : BaseViewModel
    {

        public IEnumerable<MenyMaträtt> MenyMaträtter { get; set; }

        



    }
}
