using UnityEngine;

namespace UnityEssentials
{
    public class SettingsGlobalFrameRate : SettingsBase, ISettingsBase<int>, ISettingsSliderConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "SettingsGlobalFrameRate is responsible for managing the global frame rate limit for the application. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        public int Value { get; set; }
        public string Reference => "global_framerate";

        public float MinValue => 0;
        public float MaxValue => 1000;
        public float Default => 1000;

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<int>(reference = Reference);

        public override void UpdateSettings()
        {
            if (Value <= 0)
            {
                var ratio = Screen.currentResolution.refreshRateRatio;
                Value = Mathf.CeilToInt(ratio.numerator / ratio.denominator);
            }

            GlobalRefreshRate.SetTargetRefreshRate(Value);
        }
    }
}