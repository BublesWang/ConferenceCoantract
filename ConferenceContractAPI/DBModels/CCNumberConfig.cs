using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class CCNumberConfig
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(500)]
        public string ConferenceId { get; set; }

        [MaxLength(500)]
        public string ConferenceName { get; set; }

        [MaxLength(50)]
        public string Prefix { get; set; }

        [MaxLength(50)]
        public string Year { get; set; }

        [MaxLength(50)]
        public string CNano { get; set; }

        public int? Count { get; set; }

        public int? Status { get; set; }

        public bool? IsDelete { get; set; }

    }
}
