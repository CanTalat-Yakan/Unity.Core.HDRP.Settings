using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayFrameRateLimit : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "SettingsDisplayFrameRateLimit is responsible for managing the frame rate limit for the display. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        public static int DisplayFrameRateLimit { get; private set; }
        private static string DisplayFrameRateLimitReference { get; set; } = "display_framerate_limit"; 

        public override void InitializeGetter()
        {
            var configurator = gameObject.AddComponent<MenuSliderDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = DisplayFrameRateLimitReference;
            configurator.MinValue = 0;
            configurator.MaxValue = 1000;
            configurator.Default = 0;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(SettingsProfile profile, out string reference) =>
            DisplayFrameRateLimit = profile.Value.Get<int>(reference = DisplayFrameRateLimitReference);

        public override void UpdateSettings() =>
            Application.targetFrameRate = DisplayFrameRateLimit;
    }
}