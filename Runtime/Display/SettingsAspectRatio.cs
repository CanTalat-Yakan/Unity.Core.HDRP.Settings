using UnityEngine;

namespace UnityEssentials
{
    public class SettingsAspectRatio : SettingsBase<Vector2>
    {
        private const string Info =
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

        public override void InitDefinition() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = Options[GetProfileValue<int>()]
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

        [Console("settings.display.aspectRatio", Info)]
        private string ConsoleAspectRatio(int? index) =>
            $"AspectRatio index = {Options[GetOrSetProfileValue(index).Value]}";

        [Console("settings.display.aspectRatioForced", Info)] 
        private string ConsoleAspectRatioForced(float numerator, float denominator)
        {
            Value = new Vector2(numerator, denominator);
            UpdateSettings();
            return $"AspectRatio index = {Value}";
        }
    }
}