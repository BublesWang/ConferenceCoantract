using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class InviteCode
    {
        public InviteCode()
        {
            inviteCodeRecord = new HashSet<InviteCodeRecord>();
        }

        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string InviteCodeNumber { get; set; }

        [MaxLength(500)]
        public string CompanyServicePackId { get; set; }

        public int Count { get; set; }

        [MaxLength(50)]
        public string Year { get; set; }

        [MaxLength(500)]
        public string WebSite { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }

        public virtual ICollection<InviteCodeRecord> inviteCodeRecord { get; set; }
    }
}
