﻿using System;
using System.Collections.Generic;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public partial class AspNetRoleClaims
    {
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public string RoleId { get; set; }

        public virtual AspNetRoles Role { get; set; }
    }
}
