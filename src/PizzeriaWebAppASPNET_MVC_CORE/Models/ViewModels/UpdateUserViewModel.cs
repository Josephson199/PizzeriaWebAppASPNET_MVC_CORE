using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models.ViewModels
{
    public class UpdateUserViewModel 
    {
        public Kund Kund { get; set; }
        public ApplicationUser AspNetUser { get; set; }
        [Required(ErrorMessage = "Ange lösenord..")]
        [StringLength(20, ErrorMessage = "Lösenordet måste innehålla mellan 8 och 20 tecken..",MinimumLength = 8)]
        [DataType(DataType.Password)]

        public string Password { get; set; }

    }



}
