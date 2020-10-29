using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ConferenceContract
    {
        public ConferenceContract()
        {
            companyContract = new HashSet<CompanyContract>();
            delegateServicePackDiscountForConferenceContract = new HashSet<DelegateServicePackDiscountForConferenceContract>();
        }

        [Key]
        public Guid ConferenceContractId { get; set; }

        [MaxLength(500)]
        public string ConferenceId { get; set; }

        [MaxLength(500)]
        public string CompanyId { get; set; }

        [MaxLength(500)]
        public string ComNameTranslation { get; set; }

        [MaxLength(150)]
        public string ContractNumber { get; set; }

        [MaxLength(150)]
        public string ContractYear { get; set; }

        [MaxLength(50)]
        public string ContractCode { get; set; }

        [MaxLength(50)]
        public string ContractStatusCode { get; set; }

        [MaxLength(50)]
        public string PaymentStatusCode { get; set; }

        [MaxLength(150)]
        public string ModifyPermission { get; set; }

        public bool? IsDelete { get; set; }

        public bool? IsSendEmail { get; set; }

        public bool? IsModify { get; set; }

        [MaxLength(500)]
        public string Owerid { get; set; }

        [MaxLength(50)]
        public string Ower { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }

        public virtual ICollection<CompanyContract> companyContract { get; set; }

        public virtual ICollection<DelegateServicePackDiscountForConferenceContract> delegateServicePackDiscountForConferenceContract { get; set; }
    }
}
