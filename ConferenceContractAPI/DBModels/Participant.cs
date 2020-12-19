using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Participant
    {
        public Participant() {
            TalkParticipantMap = new HashSet<TalkParticipantMap>();

            ParticipantGroupMap = new HashSet<ParticipantGroupMap>();

        }

        [Key]
        public Guid ParticipantID { get; set; }

        [MaxLength(1000)]
        public string ParticipantNameTranslation { get; set; }

        [MaxLength(100)]
        public string IMGSRC { get; set; }

        [MaxLength(1000)]
        public string CompanyTranslation { get; set; }

        [MaxLength(1000)]
        public string JobTranslation { get; set; }

        [MaxLength(1000)]
        public string CountryTranslation { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Mobile { get; set; }

        [MaxLength(40000)]
        public string IntroduceTranslation { get; set; }
        
        public int Sort { get; set; }

        [MaxLength(50)]
        public string PersonContractID { get; set; }

        [MaxLength(50)]
        public string PerContractNumber { get; set; }

        public bool IsDelete { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [MaxLength(50)]
        public string Owerid { get; set; }

        [MaxLength(50)]
        public string Ower { get; set; }

        [MaxLength(500)]
        public string AppellationTranslation { get; set; }

        [MaxLength(50)]
        public string CompanyId { get; set; }

        [MaxLength(50)]
        public bool ConfirmPPT { get; set; }

        [MaxLength(50)]
        public string MemberPK { get; set; }

        public int ShowOnFont { get; set; }

        [MaxLength(50)]
        public string Year { get; set; }

        public virtual ICollection<TalkParticipantMap> TalkParticipantMap { get; set; }
        public virtual ICollection<ParticipantGroupMap> ParticipantGroupMap { get; set; }

       




    }
}
