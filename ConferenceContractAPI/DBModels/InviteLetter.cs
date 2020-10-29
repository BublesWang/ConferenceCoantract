using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class InviteLetter
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string Company { get; set; }

        [MaxLength(500)]
        public string EHall { get; set; }

        [MaxLength(500)]
        public string ENo { get; set; }

        [MaxLength(1000)]
        public string Profile { get; set; }

        [MaxLength(1000)]
        public string EAcitvity { get; set; }

        [MaxLength(50)]
        public string Language { get; set; }

        public DateTime? CreatedOn { get; set; }
    }
}
