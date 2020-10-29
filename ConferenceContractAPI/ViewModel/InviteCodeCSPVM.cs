using ConferenceContractAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class InviteCodeCSPVM
    {
        public InviteCode inviteCode { get; set; }
        public CompanyServicePack companyServicePack { get; set; }
        public int InviteRecordCount { get; set; }
    }
}
