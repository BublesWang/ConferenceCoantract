using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ParticipantGroup
    {
        public ParticipantGroup()
        {
            ParticipantGroupMap = new HashSet<ParticipantGroupMap>();

            ParticipantConferenceMap = new HashSet<ParticipantConferenceMap>();
        }

        [Key]
        public Guid ParticipantGroupID { get; set; }

        [MaxLength(50)]
        public string ConferenceID { get; set; }

        [MaxLength(200)]
        public string ConferenceName { get; set; }

        [MaxLength(4000)]
        public string ParticipantGroupNameTranslation { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<ParticipantGroupMap> ParticipantGroupMap { get; set; }

        public virtual ICollection<ParticipantConferenceMap> ParticipantConferenceMap { get; set; }
    }
}
