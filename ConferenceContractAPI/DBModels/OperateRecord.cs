using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class OperateRecord
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string ContractNumber { get; set; }

        public string OperateContent { get; set; }

        [MaxLength(50)]
        public string OperateTime { get; set; }

        [MaxLength(50)]
        public string Operator { get; set; }
    }
}
