using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class AbstractParticipant
    {
        public AbstractParticipant()
        {
            AbstractDraft = new HashSet<AbstractDraft>();
        }

        [Key]
        public Guid AbstractParticipantID { get; set; }

        [MaxLength(1000)]
        public string NameTranslation { get; set; }

        [MaxLength(200)]
        public string IMGSRC { get; set; }

        [MaxLength(100)]
        public string JobTitleTranslation { get; set; }

        [MaxLength(40000)]
        public string JobTranslation { get; set; }

        [MaxLength(500)]
        public string CompanyTranslation { get; set; }

        [MaxLength(200)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }

        [MaxLength(50)]
        public string Tel { get; set; }

        [MaxLength(50)]
        public string CountryTranslantion { get; set; }

        [MaxLength(100)]
        public string PostCodes { get; set; }

        [MaxLength(40000)]
        public string IntroduceTranslation { get; set; }

        [MaxLength(100)]
        public string Language { get; set; }

        [MaxLength(100)]
        public string ContactsName { get; set; }

        [MaxLength(200)]
        public string ContactsJob { get; set; }

        [MaxLength(200)]
        public string ContactsCompany { get; set; }

        [MaxLength(100)]
        public string ContactsPostCode { get; set; }

        [MaxLength(100)]
        public string ContactsTel { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public bool IsDelete { get; set; }

        public virtual ICollection<AbstractDraft> AbstractDraft { get; set; }
    }
}
