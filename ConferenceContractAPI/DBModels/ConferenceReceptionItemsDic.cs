using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ConferenceReceptionItemsDic
    {

        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string ItemName { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }
    }
}
