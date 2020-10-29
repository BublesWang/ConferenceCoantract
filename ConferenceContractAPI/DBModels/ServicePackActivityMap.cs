using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ServicePackActivityMap
    {
        [Key]
        public Guid MapId { get; set; }

        public Guid? ServicePackId { get; set; }

        [MaxLength(10000)]
        public string SessionIDs { get; set; }

        [MaxLength(500)]
        public string ActivityId { get; set; }

        [ForeignKey("ServicePackId")]
        public virtual ServicePack servicePack { get; set; }

        [MaxLength(1000)]
        public string ActivityName { get; set; }

        [MaxLength(500)]
        public string SessionConferenceID { get; set; }

        [MaxLength(1000)]
        public string SessionConferenceName { get; set; }

        public int Sort { get; set; }

    }
}
