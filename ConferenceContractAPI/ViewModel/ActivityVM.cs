using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class ActivityVM
    {
        public string SessionIDs { get; set; }
        public string ActivityId { get; set; }

        public string ActivityName { get; set; }

        public string SessionConferenceID { get; set; }

        public string SessionConferenceName { get; set; }

        public int Sort { get; set; }
    }
}
