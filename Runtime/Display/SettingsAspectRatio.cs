using UnityEngine;

namespace UnityEssentials
{
    public class SettingsAspectRatio : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the aspect ratio of the camera render texture based on the settings profile.\n" +
            "It listens for changes in the aspect ratio setting and applies the selected aspect ratio to the camera render texture handler.\n\n" +
            "This component populates the aspect ratio options in the settings menu.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred aspect ratio.";

        public static Vector2 AspectRatio { get; private set; }
        private static string[] AspectRatioOptions { get; set; }
        private static string AspectRatioReference { get; set; } = "aspect_ratio";

        public override void InitializeGetter()
        {
            AspectRatioOptions = new[]
            {
                "Auto",
                "16:9",
                "16:10",
                "21:9",
                "32:9",
                "9:16",
                "10:16",
                "1:1",
                "4:3",
                "2.39:1",
                "2.35:1"
            };

            var configurator = gameObject.AddComponent<MenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = AspectRatioReference;
            configurator.Options = AspectRatioOptions;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(MenuProfile profile, out string reference) =>
            AspectRatio = AspectRatioOptions[profile.Get<int>(reference = AspectRatioReference)]
                .ExtractVector2FromString(':');

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            var aspectRatioNumerator = Mathf.Max(0, AspectRatio.x);
            var aspectRatioDenominator = Mathf.Max(0, AspectRatio.y);

            RenderTextureHandler.Settings.AspectRatioNumerator = aspectRatioNumerator;
            RenderTextureHandler.Settings.AspectRatioDenominator = aspectRatioDenominator;
        }
    }
}