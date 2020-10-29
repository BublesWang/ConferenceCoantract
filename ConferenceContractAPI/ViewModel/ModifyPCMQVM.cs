using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class ModifyPCMQVM
    {
        public string OldMemberPK { get; set; }
        public string NewMemberPK { get; set; }
        public string OldTypeCode { get; set; }
        public string NewTypeCode { get; set; }
        public string ContractId { get; set; }
        public string ContractNumber { get; set; }
        public string Year { get; set; }
        public string Ower { get; set; }
        public string Owerid { get; set; }
    }
}
