using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderRefreshRate : SettingsBase<int>
    {
        private const string Info =
            "Specifies a refresh rate for rendering, which can differ from the screen refresh rate. " +
            "Performance optimization, as rendering at a lower refresh rate can improve GPU frame times while still displaying at a higher fps. " +
            "If set to 0, it will match the current screen refresh rate.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/RenderRefreshRate";

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 0, 1000, 1, 0, "FPS")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public CameraRefreshRate CameraRefreshRate =>
            _cameraRefreshRate ??= CameraProvider.Active?.GetComponent<CameraRefreshRate>();

        private CameraRefreshRate _cameraRefreshRate;

        public override void UpdateSettings()
        {
            if (Value <= 0)
            {
                var ratio = Screen.currentResolution.refreshRateRatio;
                Value = Mathf.CeilToInt(ratio.numerator / ratio.denominator);
            }

            CameraRefreshRate?.SetTarget(Value);
        }

        [Console("settings.rendering.renderRefreshRate", "Gets/sets render refresh rate. 0 means match screen refresh rate.")]
        private string ConsoleRenderRefreshRate(int? fps)
        {
            if (fps == null) return $"RenderRefreshRate = {Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, fps.Value);
            return $"RenderRefreshRate = {fps.Value}";
        }
        
        [Console("settings.rendering.renderRequest", Info)]
        private string ConsoleRenderRequest(bool? enabled) =>
            $"RenderRefreshRate = {GetOrSetProfileValue(enabled).Value}";
    }
}