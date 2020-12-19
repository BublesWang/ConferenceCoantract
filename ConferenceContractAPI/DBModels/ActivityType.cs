using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ActivityType
    {
        public ActivityType()
        {
            Activity = new HashSet<Activity>();
        }

        [Key]
        public Guid ActivityTypeID { get; set; }

        [MaxLength(2000)]
        public string Translation { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [MaxLength(50)]
        public string ActivityCode { get; set; }

        public virtual ICollection<Activity> Activity { get; set; }
    }
}
