using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class OrganizerLevel
    {
        [Key]
        public Guid OrganizerLevelID { get; set; }

        public Guid? ConferenceId { get; set; }

        [ForeignKey("ConferenceId")]
        public virtual Conference Conference { get; set; }

        [MaxLength(2000)]
        public string Translation { get; set; }

        public bool IsHide { get; set; }

        [MaxLength(50)]
        public string Sort { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }
}
