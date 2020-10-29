using ConferenceContractAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class ServicePackVM
    {
        public ServicePack ServicePack { get; set; }

        public List<ActivityVM> ActivityList { get; set; }
    }
}
