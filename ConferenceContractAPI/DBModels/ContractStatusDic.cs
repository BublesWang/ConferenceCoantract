using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ContractStatusDic
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(50)]
        public string StatusName { get; set; }

        [MaxLength(100)]
        public string StatusCode { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }
    }
}
