using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaWebAppASPNET_MVC_CORE.Models;
using PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels;

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

        public IEnumerable<Produkt> GetProdukter(int id)
        {
            return _context.Produkt.Where(x => x.ProduktId == id).ToList();
        }

        public IEnumerable<Matratt> GetMatratter()
        {
            return _context.Matratt.ToList();
        }

        public IEnumerable<MatrattProdukt> GetMatratterProdukter()
        {
            return _context.MatrattProdukt.
                Include(x => x.Produkt)
                .ToList();
        }

        public Matratt GetMatratt(int id)
        {
            return _context.Matratt.FirstOrDefault(x => x.MatrattId == id);
        }

        public void SaveUser(Kund kund)
        {
            
            try
            {
                _context.Kund.Add(kund);
                _context.SaveChanges();
               
            }
            catch (Exception ex)
            {
                throw new Exception("SaveUser(); Method failed. Tried to add user to db and savechanges.", ex.InnerException);
                
            }
        }

        public Kund ValidateUserLogin(BaseViewModel login)
        {
            if(!string.IsNullOrWhiteSpace(login.LoginAnvändarnamn) && !string.IsNullOrWhiteSpace(login.LoginLösenord)) { 

            var användarNamn = login.LoginAnvändarnamn.Trim().ToLower();
            var lösenord = login.LoginLösenord.Trim();

            var kund = _context.Kund.FirstOrDefault(u => u.AnvandarNamn == användarNamn);

            if (kund.Losenord == lösenord)
                return kund;

            }

            return null;
            
        }

        public bool UpdateUser(Kund kund)
        {
            var updatedKund = _context.Kund.FirstOrDefault(x => x.KundId == kund.KundId);

            if (updatedKund != null)
            {
                updatedKund.AnvandarNamn = kund.AnvandarNamn;
                updatedKund.Losenord = kund.Losenord;
                updatedKund.Namn = kund.Namn;
                updatedKund.Email = kund.Email;
                updatedKund.Gatuadress = kund.Gatuadress;
                updatedKund.Postnr = kund.Postnr;
                updatedKund.Postort = kund.Postort;
                updatedKund.Telefon = kund.Telefon;

                _context.SaveChanges();
                return true;
                
            }

            return false;
        }

       
        
    }
}
