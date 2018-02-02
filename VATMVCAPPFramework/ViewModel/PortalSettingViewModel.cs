using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using VATMVCAPPFramework.Data.Entities;

namespace VATMVCAPPFramework.ViewModel
{
    public class PortalSettingViewModel
    {
        public Int64 Id { get; set;}

        public bool HasAdminUserConfigured { get; set; }
        [Required(ErrorMessage = "* Required")]
        [StringLength(100)]
        [DisplayName("Portal Title")]
        public string PortalTitle { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Portal Description")]
        public string PortalDescription { get; set; }

        [StringLength(200)]
        [DisplayName("Terms & Condition")]
        public string TermsAndConditionPath { get; set; }

        [DisplayName("Display Picture Icon?")]
        public bool DisplayPictureBesideUserName
        {
            get { return PictureBesideUsername > 0; }
            set { PictureBesideUsername = value ? 1 : 0; }
        }

        [ScaffoldColumn(false)]
        [Range(0, 1)]
        public int PictureBesideUsername { get; set; }

        //[Required(ErrorMessage = "* Required")]
        //[StringLength(100)]
        //[DisplayName("Default Portal Style")]
        //public string DefaultStylePath { get; set; }
        //[Required(ErrorMessage = "* Required")]
        //public int PortalMenuType { get; set; }
        //[Required(ErrorMessage = "* Required")]
        //public int PortalTheme { get; set; }



        public static PortalSettingViewModel EntityToModels(Application dbmodel)
        {
            return dbmodel == null
                ? null
                : new PortalSettingViewModel
                {
                    Id = dbmodel.Id,
                    PortalTitle = dbmodel.ApplicationName,
                    PortalDescription=dbmodel.Description,
                    HasAdminUserConfigured=dbmodel.HasAdminUserConfigured

                    // _Id = ExtentionUtility.Encrypt(dbmodel.Id.ToString()),
                };
        }

    }
}