using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class PotentialCustomers
    {
        [Key]
        public Guid? PotentialCustomersPK { get; set; }
        [MaxLength(300)]
        public string CompanyPK { get; set; }
        [MaxLength(300)]
        public string CompanyNameTranslation { get; set; }
        [MaxLength(300)]
        public string ContactName { get; set; }
        [MaxLength(300)]
        public string EMail { get; set; }
        [MaxLength(300)]
        public string Tel { get; set; }
        [MaxLength(300)]
        public string Mobile { get; set; }
        [MaxLength(300)]
        public string Position { get; set; }
        [MaxLength(300)]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Year { get; set; }
        [MaxLength(300)]
        public string CountryPK { get; set; }
        [MaxLength(300)]
        public string Country { get; set; }
        [MaxLength(500)]
        public string Address { get; set; }
        [MaxLength(300)]
        public string PostCode { get; set; }
        [MaxLength(300)]
        public string SourceCode { get; set; }
        [MaxLength(300)]
        public string Score { get; set; }

        public bool IsOldCustomer { get; set; }
        public DateTime? CreatedOn { get; set; }
        [MaxLength(50)]
        public string CreatedBy { get; set; }
        public DateTime? ModefieldOn { get; set; }
        [MaxLength(50)]
        public string ModefieldBy { get; set; }

    }
}
