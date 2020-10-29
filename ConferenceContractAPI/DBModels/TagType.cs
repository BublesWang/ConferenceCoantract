using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class TagType
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string NameTranslation { get; set; }

        [MaxLength(500)]
        public string Code { get; set; }
    }
}
