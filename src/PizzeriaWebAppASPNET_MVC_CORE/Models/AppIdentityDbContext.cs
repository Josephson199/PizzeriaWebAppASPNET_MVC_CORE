using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaWebAppASPNET_MVC_CORE.Models
{
    public class AppIdentityDbContext : IdentityDbContext
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) 
            : base(options) { }
    }
}
