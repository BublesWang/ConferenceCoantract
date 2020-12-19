using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class AbstractDraft
    {
        public AbstractDraft()
        {
           
        }

        [Key]
        public Guid AbstractDraftID { get; set; }

        [MaxLength(50)]
        public string CFTopicPK { get; set; } 

        [MaxLength(50)]
        public string MemberId { get; set; }

        [MaxLength(200)]
        public string MemberName { get; set; }

        [MaxLength(500)]
        public string MemberTitle { get; set; }

        [MaxLength(50)]
        public string Tel { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(500)]
        public string CompanyName { get; set; }

        [MaxLength(2000)]
        public string DraftNameTranslation { get; set; }

        [MaxLength(40000)]
        public string RemarkTranslation { get; set; }

        [MaxLength(200)]
        public string Source { get; set; }

        [MaxLength(1000)]
        public string ExtendedDocument { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [MaxLength(50)]
        public string ParticipantID { get; set; }

        [MaxLength(40000)]
        public string DraftContentTranslation { get; set; }

        [MaxLength(100)]
        public string perContractNumber { get; set; }

        [MaxLength(50)]

        public Guid AbstractParticipantID { get; set; }

        [MaxLength(200)]
        public string AbstractDraftType { get; set; }

        [ForeignKey("AbstractParticipantID")]
        public virtual AbstractParticipant AbstractParticipant { get; set; }

        [MaxLength(500)]
        public string ConferenceName { get; set; }

        [MaxLength(50)]
        public string Year { get; set; }

        [MaxLength(50)]
        public string RuleNumber { get; set; }

        [MaxLength(2000)]
        public string OtherName { get; set; }

        public bool IsDelete { get; set; }

        [MaxLength(50)]
        public string TopicCategoryID { get; set; }

    }
}
