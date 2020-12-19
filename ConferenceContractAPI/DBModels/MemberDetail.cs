using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.DBModels
{
    public class MemberDetail
    {
        public MemberDetail()
        {

        }

        [Key]
        public Guid? MemberPK { get; set; }

        [MaxLength(150)]
        public string MemProTitle { get; set; }

        [MaxLength(200)]
        public string MemFax { get; set; }

        [MaxLength(500)]
        public string MemWebSite { get; set; }

        [MaxLength(50)]
        public string MemLicenseCountry { get; set; }

        [MaxLength(500)]
        public string MemPermanentResidence { get; set; }

        [MaxLength(1000)]
        public string IdNumberPicFront { get; set; }

        [MaxLength(1000)]
        public string IdNumberPicBehind { get; set; }

        [MaxLength(250)]
        public string MemAddressDetail { get; set; }

        [MaxLength(4000)]
        public string MemHeadPic { get; set; }

        [MaxLength(150)]
        public string AssistantName { get; set; }

        [MaxLength(150)]
        public string AssistantPosition { get; set; }

        [MaxLength(200)]
        public string AssistantMobile { get; set; }

        [MaxLength(50)]
        public string AssistantEmail { get; set; }
        public bool? IsMediaConference { get; set; }

        [MaxLength(500)]
        public string PersonalIntro { get; set; }

        [MaxLength(500)]
        public string MediaIntro { get; set; }

        [MaxLength(500)]
        public string JoinedActivites { get; set; }

        [MaxLength(500)]
        public string MediaName { get; set; }

        public int ModifyMemNameCnCount { get; set; }

        public int ModifyMemNameEnCount { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

    }
}
