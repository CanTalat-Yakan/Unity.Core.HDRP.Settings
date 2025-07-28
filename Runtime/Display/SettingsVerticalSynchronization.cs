using UnityEngine;

namespace UnityEssentials
{
    public class SettingsVerticalSynchronization : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the VSync count based on the user's selection in the settings menu.\n" +
            "It listens for changes in the VSync setting and applies the selected value to Unity's QualitySettings.\n\n" +
            "This component populates the VSync options in the settings menu.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred VSync setting.";

        public static int VSync { get; private set; }
        private static string[] VSyncOptions { get; set; }
        private static string VSyncReference { get; set; } = "v-sync";

        public override void InitializeGetter()
        {
            VSyncOptions = new string[]
            {
                "Disabled",
                "Every VBlank",
                "Every Second VBlank"
            };

            var configurator = gameObject.AddComponent<UIMenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = VSyncReference;
            configurator.Options = VSyncOptions;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            VSync = profile.Get<int>(reference = VSyncReference);

        public override void UpdateSettings() =>
            QualitySettings.vSyncCount = VSync;
    }
}