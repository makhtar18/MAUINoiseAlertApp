using System;
using Microsoft.Maui.Controls.PlatformConfiguration;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace NoiseAlertApp
{
	internal class NotificationPermission : BasePlatformPermission
	{
    #if ANDROID
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions =>
            new List<(string permission,  bool isRuntime)>
            {
                ("android.permission.POST_NOTIFICATIONS",true)

            }.ToArray();
    #endif
    }
}

