using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Conference
    {
        public Conference()
        {
            Activity = new HashSet<Activity>();

            OrganizerLevel = new HashSet<OrganizerLevel>();
        }

        [Key]
        public Guid ConferenceID { get; set; }

        [MaxLength(50)]
        public string ParentID { get; set; }

        [MaxLength(2000)]
        public string Translation { get; set; }

        public Guid CFAddressPK { get; set; }

        [ForeignKey("CFAddressPK")]
        public virtual CFAddress CFAddress  { get; set; }

        [MaxLength(50)]
        public string StartDateTime { get; set; }

        [MaxLength(50)]
        public string StartDate { get; set; }

        [MaxLength(200)]
        public string Abbreviation { get; set; }

        [MaxLength(40000)]
        public string ConferenceIntroduce { get; set; }

        [MaxLength(10)]
        public string Year { get; set; }

        [MaxLength(50)]
        public string Sort { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public int Level { get; set; }

        [MaxLength(200)]
        public string TimeRange { get; set; }

        public bool ShowOnFront { get; set; }

        public bool ApplySearch { get; set; }

        public virtual ICollection<Activity> Activity { get; set; }

        public virtual ICollection<OrganizerLevel> OrganizerLevel { get; set; }
    }
}
