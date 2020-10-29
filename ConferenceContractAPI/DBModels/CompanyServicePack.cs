using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CompanyServicePack
    {
        public CompanyServicePack()
        {
            companyServicePackMap = new HashSet<CompanyServicePackMap>();
        }

        [Key]
        public Guid CompanyServicePackId { get; set; }

        public Guid? ContractTypeId { get; set; }

        [MaxLength(50)]
        public string CTypeCode { get; set; }

        [ForeignKey("ContractTypeId")]
        public virtual ContractType contractType { get; set; }

        [MaxLength(500)]
        public string Translation { get; set; }

        public int? Sort { get; set; }

        [MaxLength(500)]
        public string ConferenceId { get; set; }

        [MaxLength(500)]
        public string ConferenceName { get; set; }

        [MaxLength(50)]
        public string PriceRMB { get; set; }

        [MaxLength(50)]
        public string PriceUSD { get; set; }

        public bool? IsShownOnFront { get; set; }

        [MaxLength(1000)]
        public string RemarkTranslation { get; set; }

        [MaxLength(500)]
        public string RemarkCode { get; set; }
        
        public bool IsSpeaker { get; set; }

        public bool? IsDelete { get; set; }

        public bool IsGive { get; set; }

        [MaxLength(150)]
        public string Year { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }

        public virtual ICollection<CompanyServicePackMap> companyServicePackMap { get; set; }
    }
}
