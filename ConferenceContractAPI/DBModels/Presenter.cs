using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ConferenceContractAPI.DBModels
{
    public class Presenter
    {
        [Key]
        public Guid PresenterID { get; set; }

        public Guid? ConferenceID { get; set; }

        [ForeignKey("ConferenceID")]
        public virtual Conference Conference { get; set; }

        [MaxLength(1000)]
        public string PresenterNameTranslation { get; set; }

        [MaxLength(1000)]

        public string PresenterTypeTranslation { get; set; }

        [MaxLength(1000)]
        public string JobTitleTranslation { get; set; }

        [MaxLength(1000)]
        public string AppellationTranslation { get; set; }

        [MaxLength(1000)]

        public string CountryTranslation { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

        [MaxLength(50)]
        public string EMail { get; set; }

        [MaxLength(50)]
        public string Mobile { get; set; }
    }
}
