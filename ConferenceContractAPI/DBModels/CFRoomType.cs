using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CFRoomType
    {
        [Key]
        public Guid CFRoomTypePK { get; set; }

        [MaxLength(50)]
        public string CFRoomTypeName { get; set; }

        [MaxLength(300)]
        public string Remark { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }
}
