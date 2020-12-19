using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ParticipantConferenceMap
    {
        [Key]
        public Guid ParticipantConferenceMapID { get; set; }

        [MaxLength(50)]
        public Guid ParticipantID { get; set; }

        [ForeignKey("ParticipantID")]
        public virtual Participant Participant { get; set; }

        [MaxLength(50)]
        public string IMGSRC { get; set; }

        [MaxLength(50)]
        public string SessionConferenceID { get; set; }

        [MaxLength(200)]
        public string SessionCN { get; set; }

        [MaxLength(200)]
        public string SessionEN { get; set; }

        [MaxLength(2000)]
        public string SessionJP { get; set; }

        [MaxLength(200)]
        public string SpeechTimeCN { get; set; }

        [MaxLength(200)]
        public string SpeechTimeEN { get; set; }

        [MaxLength(2000)]
        public string SpeechTimeJP { get; set; }

        public bool HasPersonInfo { get; set; }

        [MaxLength(50)]
        public Guid ParticipantGroupID { get; set; }

        [MaxLength(50)]
        public string AbstractDraftID { get; set; }

        [ForeignKey("ParticipantGroupID")]
        public virtual ParticipantGroup ParticipantGroup { get; set; }

        [MaxLength(50)]
        public string Sort { get; set; }
        
    }
}
