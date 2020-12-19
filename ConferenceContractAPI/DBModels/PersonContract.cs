using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class PersonContract
    {
        //public PersonContract()
        //{
        //    extraService = new HashSet<ExtraService>();
        //}

        [Key]
        public Guid PersonContractId { get; set; }

        [MaxLength(500)]
        public string ConferenceId { get; set; }

        public Guid? ContractId { get; set; }

        [ForeignKey("ContractId")]
        public virtual CompanyContract companyContract { get; set; }

        [MaxLength(150)]
        public string PerContractNumber { get; set; }

        public Guid? MemberPK { get; set; }

        [MaxLength(500)]
        public string MemTranslation { get; set; }

        public bool? PCIsdelete { get; set; }

        [MaxLength(50)]
        public string CTypeCode { get; set; }

        [MaxLength(500)]
        public string InviteCodeId { get; set; }

        [MaxLength(500)]
        public string Owerid { get; set; }

        [MaxLength(50)]
        public string Ower { get; set; }

        [MaxLength(150)]
        public string PaidAmount { get; set; }

        public bool? IsCheckIn { get; set; }

        public bool? IsSendEmail { get; set; }

        public bool? IsModify { get; set; }

        public bool? IsFianceRecord { get; set; }
        public bool? IsInviteCode { get; set; }
        public bool? IsCommitAbstract { get; set; }
        public bool? IsPrint { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }
        [MaxLength(50)]
        public string Year { get; set; }
        [MaxLength(3000)]
        public string  OtherOwner { get; set; }
        [MaxLength(3000)]
        public string OtherOwnerId { get; set; }

        public Guid? CompanyServicePackId { get; set; }

        [MaxLength(50)]
        public string CompanyServicePackCode { get; set; }


        public Guid? ConferenceContractId { get; set; }
        [MaxLength(150)]
        public string PerPrice { get; set; }
        

        //public virtual ICollection<ExtraService> extraService { get; set; }
    }
}
