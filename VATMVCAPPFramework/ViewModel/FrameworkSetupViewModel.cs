using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VATMVCAPPFramework.ViewModel
{
    [Serializable]
    public class FrameworkSetupViewModel
    {
        public string IntroMessage { get; set; }
        public AdminUserSettingViewModel AdminUserSetting { get; set; }
        public FrameworkDefaultSettingViewModel DefaultSetting { get; set; }

        public PortalSettingViewModel PortalSetting { get; set; }
    }
}