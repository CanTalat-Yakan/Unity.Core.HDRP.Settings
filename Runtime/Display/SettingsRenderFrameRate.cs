using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderFrameRate : SettingsBase, ISettingsBase<int>, ISettingsSliderConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "SettingsRenderFrameRate is responsible for managing the frame rate limit for rendering cameras. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        public int Value { get; set; }
        public string Reference => "render_framerate";

        public float MinValue => 0;
        public float MaxValue => 1000;
        public float Default => 120;

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<int>(reference = Reference);

        public CameraFrameRate CameraFrameRate => _cameraFrameRate ??= CameraProvider.Active?.GetComponent<CameraFrameRate>();
        private CameraFrameRate _cameraFrameRate;

        public override void UpdateSettings()
        {
            if (Value <= 0)
            {
                var ratio = Screen.currentResolution.refreshRateRatio;
                Value = Mathf.CeilToInt(ratio.numerator / ratio.denominator);
            }

            CameraFrameRate?.SetTargetFrameRate(Value);
        }
    }
}