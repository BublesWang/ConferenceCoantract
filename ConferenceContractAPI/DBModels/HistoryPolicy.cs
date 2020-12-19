using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class HistoryPolicy
    {
        [Key]
        public Guid HistoryPolicyID { get; set; }

        [MaxLength(50)]
        public string PolicyID { get; set; }

        [MaxLength(1000)]
        public string CompanyNameTranslation { get; set; }

        [MaxLength(200)]
        public string Country { get; set; }

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
        public string OwnerName { get; set; }

        [MaxLength(100)]
        public string OwnerID { get; set; }

        [MaxLength(50)]
        public string Operation { get; set; }

        [MaxLength(50)]
        public string Operator { get; set; }

        [MaxLength(50)]
        public string OperationTime { get; set; }

        [MaxLength(50)]
        public string Year { get; set; }

        [MaxLength(50)]
        public string Score { get; set; }
    }
}
