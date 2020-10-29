using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class CompanyServicePackMapVM
    {
        public Guid MapId { get; set; }

        public Guid CompanyServicePackId { get; set; }

        public Guid ServicePackId { get; set; }

        public string ConferenceId { get; set; }

        public string ConferenceName { get; set; }
    }
}
