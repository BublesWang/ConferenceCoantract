using ConferenceContractAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class ExtraServiceVM
    {
        public ExtraService ExtraService { get; set; }

        public List<ServicePack> ServicePackList { get; set; }
    }
}
