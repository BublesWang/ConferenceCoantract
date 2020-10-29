using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class ConferenceOnsite
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(150)]
        public string ContractNumber { get; set; }

        [MaxLength(150)]
        public string UserName { get; set; }

        [MaxLength(150)]
        public string CompanyName { get; set; }

        [MaxLength(1500)]
        public string CompanyServicePackId { get; set; }

        [MaxLength(1500)]
        public string CompanyServicePackName { get; set; }

        [MaxLength(150)]
        public string Currency { get; set; }

        [MaxLength(150)]
        public string PayType { get; set; }

        [MaxLength(150)]
        public string Credited { get; set; }

        [MaxLength(150)]
        public string AddDate { get; set; }

        public decimal Cost { get; set; }

        public string Remark { get; set; }

        [MaxLength(50)]
        public string ContractYear { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }
    }
}
