using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ParticipantTypeMap
    {
        [Key]
        public Guid ParticipantTypeMapID { get; set; }

        [MaxLength(50)]
        public Guid ParticipantTypeID { get; set; }

        [ForeignKey("ParticipantTypeID")]
        public virtual ParticipantType ParticipantType { get; set; }

        [MaxLength(50)]
        public Guid ParticipantID { get; set; }

        [ForeignKey("ParticipantID")]
        public virtual Participant Participant { get; set; }


    }
}
