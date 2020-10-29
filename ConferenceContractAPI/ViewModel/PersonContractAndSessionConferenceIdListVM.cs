using ConferenceContractAPI.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.ViewModel
{
    public class PersonContractAndSessionConferenceIdListVM
    {
        public PersonContract PersonContract { get; set; }

        public CompanyContract CompanyContract { get; set; }

        public List<string> SessionConferenceIds { get; set; }

        public string CompanyServicePackName { get; set; }

    }
}
