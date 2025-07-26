using UnityEngine;

namespace UnityEssentials
{
    public class SetGlobalFrameRateLimit : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "SetGlobalFrameRateLimit is responsible for managing the global frame rate limit for the application. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public int GlobalFrameRateLimit { get; private set; }

        private const string GlobalFrameRateLimitReference = "global_framerate_limit";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            GlobalFrameRateLimit = profile.Get<int>(reference = GlobalFrameRateLimitReference);

        public void Update()
        {
            if (GlobalFrameRateLimit <= 0)
                GlobalFrameRateLimit = (int)GetScreenFrameRate();

            GlobalRefreshRateLimiter.SetTargetFrameRate(GlobalFrameRateLimit);
        }

        private float GetScreenFrameRate() =>
            Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator;
    }
}