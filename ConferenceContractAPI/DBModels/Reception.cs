using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Reception
    {
        [Key]
        public Guid ReceptionID { get; set; }

        [MaxLength(50)]
        public string MemberPK { get; set; }

        [MaxLength(50)]
        public bool IsReception { get; set; }

        [MaxLength(50)]
        public bool IsArrange { get; set; }

        [MaxLength(100)]
        public string ArriveDate { get; set; }

        [MaxLength(100)]
        public string LeaveDate { get; set; }

        [MaxLength(200)]
        public string Hotel { get; set; }

        [MaxLength(200)]
        public string Receptioner { get; set; }
    }
}
