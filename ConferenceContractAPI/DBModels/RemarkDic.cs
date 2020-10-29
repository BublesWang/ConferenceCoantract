using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class RemarkDic
    {
        [Key]
        public Guid Id { get; set; }

        public string ContentCn { get; set; }

        public string ContentEn { get; set; }

        public string ContentJp { get; set; }

        [MaxLength(500)]
        public string ContentCode { get; set; }
    }
}
