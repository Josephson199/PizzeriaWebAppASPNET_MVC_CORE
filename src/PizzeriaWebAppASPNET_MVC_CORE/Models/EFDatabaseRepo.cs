using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    
    public class EFDatabaseRepo : IDatabaseRepo
    {
        private readonly TomasosContext _context;

        public EFDatabaseRepo(TomasosContext context)
        {
            _context = context;
        }

        public IEnumerable<Produkt> GetProdukter()
        {
            return _context.Produkt.ToList();
        }

        public IEnumerable<Matratt> GetMatratter()
        {
            return _context.Matratt.ToList();
        }
    }
}
