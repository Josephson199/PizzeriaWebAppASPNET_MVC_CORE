using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public partial class Kund
    {
        public Kund()
        {
            Bestallning = new HashSet<Bestallning>();
        }

        public int KundId { get; set; }
        [Required(ErrorMessage = "Ange namn..")]
        [StringLength(100,ErrorMessage = "Max 100 karaktärer..")]
        
        public string Namn { get; set; }
        [Required(ErrorMessage = "Ange gatuadress..")]
        [StringLength(50, ErrorMessage = "Max 50 karaktärer..")]
        public string Gatuadress { get; set; }
        [Required(ErrorMessage = "Ange postnummer..")]
        [StringLength(20, ErrorMessage = "Max 20 karaktärer..")]
        public string Postnr { get; set; }
        [Required(ErrorMessage = "Ange postort..")]
        [StringLength(100, ErrorMessage = "Max 100 karaktärer..")]
        public string Postort { get; set; }
        [StringLength(50, ErrorMessage = "Max 50 karaktärer..")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Fel format på email adress..")]
        //[EmailAddress(ErrorMessage = "Fel format på email adress..")]
        public string Email { get; set; }
        [StringLength(50, ErrorMessage = "Max 50 karaktärer..")]
        [DataType(DataType.PhoneNumber)]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Ange användarnamn..")]
        [StringLength(20, ErrorMessage = "Max 20 karaktärer..")]
        [RegularExpression("^[^<>.!@#%/]+$",ErrorMessage = "Dessa symboler är förbjudna(^<>.!@#%/)")]
        
        public string AnvandarNamn { get; set; }
        [Required(ErrorMessage = "Ange lösenord..")]
        [StringLength(20, ErrorMessage = "Max 20 karaktärer..")]
        [DataType(DataType.Password)]
       
        public string Losenord { get; set; }
        //[Required]
        //[Compare("Losenord",ErrorMessage = "Lösenordet måste stämma överens...")]
        //public string ComparePassword { get; set; }

        public virtual ICollection<Bestallning> Bestallning { get; set; }
    }
}
