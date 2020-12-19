using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MemberAPI.DBModels
{
    public class Member
    {
        public Member()
        {
            //company = new HashSet<Company>();
        }

        [Key]
        public Guid MemberPK { get; set; }

        public Guid? SourcePK { get; set; }



        [MaxLength(500)]
        public string CompanyPK { get; set; }

        [MaxLength(150)]
        public string MemNameCn { get; set; }

        [MaxLength(150)]
        public string MemNameEn { get; set; }

        [MaxLength(150)]
        public string MemCountryNameCn { get; set; }

        [MaxLength(150)]
        public string MemCountryNameEn { get; set; }

        [MaxLength(500)]
        public string MemCompany { get; set; }

        [MaxLength(500)]
        public string MemEmail { get; set; }

        [MaxLength(500)]
        public string MemPassword { get; set; }

        [MaxLength(50)]
        public string MemType { get; set; }

        [MaxLength(50)]
        public string MemKindType { get; set; }

        [MaxLength(50)]
        public string MemPurposeCode { get; set; }

        [MaxLength(50)]
        public string MemMarktingCode { get; set; }

        [MaxLength(20)]
        public string MemGender { get; set; }

        [MaxLength(500)]
        public string MemDepartment { get; set; }

        [MaxLength(50)]
        public string MemTitle { get; set; }

        [MaxLength(150)]
        public string MemPosition { get; set; }


        [MaxLength(200)]
        public string MemQQ { get; set; }


        [MaxLength(200)]
        public string MemMSN { get; set; }

        [MaxLength(200)]
        public string MemMobile { get; set; }

        [MaxLength(200)]
        public string MemTel { get; set; }

        [MaxLength(200)]
        public string MemPostCode { get; set; }

        [MaxLength(50)]
        public string MemLicenseType { get; set; }

        [MaxLength(500)]
        public string MemLicenseNumber { get; set; }

        [MaxLength(50)]
        public string LicenseVerifyState { get; set; }

        public DateTime MemLastLoginTime { get; set; }

        [MaxLength(250)]
        public string MemAddress { get; set; }


        public bool? MemIsDelete { get; set; }

        public bool? MemIsActivation { get; set; }

        public bool? IsEmailActivation { get; set; }

        public bool? IsMobileActivation { get; set; }

        [MaxLength(500)]
        public string Owerid { get; set; }

        [MaxLength(50)]
        public string Ower { get; set; }

        [MaxLength(500)]
        public string SourceCompany { get; set; }

        [MaxLength(500)]
        public string WeChatId { get; set; }

        [MaxLength(500)]
        public string WeChatName { get; set; }

        public bool? IsWechatActivation { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [MaxLength(50)]
        public string ModifiedBy { get; set; }




    }
}
