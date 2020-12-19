using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Organizer
    {
        [Key]
        public Guid OrganizerID { get; set; }

        public Guid? OrganizerLevelID { get; set; }

        [ForeignKey("OrganizerLevelID")]

        public virtual OrganizerLevel OrganizerLevel { get; set; }

        [MaxLength(2000)]
        public string Translation { get; set; }

        [MaxLength(50)]
        public string Sort { get; set; }

        [MaxLength(200)]
        public string IMGSRC { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [MaxLength(200)]
        public string CompanyURL { get; set; }
    }
}
