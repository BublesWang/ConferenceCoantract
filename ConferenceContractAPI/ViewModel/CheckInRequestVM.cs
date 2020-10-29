using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class CheckInRequestVM
    {
        public List<string> Ids { get; set; }
        public bool IsCheckIn { get; set; }
    }
}
