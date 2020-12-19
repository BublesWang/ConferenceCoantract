using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CFRoom
    {
        [Key]
        public Guid CFRoomPK { get; set; }

        [MaxLength(100)]
        public string RoomNameCN { get; set; }

        [MaxLength(100)]
        public string RoomNameEN { get; set; }

        [MaxLength(50)]
        public string RoomArea { get; set; }

        [MaxLength(100)]
        public string RoomNumber { get; set; }

        [MaxLength(100)]
        public string RoomType { get; set; }

        public int RoomPatacity { get; set; }

        [MaxLength(50)]
        public string HalfPrice { get; set; }

        [MaxLength(50)]
        public string Price { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }


        public virtual ICollection<CFSeat> CFSeat { get; set; }

    }
}
