using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplayFrameRateLimit : SettingsMenuSetterBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "SetDisplayFrameRateLimit is responsible for managing the frame rate limit for the display. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public int DisplayFrameRateLimit { get; private set; }

        private const string DisplayFrameRateLimitReference = "display_framerate_limit";

        public void Start() =>
            InitializeSetter(DisplayFrameRateLimitReference, (profile) =>
                DisplayFrameRateLimit = profile.Get<int>(DisplayFrameRateLimitReference));

        public void Update()
        {
            Application.targetFrameRate = DisplayFrameRateLimit;
        }

        private float GetScreenFrameRate() =>
            Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator;
    }
}