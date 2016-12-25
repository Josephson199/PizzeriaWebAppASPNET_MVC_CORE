using System;
using System.Collections.Generic;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public partial class Matratt
    {
        public Matratt()
        {
            BestallningMatratt = new HashSet<BestallningMatratt>();
            MatrattProdukt = new HashSet<MatrattProdukt>();
        }

        public int MatrattId { get; set; }
        public string MatrattNamn { get; set; }
        public string Beskrivning { get; set; }
        public int Pris { get; set; }
        public int MatrattTyp { get; set; }

        public virtual ICollection<BestallningMatratt> BestallningMatratt { get; set; }
        public virtual ICollection<MatrattProdukt> MatrattProdukt { get; set; }
        public virtual MatrattTyp MatrattTypNavigation { get; set; }
    }
}
