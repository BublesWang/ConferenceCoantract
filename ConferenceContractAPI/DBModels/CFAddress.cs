using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CFAddress
    {
        public CFAddress()
        {
            Conference = new HashSet<Conference>();
             
        }

        [Key]
        public Guid CFAddressPK { get; set; }
        [MaxLength(2000)]
        public string Translation { get; set; }
        [MaxLength(100)]
        public string PostCode { get; set; }
        [MaxLength(250)]
        public string Country { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public virtual ICollection<Conference> Conference { get; set; }
    }
}
