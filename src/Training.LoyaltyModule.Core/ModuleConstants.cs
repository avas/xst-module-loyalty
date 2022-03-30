using System;
using System.Collections.Generic;
using VirtoCommerce.Platform.Core.Settings;

namespace Training.LoyaltyModule.Core
{
    public static class ModuleConstants
    {
        public static class Security
        {
            public static class Permissions
            {
                public static string[] AllPermissions { get; } = Array.Empty<string>();
            }
        }

        public static class Settings
        {
            public static IEnumerable<SettingDescriptor> AllSettings => Array.Empty<SettingDescriptor>();
        }
    }
}
