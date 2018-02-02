using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VATMVCAPPFramework.Utilities.ValidationHelper;
using VATMVCAPPFramework.Data.IdentityModel;

namespace VATMVCAPPFramework.ViewModel
{
    [PropertiesMustMatch("Password", "ConfirmPassword", ErrorMessage = "Password and Confirm Password must match.")]
    [Serializable]
    public class AdminUserSettingViewModel
    {
        public Int64 Id { get; set; }
        [Required(ErrorMessage = "* Required")]
        [DisplayName("First Name")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Last Name")]
        [StringLength(50)]
        public string LastName { get; set; }
        
        [DisplayName("Middle Name")]
        [StringLength(50)]
        public string MiddleName { get; set; }


        [Required(ErrorMessage = "* Required")]
        [DisplayName("User Name")]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Mobile Number")]
        [StringLength(50)]
        public string MobileNumber { get; set; }
        
        [DisplayName("Phone Number")]
        [StringLength(50)]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "* Required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "* Required")]
        //[ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DataType(DataType.Password)]
        //[ValidatePasswordLength]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }



        public static AdminUserSettingViewModel EntityToModels(ApplicationUser dbmodel)
        {
            return dbmodel == null
                ? null
                : new AdminUserSettingViewModel
                {
                    Id = dbmodel.Id,
                    FirstName = dbmodel.FirstName,
                    LastName=dbmodel.LastName,
                    MiddleName=dbmodel.MiddleName,
                    UserName = dbmodel.UserName,
                    MobileNumber = dbmodel.MobileNumber,
                    PhoneNumber=dbmodel.PhoneNumber,
                    Email=dbmodel.Email


                    // _Id = ExtentionUtility.Encrypt(dbmodel.Id.ToString()),
                };
        }
    }
}