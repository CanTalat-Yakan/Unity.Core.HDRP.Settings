using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplayFrameRateLimit : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "SetDisplayFrameRateLimit is responsible for managing the frame rate limit for the display. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public int DisplayFrameRateLimit { get; private set; }

        private const string DisplayFrameRateLimitReference = "display_framerate_limit";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            DisplayFrameRateLimit = profile.Get<int>(reference = DisplayFrameRateLimitReference);

        public void Update() =>
            Application.targetFrameRate = DisplayFrameRateLimit;
    }
}