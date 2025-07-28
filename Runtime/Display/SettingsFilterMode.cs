using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsFilterMode : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the filter mode based on the user's selection in the settings menu.\n" +
            "It listens for changes in the filter mode setting and applies the selected FilterMode to the camera render texture handler.\n\n" +
            "This component populates the filter mode options in the settings menu by retrieving all available FilterMode enum names.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred texture filtering mode.";

        public static int FilterMode { get; private set; }
        private static string[] FilterModeOptions { get; set; }
        private static string FilterModeReference { get; set; } = "filter_mode"; 

        public override void InitializeGetter()
        {
            FilterModeOptions = Enum.GetNames(typeof(FilterMode));

            var configurator = gameObject.AddComponent<UIMenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = FilterModeReference;
            configurator.Options = FilterModeOptions;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            FilterMode = profile.Get<int>(reference = FilterModeReference);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.FilterMode = (FilterMode)FilterMode;
        }
    }
}