using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ServicePack
    {
        public ServicePack()
        {
            companyServicePackMap = new HashSet<CompanyServicePackMap>();
            extraServicePackMap = new HashSet<ExtraServicePackMap>();
            servicePackActivityMap = new HashSet<ServicePackActivityMap>();
        }

        [Key]
        public Guid ServicePackId { get; set; }

        [MaxLength(500)]
        public string ConferenceId { get; set; }

        [MaxLength(500)]
        public string ConferenceName { get; set; }

        [MaxLength(500)]
        public string SessionConferenceId { get; set; }

        [MaxLength(500)]
        public string SessionConferenceName { get; set; }

        [MaxLength(500)]
        public string SessionAddress { get; set; }

        [MaxLength(500)]
        public string ThirdSessionConferenceId { get; set; }

        [MaxLength(500)]
        public string ThirdSessionConferenceName { get; set; }

        [MaxLength(500)]
        public string SessionDate { get; set; }

        [MaxLength(500)]
        public string SessionStartTime { get; set; }

        [MaxLength(500)]
        public string Translation { get; set; }

        [MaxLength(50)]
        public string PriceRMB { get; set; }

        [MaxLength(50)]
        public string PriceUSD { get; set; }
        [MaxLength(50)]
        public string PriceJP { get; set; }
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(150)]
        public string Year { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        public bool IsDelete { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }

        public virtual ICollection<CompanyServicePackMap> companyServicePackMap { get; set; }

        public virtual ICollection<ExtraServicePackMap> extraServicePackMap { get; set; }

        public virtual ICollection<ServicePackActivityMap> servicePackActivityMap { get; set; }
    }
}
