using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ExtraService
    {
        public ExtraService()
        {
            extraServicePackMap = new HashSet<ExtraServicePackMap>();
        }

        [Key]
        public Guid ExtraServiceId { get; set; }

        //public Guid? PersonContractId { get; set; }

        //[ForeignKey("PersonContractId")]
        //public virtual PersonContract personContract { get; set; }

        [MaxLength(150)]
        public string ExtraContractNumber { get; set; }

        public Guid? MemberPK { get; set; }

        [MaxLength(500)]
        public string MemTranslation { get; set; }

        public bool? IsDelete { get; set; }

        [MaxLength(500)]
        public string Owerid { get; set; }

        [MaxLength(50)]
        public string Ower { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }

        public virtual ICollection<ExtraServicePackMap> extraServicePackMap { get; set; }
    }
}
