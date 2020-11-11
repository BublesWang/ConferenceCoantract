using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class PackDescriptionDic
    {
        [Key]
        public Guid PackDescriptionDicId { get; set; }

        [MaxLength(150)]
        public string NameTranslation { get; set; }

        [MaxLength(50)]
        public string Code { get; set; }
    }
}
