using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CFSeat
    {
        [Key]
        public Guid CFSeatPK { get; set; }

        public Guid? CFRoomPK { get; set; }

        [ForeignKey("CFRoomPK")]
        public virtual CFRoom CFRoom { get; set; }

        public int? SeatNumber { get; set; }

        [MaxLength(50)]
        public string SeatName { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }
    }
}
