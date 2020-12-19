using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.DBModels
{
    public class Company
    {
        public Company()
        {
           
        }

        [Key]
        public Guid CompanyPK { get; set; }
        [Required]
        public Guid? CompanyTypePK { get; set; }
        [Required]
        [ForeignKey("CompanyTypePK")]
        

        //public Guid? IndustryTypePK { get; set; }

        //[ForeignKey("IndustryTypePK")]
        //public virtual IndustryType industryType { get; set; }
    
        [MaxLength(500)]
        public string IndustryTypeCode { get; set; }
        [Required]
        [MaxLength(500)]
        public string MemberPK { get; set; }

        //[ForeignKey("MemberPK")]
        //public virtual Member member { get; set; }
        [Required]
        [MaxLength(150)]
        public string ComNameCn { get; set; }
        [Required]
        [MaxLength(150)]
        public string ComNameEn { get; set; }
        [Required]
        [MaxLength(250)]
        public string ComAreaDetail { get; set; }
        [Required]
        [MaxLength(500)]
        public string ComContactEmail { get; set; }

        [MaxLength(200)]
        public string ComPostCode { get; set; }

        [MaxLength(200)]
        public string ComFax { get; set; }
        [Required]
        [MaxLength(200)]
        public string ComTel { get; set; }
        [Required]
        [MaxLength(150)]
        public string ComWebSite { get; set; }
        [Required]
        [MaxLength(150)]
        public string ComClient { get; set; }
        [Required]
        [MaxLength(50)]
        public string ComNature { get; set; }
        [Required]
        [MaxLength(50)]
        public string ComLang { get; set; }
        [Required]
        [MaxLength(500)]
        public string ComRemark { get; set; }
        [Required]
        public bool? ComIsDelete { get; set; }
        [Required]
        public bool? ComIsVerify { get; set; }
        [Required]
        public bool? ComIsWebSite { get; set; }
        [Required]
        [MaxLength(250)]
        public string ComAddress { get; set; }
        [Required]
        [MaxLength(250)]
        public string ComContract { get; set; }
        [Required]
        [MaxLength(250)]
        public string ComTitle { get; set; }
        [Required]
        [MaxLength(250)]
        public string ComShortName { get; set; }
        [Required]
        [MaxLength(1000)]
        public string ComContractStyle { get; set; }
        [Required]
        [MaxLength(500)]
        public string Owerid { get; set; }
        [Required]
        [MaxLength(50)]
        public string Ower { get; set; }
        [Required]
        public DateTime? CreatedOn { get; set; }
        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }

      
        //UniqueCode需要自动生成，每个公司唯一不变
        [Required]
        public string UniqueCode { get; set; }

    }
}
