using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderRefreshRate : SettingsBase<int>
    {
        [Info, SerializeField]
        private string _info =
            "Specifies a refresh rate for rendering, which can differ from the screen refresh rate. " +
            "Performance optimization, as rendering at a lower refresh rate can improve GPU frame times while still displaying at a higher fps. " +
            "If set to 0, it will match the current screen refresh rate.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/RenderRefreshRate";

        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetIntSlider(Reference, 0, 1000, 1, 120, "FPS")
                .SetTooltip(_info);

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        public CameraRefreshRate CameraRefreshRate => _cameraRefreshRate ??= CameraProvider.Active?.GetComponent<CameraRefreshRate>();
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
    }
}