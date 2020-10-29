using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class CreatePCMQVM
    {
        public string Ower { get; set; }
        public string Owerid { get; set; }
        public string MemberPK { get; set; }
        public string ContractId { get; set; }
        public string ContractNumber { get; set; }
        public string Year { get; set; }
        public string TypeCode { get; set; }
    }
}
