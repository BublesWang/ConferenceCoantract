using Microsoft.EntityFrameworkCore;
using RoleServiceAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.CCDBContext
{
    public class RoleDBContext : DbContext
    {

        public RoleDBContext()
        {
        }
        public RoleDBContext(DbContextOptions<RoleDBContext> options) : base(options)
        {
        }


        public DbSet<User> User { get; set; }

    }

}
