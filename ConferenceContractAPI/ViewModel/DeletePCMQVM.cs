using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class DeletePCMQVM
    {
        public string MemberPK { get; set; }
        public string ContractId { get; set; }
        public string ContractNumber { get; set; }
        public string Year { get; set; }
        public string TypeCode { get; set; }
    }
}
