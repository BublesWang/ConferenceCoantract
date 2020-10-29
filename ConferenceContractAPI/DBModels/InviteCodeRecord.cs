using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class InviteCodeRecord
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? InviteCodeId { get; set; }

        [ForeignKey("InviteCodeId")]
        public virtual InviteCode inviteCode { get; set; }

        [MaxLength(500)]
        public string MemberPK { get; set; }

        [MaxLength(500)]
        public string MemberName { get; set; }

        [MaxLength(500)]
        public string UseDate { get; set; }

        [MaxLength(500)]
        public string PersonContractId { get; set; }

        [MaxLength(1000)]
        public string PersonContractNumber { get; set; }
        public bool? IsDelete { get; set; }

    }
}
