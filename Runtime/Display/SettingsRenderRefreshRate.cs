using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderRefreshRate : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Specifies a refresh rate for rendering, which can differ from the screen refresh rate. " +
            "Performance optimization, as rendering at a lower refresh rate can improve GPU frame times while still displaying at a higher fps. " +
            "If set to 0, it will match the current screen refresh rate.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/RenderRefreshRate";

        public override void InitMetadata() =>
            Definition.SetIntSlider(Reference, 0, 1000, 1, 0, "FPS")
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

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

        [Console("settings.display.renderRefreshRate", "Gets/sets render refresh rate. 0 means match screen refresh rate.")]
        private string ConsoleRenderRefreshRate(int? fps)
        {
            if (fps == null) return $"RenderRefreshRate = {Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, fps.Value);
            return $"RenderRefreshRate = {fps.Value}";
        }
        
        [Console("settings.display.renderRequest", "Gets/sets send render request.")]
        private string ConsoleRenderRequest(bool? enabled)
        {
            if (enabled == null) return $"RenderRequest = {CameraRefreshRate.Settings.SendRenderRequest}";
            CameraRefreshRate.Settings.SendRenderRequest = enabled.Value;
            return $"RenderRefreshRate = {enabled.Value}";
        }
    }
}