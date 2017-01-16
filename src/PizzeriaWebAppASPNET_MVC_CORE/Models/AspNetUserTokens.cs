using System;
using System.Collections.Generic;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public partial class AspNetUserTokens
    {
        public string UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
