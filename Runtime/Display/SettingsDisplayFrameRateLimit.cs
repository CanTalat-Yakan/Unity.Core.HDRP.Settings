using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayFrameRateLimit : SettingsMenuBase, ISettingsBase<int>, ISettingsSliderConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "SettingsDisplayFrameRateLimit is responsible for managing the frame rate limit for the display. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        public int Value { get; set; }
        public string Reference => "display_framerate_limit";
        
        public float MinValue => 0;
        public float MaxValue => 1000;
        public float Default => 0;

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<int>(reference = Reference);

        public override void UpdateSettings() =>
            Application.targetFrameRate = Value;
    }
}