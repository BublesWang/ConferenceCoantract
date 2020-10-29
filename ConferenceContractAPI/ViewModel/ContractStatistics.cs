using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class ContractStatistics
    {
        public string CompanyServicePackId { get; set; }
        public string CompanyServicePackName { get; set; }
        public string PersonCount { get; set; }
        public string MaxContractNumberSum { get; set; }
    }
}
