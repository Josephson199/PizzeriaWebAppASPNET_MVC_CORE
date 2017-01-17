using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels
{
    public class AdminFoodsViewModel
    {
        public class CreateFood
        {
            public Matratt Matratt { get; set; }
            public IEnumerable<MatrattTyp> MatrattTyper { get; set; }
            public IEnumerable<Produkt> Produkter { get; set; }
            public int[] ProduktIdsToRemove { get; set; }
            public int[] ProduktIdsToAdd { get; set; }

            public IEnumerable<MenyMaträtt> MenyMaträtter { get; set; }
            public IEnumerable<Produkt> ProdukterInMatratt { get; set; }
        }
       
        
        

        //public IEnumerable<ProduktCheckBox> CheckBoxes { get; set; }
    }
}
