using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SettingsResolutionScale : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the resolution scale based on the settings profile.\n" +
            "It allows dynamic resolution scaling if the resolution scale is below 100%.";

        public static int ResolutionScale { get; private set; }
        private static string ResolutionScaleReference { get; set; } = "resolution_scale";

        public override void InitializeGetter()
        {
            var configurator = gameObject.AddComponent<MenuSliderDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = ResolutionScaleReference;
            configurator.MinValue = 10;
            configurator.MaxValue = 100;
            configurator.Default = 100;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(MenuProfile profile, out string reference) =>
            ResolutionScale = profile.Get<int>(reference = ResolutionScaleReference);

        public override void UpdateSettings()
        {
            CameraProvider.Active?.SetDynamicResolution(ResolutionScale < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => ResolutionScale, 0);
        }
    }
}