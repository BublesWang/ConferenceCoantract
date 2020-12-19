
using MemberAPI.DBModels;
using Microsoft.EntityFrameworkCore;

namespace ConferenceContractAPI.CCDBContext
{
    public class MemberDBContext : DbContext
    {
        public MemberDBContext()
        {
        }
        public MemberDBContext(DbContextOptions<MemberDBContext> options) : base(options)
        {
        }

        public DbSet<Member> Member { get; set; }

        public DbSet<Company> Company { get; set; }
    }
}