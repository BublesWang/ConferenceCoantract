using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ScheduleList
    {
        public Guid ScheduelListId { get; set; }
        public Guid? CompanyServicePackId { get; set; }
        public Guid? ContractTypeId { get; set; }
        public string ConferenceId { get; set; }
        public string ConferenceName { get; set; }
        public string CTypeCode { get; set; }
        public Int32? Sort { get; set; }
        public string CSPTranslation { get; set; }
        public string CSPPriceRMB { get; set; }
        public string CSPPriceUSD { get; set; }
        public string CSPPriceJP { get; set; }
        public bool IsShownOnFront { get; set; }
        public string RemarkTranslation { get; set; }
        public string RemarkCode { get; set; }
        public bool? CSPIsDelete { get; set; }
        public bool? IsSpeaker { get; set; }
        public bool? IsGive { get; set; }
        public Guid? ServicePackId { get; set; }
        public string SessionConferenceId { get; set; }
        public string SessionConferenceName { get; set; }
        public string ThirdSessionConferenceId { get; set; }
        public string ThirdSessionConferenceName { get; set; }
        public string SessionDate { get; set; }
        public string SessionStartTime { get; set; }
        public string SessionAddress { get; set; }
        public string SPTranslation { get; set; }
        public string SPPriceRMB { get; set; }
        public string SPPriceUSD { get; set; }
        public string SPPriceJP { get; set; }
        public bool? SPIsDelete { get; set; }
        public string SPCode { get; set; }
        public Guid? SPAMapId { get; set; }
        public string SessionIDs { get; set; }
        public string ActivityName { get; set; }
        public Int32? SPASort { get; set; }
        public string ActivityId { get; set; }
        public Guid? CSPMapId { get; set; }
        public string CSPCode { get; set; }
        public string MyProperty { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime? ModefieldOn { get; set; }
        [MaxLength(50)]
        public string ModefieldBy { get; set; }

    }
}
