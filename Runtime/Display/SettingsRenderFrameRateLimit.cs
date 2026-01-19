using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderFrameRateLimit : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "SettingsRenderFrameRateLimit is responsible for managing the frame rate limit for rendering cameras. " +
            "It allows the user to set a specific frame rate limit through the settings menu, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        public static int RenderFrameRateLimit { get; private set; }
        private static string RenderFrameRateLimitReference { get; set; } = "render_framerate_limit";

        public override void InitializeGetter()
        {
            var configurator = gameObject.AddComponent<MenuSliderDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = RenderFrameRateLimitReference;
            configurator.MinValue = 0;
            configurator.MaxValue = 1000;
            configurator.Default = 120;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(SettingsProfile profile, out string reference) =>
            RenderFrameRateLimit = profile.Value.Get<int>(reference = RenderFrameRateLimitReference);

        public CameraFrameRateLimiter CameraFrameRateLimiter => _cameraFrameRateLimiter ??= CameraProvider.Active?.GetComponent<CameraFrameRateLimiter>();
        private CameraFrameRateLimiter _cameraFrameRateLimiter;

        public override void UpdateSettings()
        {
            if (RenderFrameRateLimit <= 0)
                RenderFrameRateLimit = (int)GetScreenFrameRate();

            CameraFrameRateLimiter?.SetTargetFrameRate(RenderFrameRateLimit);
        }

        private float GetScreenFrameRate() =>
            Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator;
    }
}