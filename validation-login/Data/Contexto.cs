using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace validation_login.Data
{
    public class Contexto : IdentityDbContext<IdentityUser>
    {
        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {
                    
        }
    }
}
