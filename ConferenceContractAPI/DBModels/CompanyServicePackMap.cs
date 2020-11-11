using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CompanyServicePackMap
    {
        [Key]
        public Guid MapId { get; set; }

        public Guid? CompanyServicePackId { get; set; }

        public Guid? ServicePackId { get; set; }

        [ForeignKey("CompanyServicePackId")]
        public virtual CompanyServicePack companyServicePack { get; set; }

        [ForeignKey("ServicePackId")]
        public virtual ServicePack servicePack { get; set; }

        [MaxLength(150)]
        public string  SPCCode { get; set; }
        [MaxLength(150)]
        public string CSPCCode { get; set; }
        [MaxLength(150)]
        public string Year { get; set; }
    }
}
