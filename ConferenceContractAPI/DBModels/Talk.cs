using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Talk
    {
        [Key]
        public Guid TalkID { get; set; }

        [MaxLength(50)]
        public Guid? ActivityID { get; set; }

        [ForeignKey("ActivityID")]

        public virtual Activity Activity { get; set; }

        [MaxLength(50)]
        public Guid? TalkTypeID { get; set; }

        [ForeignKey("TalkTypeID")]
        public virtual TalkType TalkType { get; set; }

        [MaxLength(50)]
        public string CFTopicPK { get; set; }

        [MaxLength(100)]
        public string TimeLength { get; set; }
         
        public int Sort { get; set; }

        [MaxLength(100)]
        public string StartDate { get; set; }

        [MaxLength(2000)]
        public string Translation { get; set; }

        [MaxLength(200)]
        public string StartTime { get; set; }

        [MaxLength(200)]
        public string EndTime { get; set; }

        [MaxLength(500)]
        public string CFTopicName { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

    }
}
