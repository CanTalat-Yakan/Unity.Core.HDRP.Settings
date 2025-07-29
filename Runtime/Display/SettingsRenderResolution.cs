using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderResolution : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        public static Vector2Int RenderResolution { get; private set; }
        private static string[] RenderResolutionOptions { get; set; }
        private static string RenderResolutionReference { get; set; } = "render_resolution";

        public override void InitializeGetter()
        {
            RenderResolutionOptions = new string[Screen.resolutions.Length + 1];
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                RenderResolutionOptions[i] = $"{resolution.width}x{resolution.height}";
            }
            RenderResolutionOptions[^1] = "Native";
            RenderResolutionOptions = RenderResolutionOptions.Reverse().ToArray();

            var configurator = gameObject.AddComponent<UIMenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = RenderResolutionReference;
            configurator.Options = RenderResolutionOptions;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            RenderResolution = RenderResolutionOptions[profile.Get<int>(reference = RenderResolutionReference)]
                .ExtractVector2FromString('x').ToVector2Int();

        public override void BindAction(out Action source, out Action toBind) =>
            (source, toBind) = (SettingsDisplayInput.OnDisplayInputChanged, SetDirty);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            if (RenderResolution.x <= 0 || RenderResolution.y <= 0)
                RenderResolution = SettingsDisplayResolution.DisplayResolution;

            RenderTextureHandler.Settings.RenderWidth = RenderResolution.x;
            RenderTextureHandler.Settings.RenderHeight = RenderResolution.y;
        }
    }
}