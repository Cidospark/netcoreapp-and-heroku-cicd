using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UMS.Models;

namespace UMS.Data
{
    public class UMSDbContext : IdentityDbContext<AppUser>
    {
        public UMSDbContext(DbContextOptions<UMSDbContext> options): base(options){}

        public DbSet<Photo> Photos { get; set; }
        public DbSet<ClaimValue> ClaimsValues { get; set; }
    }
}
