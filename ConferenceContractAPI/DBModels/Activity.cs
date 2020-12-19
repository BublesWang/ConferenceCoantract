using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Activity
    {
        [Key]
        public Guid ActivityID { get; set; }

        [MaxLength(100)]
        public Guid? ConferenceID { get; set; }

        [ForeignKey("ConferenceID")]
        public virtual Conference Conference { get; set; }

        [MaxLength(100)]
        public Guid? ActivityTypeID { get; set; }

        [ForeignKey("ActivityTypeID")]
        public virtual ActivityType ActivityType { get; set; }

        [MaxLength(100)]
        public string TimeLength { get; set; }
        
        public int Sort { get; set; }

        [MaxLength(100)]
        public string StartDate { get; set; }

        [MaxLength(200)]
        public string StartTime { get; set; }

        [MaxLength(200)]
        public string EndTime { get; set; }

        [MaxLength(2000)]
        public string Translation { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }


    }
}
