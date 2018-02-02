using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VATMVCAPPFramework.Utilities.ValidationHelper;
using VATMVCAPPFramework.Data.IdentityModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using VATMVCAPPFramework.Utilities;

namespace VATMVCAPPFramework.ViewModel
{
    public class UserViewModel
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
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10,14})$", ErrorMessage = "Contact Mobile Number is not valid")]
        public string MobileNumber { get; set; }

        [DisplayName("Phone Number")]
        [StringLength(50)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(\d{10,14})$", ErrorMessage = "Contact Phone Number is not valid")]
        public string PhoneNumber { get; set; }

        public string RoleLists { get; set; }

        [DisplayName("Date of Birth")]
        public string DOB { get; set; }

        [DisplayName("Home Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email address is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "Role(s)")]
        public IEnumerable<string> SelectedRole { get; set; }
        public IEnumerable<long> SelectedRoleId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Roles { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public virtual ICollection<ApplicationUserRole> UserRoles{ get;set;  }

        public static ApplicationUser ModeltoEntity(UserViewModel model)
        {
            if(model== null)
            {
                return null;
            }else
            {
                var usermodel = new ApplicationUser();
                usermodel.FirstName = model.FirstName;
                usermodel.LastName = model.LastName;
                usermodel.MiddleName = model.MiddleName;
                usermodel.UserName = model.UserName;
                usermodel.MobileNumber = model.MobileNumber;
                usermodel.PhoneNumber = model.PhoneNumber;
                usermodel.Email = model.Email;
                //try
                //{
                //    if (!string.IsNullOrEmpty(model.DOB))
                //    {
                //        usermodel.DOB = ExtentionUtility.GetDateValue(model.DOB);
                //    }
                //}
                //catch
                //{
                //}
                //  DOB = !string.IsNullOrEmpty(model.DOB) ? Convert.ToDateTime(model.DOB) : null,
              
                //usermodel.Address = model.Address;
                usermodel.EmailConfirmed = true;
                usermodel.PhoneNumberConfirmed = true;
                usermodel.TwoFactorEnabled = false;
                usermodel.LockoutEnabled = false;
                usermodel.AccessFailedCount = 0;
                usermodel.DateCreated = DateTime.Now;
                usermodel.IsFirstLogin = true;
                return usermodel;

            }
            //return model == null
            //   ? null
            //   : new ApplicationUser
            //   {
            //       FirstName = model.FirstName,
            //       LastName = model.LastName,
            //       MiddleName = model.MiddleName,
            //       UserName = model.UserName,
            //       MobileNumber = model.MobileNumber,
            //       PhoneNumber = model.PhoneNumber,
            //       Email = model.Email,
            //       DOB = !string.IsNullOrEmpty(model.DOB) ? Convert.ToDateTime(model.DOB) : null,
            //       Address = model.Address,
            //       EmailConfirmed = true,
            //       PhoneNumberConfirmed = true,
            //       TwoFactorEnabled = false,
            //       LockoutEnabled = false,
            //       AccessFailedCount = 0,
            //       DateCreated = DateTime.Now,
            //       IsFirstLogin = true
            //   };
        }
        public static UserViewModel EntityToModels(ApplicationUser dbmodel)
        {
            return dbmodel == null
                ? null
                
                : new UserViewModel
                {
                    Id = dbmodel.Id,
                    FirstName = dbmodel.FirstName,
                    LastName = dbmodel.LastName,
                    MiddleName = dbmodel.MiddleName,
                    UserName = dbmodel.UserName,
                    MobileNumber = dbmodel.MobileNumber,
                    PhoneNumber = dbmodel.PhoneNumber,
                    Email = dbmodel.Email,
                    //DOB=dbmodel.DOB.HasValue ? dbmodel.DOB.GetValueOrDefault().ToString("dd/MM/yyyy") :string.Empty,
                    //Address=dbmodel.Address,
                    UserRoles=dbmodel.Roles,
                    IsActive=dbmodel.IsActive
                };
        }
    }
}