﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels
{
    public class CheckOutViewModel 
    {
        public IEnumerable<Matratt> Matratter { get; set; }

        public Kund Kund { get; set; }
        public int TotalKostnad { get; set; }
    }
}
