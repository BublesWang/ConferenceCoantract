using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CompanyContract
    {
        public CompanyContract()
        {
            delegateServicePackDiscount = new HashSet<DelegateServicePackDiscount>();
            personContract = new HashSet<PersonContract>();
        }

        [Key]
        public Guid ContractId { get; set; }

        public Guid? ConferenceContractId { get; set; }

        [ForeignKey("ConferenceContractId")]
        public virtual ConferenceContract conferenceContract { get; set; }

        public Guid? CompanyServicePackId { get; set; }

        [ForeignKey("CompanyServicePackId")]
        public virtual CompanyServicePack companyServicePack { get; set; }

        public Guid? CompanyId { get; set; }

        [MaxLength(500)]
        public string ConferenceId { get; set; }

        [MaxLength(500)]
        public string ConferenceName { get; set; }

        [MaxLength(500)]
        public string ComNameTranslation { get; set; }

        [MaxLength(150)]
        public string ComContractNumber { get; set; }

        [MaxLength(50)]
        public string Country { get; set; }

        [MaxLength(500)]
        public string AddressTranslation { get; set; }

        [MaxLength(50)]
        public string ComPrice { get; set; }

        public int? MaxContractNumber { get; set; }

        public int? MaxContractNumberSatUse { get; set; }

        public bool? CCIsdelete { get; set; }

        public int? EnterpriseType { get; set; }

        public bool? IsCheckIn { get; set; }

        public bool? IsVerify { get; set; }

        [MaxLength(5000)]
        public string PPTUrl { get; set; }

        [MaxLength(50)]
        public string ContractStatusCode { get; set; }

        [MaxLength(500)]
        public string Owerid { get; set; }

        [MaxLength(50)]
        public string Ower { get; set; }

        [MaxLength(50)]
        public string ContractCode { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public string OtherOwnerId { get; set; }
        public string OtherOwner { get; set; }


        public virtual ICollection<DelegateServicePackDiscount> delegateServicePackDiscount { get; set; }

        public virtual ICollection<PersonContract> personContract { get; set; }
    }
}
