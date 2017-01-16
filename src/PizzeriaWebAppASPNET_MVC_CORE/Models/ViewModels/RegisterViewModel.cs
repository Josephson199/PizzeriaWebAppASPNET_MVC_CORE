using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels
{
    public class RegisterViewModel { 
        public Kund Kund { get; set; }
        public AspNetUsers AspNetUser { get; set; }
        [Required(ErrorMessage = "Ange lösenord..")]
        [StringLength(20, ErrorMessage = "Max 20 karaktärer..")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
      
    }
}
