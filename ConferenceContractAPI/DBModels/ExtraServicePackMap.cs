using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ExtraServicePackMap
    {
        [Key]
        public Guid MapId { get; set; }

        public Guid? ExtraServiceId { get; set; }

        public Guid? ServicePackId { get; set; }

        [ForeignKey("ExtraServiceId")]
        public virtual ExtraService extraService { get; set; }

        [ForeignKey("ServicePackId")]
        public virtual ServicePack servicePack { get; set; }
    }
}
