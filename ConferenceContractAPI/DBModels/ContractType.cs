using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ContractType
    {
        public ContractType()
        {
            companyServicePack = new HashSet<CompanyServicePack>();
        }

        [Key]
        public Guid ContractTypeId { get; set; }

        public int? Sort { get; set; }

        [MaxLength(500)]
        public string Translation { get; set; }

        [MaxLength(50)]
        public string CTypeCode { get; set; }

        public bool IsSpeaker { get; set; }

        public bool IsGive { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }

        public virtual ICollection<CompanyServicePack> companyServicePack { get; set; }
    }
}
