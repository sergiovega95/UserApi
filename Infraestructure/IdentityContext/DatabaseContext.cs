using Core.Entities.Identity;
using Core.Entities.Shared;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Infrastructure.IdentityContext
{
    public class DatabaseContext : IdentityDbContext<User, Role, Guid>
    {
        public DatabaseContext (DbContextOptions<DatabaseContext> options)
            : base(options)
        {
            
            
        }

        public DbSet<DocumentType> DocumentType { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
    }
}
