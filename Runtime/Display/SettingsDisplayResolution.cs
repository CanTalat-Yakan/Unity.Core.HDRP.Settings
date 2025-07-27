using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayResolution : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display resolution based on the settings profile.\n" +
            "It allows changing the screen resolution dynamically from the menu.\n\n" +
            "This component retrieves all available display resolutions from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred screen resolution.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public Vector2Int DisplayResolution { get; private set; }
        public static string[] DisplayResolutionOptions { get; private set; }
        public static string DisplayResolutionReference { get; private set; } = "display_resolution";

        public override void InitializeGetter()
        {
            DisplayResolutionOptions = new string[Screen.resolutions.Length + 1];
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                DisplayResolutionOptions[i] = $"{resolution.width}x{resolution.height}";
            }
            DisplayResolutionOptions[^1] = "Native";
            DisplayResolutionOptions = DisplayResolutionOptions.Reverse().ToArray();

            var configurator = gameObject.AddComponent<UIMenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = DisplayResolutionReference;
            configurator.Options = DisplayResolutionOptions;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            DisplayResolution = DisplayResolutionOptions[profile.Get<int>(reference = DisplayResolutionReference)]
                .ExtractVector2FromString('x').ToVector2Int();

        public override void BindAction(out Action source, out Action toBind) =>
            (source, toBind) = (SettingsDisplayInput.OnDisplayInputChanged, SetDirty);

        public override void UpdateSettings()
        {
            if (DisplayResolution.x <= 0 || DisplayResolution.y <= 0)
                DisplayResolution = new(Screen.currentResolution.width, Screen.currentResolution.height);

            Screen.SetResolution(DisplayResolution.x, DisplayResolution.y, Screen.fullScreenMode);
        }
    }
}