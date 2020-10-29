using ConferenceContractAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class CompanyServicePackVM
    {
        public CompanyServicePack CompanyServicePack { get; set; }

        public List<ServicePack> ServicePackList { get; set; }
    }
}
