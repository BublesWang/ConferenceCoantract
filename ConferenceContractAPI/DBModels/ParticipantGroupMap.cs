using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ParticipantGroupMap
    {
        [Key]
        public Guid ParticipantGroupMapID { get; set; }

        [MaxLength(50)]
        public Guid ParticipantGroupID { get; set; }

        [ForeignKey("ParticipantGroupID")]

        public virtual ParticipantGroup ParticipantGroup { get; set; }

        [MaxLength(50)]
        public Guid ParticipantID { get; set; }

        [ForeignKey("ParticipantID")]

        public virtual Participant Participant { get; set; }
    }
}
