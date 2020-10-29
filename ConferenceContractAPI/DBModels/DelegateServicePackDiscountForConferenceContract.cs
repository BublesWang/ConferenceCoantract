using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class DelegateServicePackDiscountForConferenceContract
    {
        [Key]
        public Guid DiscountId { get; set; }

        public Guid? ConferenceContractId { get; set; }

        [ForeignKey("ConferenceContractId")]
        public virtual ConferenceContract conferenceContract { get; set; }

        [MaxLength(50)]
        public string PriceAfterDiscountRMB { get; set; }

        [MaxLength(50)]
        public string PriceAfterDiscountUSD { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }
    }
}
