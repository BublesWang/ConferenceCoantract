using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Policy
    {
        [Key]
        public Guid PolicyID { get; set; }

        [MaxLength(1000)]
        public string CompanyNameTranslation { get; set; }

        [MaxLength(50)]
        public string CompanyID { get; set; }

        [MaxLength(200)]
        public string Country { get; set; }

        [MaxLength(50)]
        public string CountryID { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(200)]
        public string PostCode { get; set; }

        [MaxLength(200)]
        public string ContactName { get; set; }

        [MaxLength(50)]
        public string EMail { get; set; }

        [MaxLength(50)]
        public string Tel { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }

        [MaxLength(500)]
        public string WebSite { get; set; }

        [MaxLength(50)]
        public bool IsDelete { get; set; }

        [MaxLength(50)]
        public string AdminName { get; set; }

        [MaxLength(50)]
        public string AdminID { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        public DateTime? DeleteOn { get; set; }

        [MaxLength(50)]
        public string DeleteBy { get; set; }

        [MaxLength(50)]
        public string Year { get; set; }

        [MaxLength(50)]
        public string Score { get; set; }

        [MaxLength(50)]
        public string SourceName { get; set; }
    }
}
