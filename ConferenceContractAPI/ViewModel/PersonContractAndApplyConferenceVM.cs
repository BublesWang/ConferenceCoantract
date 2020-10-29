using ConferenceContractAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class PersonContractAndApplyConferenceVM
    {
        public PersonContract PersonContract { get; set; }

        public CompanyContract CompanyContract { get; set; }

        public string SessionConferenceId { get; set; }

    }
}
