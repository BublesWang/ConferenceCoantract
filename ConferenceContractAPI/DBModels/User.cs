using RoleServiceAPI.DBModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RoleServiceAPI.DBModels
{
    public class User
    {
        public User()
        {
        }

        [Key]
        public Guid UserPK { get; set; }

        [MaxLength(50)]
        public string UserName { get; set; }

        [MaxLength(500)]
        public string UserPassword { get; set; }

        [MaxLength(20)]
        public string UserTel { get; set; }

        [MaxLength(20)]
        public string UserFax { get; set; }

        [MaxLength(20)]
        public string UserMobile { get; set; }

        [MaxLength(20)]
        public string UserEmail { get; set; }

        [MaxLength(200)]
        public string UserDept { get; set; }

        [MaxLength(500)]
        public string UserMaxForum { get; set; }

        [MaxLength(500)]
        public string UserMaxExhibitor { get; set; }

        [MaxLength(150)]
        public string UserRealNameCn { get; set; }

        [MaxLength(150)]
        public string UserRealNameEn { get; set; }

        [MaxLength(150)]
        public string UserRealNameJp { get; set; }

        [MaxLength(150)]
        public string UserAddresseCn { get; set; }

        [MaxLength(150)]
        public string UserAddresseEn { get; set; }

        [MaxLength(150)]
        public string UserAddresseJp { get; set; }

        [MaxLength(500)]
        public string UserExHall { get; set; }

        public DateTime? CreatedOn { get; set; }

        [MaxLength(50)]
        public string CreatedBy { get; set; }

        public DateTime? ModefieldOn { get; set; }

        [MaxLength(50)]
        public string ModefieldBy { get; set; }

        

    }
}
