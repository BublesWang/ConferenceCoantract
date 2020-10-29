using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class DelegateServicePackDiscount
    {
        [Key]
        public Guid DiscountId { get; set; }

        public Guid? ContractId { get; set; }

        [ForeignKey("ContractId")]
        public virtual CompanyContract companyContract { get; set; }

        [MaxLength(50)]
        public string PriceAfterDiscountRMB { get; set; }

        [MaxLength(50)]
        public string PriceAfterDiscountUSD { get; set; }

        [MaxLength(50)]
        public string Year { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }
    }
}
