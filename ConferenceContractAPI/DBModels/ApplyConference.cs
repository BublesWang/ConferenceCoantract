using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ApplyConference
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(150)]
        public string PersonContractId { get; set; }

        [MaxLength(150)]
        public string CompanyId { get; set; }

        [MaxLength(150)]
        public string MemberPK { get; set; }

        [MaxLength(150)]
        public string SessionConferenceId { get; set; }

        public bool? IsConfirm { get; set; }

        public bool? IsParticularConf { get; set; }

        [MaxLength(40000)]
        public string TagTypeCodes { get; set; }

        [MaxLength(40000)]
        public string RemarkTranslation { get; set; }

        [MaxLength(150)]
        public string Year { get; set; }

        [MaxLength(500)]
        public string Owerid { get; set; }
    }
}
