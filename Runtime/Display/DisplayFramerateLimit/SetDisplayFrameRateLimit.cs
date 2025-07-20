using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplayFrameRateLimit : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "SetDisplayFramerateLimit is responsible for managing the frame rate limit of the application. " +
            "It reads the frame rate limit from the settings profile and applies it to the application, " +
            "ensuring that the game runs at a consistent frame rate as defined by the user.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public int DisplayFrameRateLimit { get; private set; }

        private const string DisplayFrameRateLimitReference = "display_framerate_limit";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateDisplayFrameRateLimit(UIMenuProfile profile) =>
                DisplayFrameRateLimit = profile.Get<int>(DisplayFrameRateLimitReference);

            UpdateDisplayFrameRateLimit(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == DisplayFrameRateLimitReference)
                    UpdateDisplayFrameRateLimit(profile);
            };
        }

        public void Update()
        {
            if (DisplayFrameRateLimit <= 0)
                DisplayFrameRateLimit = (int)GetScreenFrameRate();

            Application.targetFrameRate = DisplayFrameRateLimit;
        }

        private float GetScreenFrameRate() =>
            Screen.currentResolution.refreshRateRatio.numerator / Screen.currentResolution.refreshRateRatio.denominator;
    }
}
