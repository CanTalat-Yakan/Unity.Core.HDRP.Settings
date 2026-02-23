using UnityEngine;

namespace UnityEssentials
{
    public class SettingsAspectRatio : SettingsBase<Vector2>
    {
        [Info, SerializeField] private string _info =
            "Sets the aspect ratio for the camera's render texture.";

        protected override Vector2 Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/AspectRatio";

        public string[] Options { get; set; }

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

        public override void InitMetadata() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Options[Profile.Value.Get<int>(Reference)]
                .ExtractVector2FromString(':');

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??=
            CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();

        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.AspectRatioNumerator = Mathf.Max(0, Value.x);
            RenderTextureHandler.Settings.AspectRatioDenominator = Mathf.Max(0, Value.y);
        }

        [Console("settings.display.aspectRatio", "Gets/sets aspect ratio option index (0=Auto).")]
        private string ConsoleAspectRatio(int? index)
        {
            if (index == null) return $"AspectRatio index = {Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, index.Value);
            return $"AspectRatio index = {Options[index.Value]}";
        }
    }
}