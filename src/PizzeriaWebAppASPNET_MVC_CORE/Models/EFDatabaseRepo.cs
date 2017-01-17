using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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



        public void SaveMatratt(Matratt matratt)
        {

            _context.Matratt.Add(matratt);
            _context.SaveChanges();

        }

        public void SaveMatrattProdList(List<MatrattProdukt> matrattProdList)
        {
            foreach (var matrattProdukt in matrattProdList)
            {
                _context.MatrattProdukt.Add(matrattProdukt);
                _context.SaveChanges();
            }
        }

        public Matratt GetLatestSavedMatratt()
        {
            return _context.Matratt.OrderByDescending(x => x.MatrattId).FirstOrDefault();
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

        public IEnumerable<Produkt> GetProdukterInMatratt(int marattId)
        {
            var result = _context.MatrattProdukt.Where(x => x.MatrattId == marattId).Select(x => x.Produkt).ToList();

            if (result != null)
            {
                return result;
            }

            return new List<Produkt>();
        }

        public Matratt GetMatratt(int id)
        {
            return _context.Matratt.FirstOrDefault(x => x.MatrattId == id);
        }

        public IEnumerable<MatrattTyp> GetMatrattTyper()
        {
            return _context.MatrattTyp.ToList();
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

        public IEnumerable<Bestallning> GetBeställningar(int kundId)
        {
            var beställningar = _context.Bestallning.Where(x => x.KundId == kundId).ToList().OrderByDescending(y=> y.BestallningDatum);

            if (beställningar != null)
            {
                return beställningar;
            }

            return new List<Bestallning>();
            
        }

        public bool UpdateUser(Kund kund)
        {
            var updatedKund = _context.Kund.FirstOrDefault(x => x.KundId == kund.KundId);

            if (updatedKund != null)
            {
             
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

        public Kund GetKund(string userId)
        {
            var result =_context.Kund.SingleOrDefault(x => x.UserId == userId);

            if (result == null)
            {
                return new Kund();
            }

           return result;
        }

        public Kund GetKund(int kundId)
        {
            var result = _context.Kund.FirstOrDefault(x => x.KundId == kundId);

            if (result == null)
            {
                return new Kund();
            }

            return result;
        }

        public void SaveBeställning(Bestallning bestallning)
        {


            _context.Bestallning.Add(bestallning);
            _context.SaveChanges();

        }

        public Bestallning GetLatestBest()
        {
            return _context.Bestallning.OrderByDescending(x => x.BestallningId).FirstOrDefault();
        }

        public void SaveBestMatratt(BestallningMatratt bestMatratt)
        {
            _context.BestallningMatratt.Add(bestMatratt);
            _context.SaveChanges();
        }

        public IEnumerable<Kund> GetKunder()
        {
            return _context.Kund.ToList();
        }

        public IEnumerable<AspNetRoles> GetUserRoles(string userId)
        {
            return _context.AspNetUserRoles.Where(x => x.UserId == userId).Select(z => z.Role).ToList();
            
        }

        public void DeliverOrder(int bestId)
        {
            var best = _context.Bestallning.FirstOrDefault(x => x.BestallningId == bestId);

            if (best != null && best.Levererad != true)
            {
                best.Levererad = true;

                _context.SaveChanges();
            }

           
        }

        public void DeleletOrder(int bestId)
        {
            var bestMatrattToRemove = _context.BestallningMatratt.Where(x => x.BestallningId == bestId).ToList();

            if (bestMatrattToRemove != null)
            {
                foreach (var bestallningMatratt in bestMatrattToRemove)
            {
                _context.BestallningMatratt.Remove(bestallningMatratt);
            }
                _context.SaveChanges();
            }

             var bestToRemove = _context.Bestallning.FirstOrDefault(x => x.BestallningId == bestId);

            if (bestToRemove != null)
            {
                _context.Bestallning.Remove(bestToRemove);

                _context.SaveChanges();

            }
            


        }

        public int SaveBeställning(IEnumerable<Matratt> matratter, Kund kund, bool isInRolePremium)
        {
            int newBonusPoints;

            int currentKundBonusPoints = 0;

            string currentKundPoints = kund.Points;

            if (currentKundPoints != null)
            {
                 currentKundBonusPoints = Int32.Parse(currentKundPoints);
            }
           

            

            double totalSum = matratter.Sum(x => x.Pris);

            if (isInRolePremium)
            {
                if (matratter.Count() >= 3)
                {
                    totalSum = totalSum*0.8;
                }

                newBonusPoints = matratter.Count()*10;

                var updatedPoints = newBonusPoints + currentKundBonusPoints;

                if (updatedPoints >= 100)
                {
                   var removeFromBill = matratter.Min(x => x.Pris);

                    totalSum = totalSum - removeFromBill;

                    updatedPoints = updatedPoints - 100;
                }


               var uptPointsStr = updatedPoints.ToString();

               var updateKund = _context.Kund.FirstOrDefault(x => x.KundId == kund.KundId);

                updateKund.Points = uptPointsStr;

                _context.SaveChanges();



            }

            var beställning = new Bestallning
            {
                Kund = kund,
                BestallningDatum = DateTime.Now,
                Totalbelopp = (int)totalSum
            };


            SaveBeställning(beställning);


            return (int) totalSum;

        }


        public void AddIngredient(string name)
        {

            if (name != null && name.Length > 0)
            {
                var prod = new Produkt()
                {
                    ProduktNamn = name
                };

                _context.Produkt.Add(prod);
                _context.SaveChanges();
            }
            
           
        }

        public void ReplaceProdukterInMatratt(int matrattId, int[] prodIdsToReplaceWith)
        {
            var currentProds = _context.MatrattProdukt.Where(x => x.MatrattId == matrattId).ToList();


            foreach (var matrattProdukt in currentProds)
            {
                _context.MatrattProdukt.Remove(matrattProdukt);
            }

            _context.SaveChanges();

            if (prodIdsToReplaceWith != null)
            {
                  foreach (var prodId in prodIdsToReplaceWith)
            {

                var matrattProdukt = new MatrattProdukt()
                {
                    MatrattId = matrattId,
                    ProduktId = prodId
                };

                _context.MatrattProdukt.Add(matrattProdukt);

            }

            _context.SaveChanges();
            }

          
        }

        public void UpdateMatratt(Matratt matratt)
        {

            if (matratt != null)
            {
                  var matrattToChange =  _context.Matratt.FirstOrDefault(x => x.MatrattId == matratt.MatrattId);

                    matrattToChange.Beskrivning = matratt.Beskrivning;
                    matrattToChange.MatrattNamn = matratt.MatrattNamn;
                    matrattToChange.MatrattTyp = matratt.MatrattTyp;
                    matrattToChange.Pris = matratt.Pris;

                  _context.SaveChanges();
            }

           
        }
    }   
}
