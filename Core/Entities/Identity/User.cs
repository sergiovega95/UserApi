using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities.Identity
{
    public class User: IdentityUser<Guid>
    {         
        //You can add more properties and after do a migration
    }
}
