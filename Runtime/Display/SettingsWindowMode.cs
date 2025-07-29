using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsWindowMode : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the window name based on the user's selection in the settings menu.\n" +
            "It listens for changes in the window name setting and applies the selected FullScreenMode to the game window.\n\n" +
            "This component populates the window name options in the settings menu by retrieving all available FullScreenMode enum names.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred window name.";

        public static FullScreenMode WindowMode { get; private set; }
        private static string[] WindowModeOptions { get; set; }
        private static string WindowModeReference { get; set; } = "window_mode";

        public override void InitializeGetter()
        {
            WindowModeOptions = Enum.GetNames(typeof(FullScreenMode))
                .Select(name => name.Format())
                .ToArray();

            var configurator = gameObject.AddComponent<UIMenuOptionsDataConfigurator>();
            configurator.Reverse = true;
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = WindowModeReference;
            configurator.Options = WindowModeOptions;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            WindowMode = (FullScreenMode)profile.Get<int>(reference = WindowModeReference);

        public override void UpdateSettings() =>
            Screen.fullScreenMode = WindowMode;
    }
}