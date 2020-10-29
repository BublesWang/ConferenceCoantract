using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class PersonContractActivityMap
    {
        [Key]
        public Guid MapId { get; set; }

        [MaxLength(150)]
        public string PersonContractId { get; set; }

        [MaxLength(150)]
        public string MemberPK { get; set; }

        [MaxLength(150)]
        public string ActivityId { get; set; }

        [MaxLength(1000)]
        public string ActivityName { get; set; }

        [MaxLength(500)]
        public string SessionConferenceID { get; set; }

        [MaxLength(1000)]
        public string SessionConferenceName { get; set; }

        public bool? IsConfirm { get; set; }

        public bool? IsCheck { get; set; }

        [MaxLength(100)]
        public string Year { get; set; }
    }
}
