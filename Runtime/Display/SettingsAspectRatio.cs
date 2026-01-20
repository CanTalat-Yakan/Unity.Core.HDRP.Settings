using UnityEngine;

namespace UnityEssentials
{
    public class SettingsAspectRatio : SettingsBase, ISettingsBase<Vector2>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the aspect ratio of the camera render texture based on the settings profile.\n" +
            "It listens for changes in the aspect ratio setting and applies the selected aspect ratio to the camera render texture handler.\n\n" +
            "This component populates the aspect ratio options in the settings menu.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred aspect ratio.";

        protected override string ProfileName => "Display";
        protected override string Reference => "AspectRatio";

        public Vector2 Value { get; set; }
        public string[] Options { get; set; }
        public int Default => 0;

        public override void InitOptions() =>
            Options = new[]
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

        public override void InitValue(SettingsProfile profile) =>
            Value = Options[profile.Value.Get<int>(Reference)]
                .ExtractVector2FromString(':');

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.AspectRatioNumerator = Mathf.Max(0, Value.x);
            RenderTextureHandler.Settings.AspectRatioDenominator = Mathf.Max(0, Value.y);
        }
    }
}