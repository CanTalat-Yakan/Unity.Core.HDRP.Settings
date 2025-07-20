using UnityEngine;

namespace UnityEssentials
{
    public class SetWindowMode : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info = 
            "This component sets the window mode based on the user's selection in the settings menu.\n" +
            "It listens for changes in the window mode setting and applies the selected FullScreenMode to the game window.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public int WindowMode { get; private set; }

        private const string WindowModeReference = "window_mode";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateResolutionScale(UIMenuProfile profile) =>
                WindowMode = profile.Get<int>(WindowModeReference);

            UpdateResolutionScale(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == WindowModeReference)
                    UpdateResolutionScale(profile);
            };
        }

        public void Update() =>
            Screen.fullScreenMode = (FullScreenMode)WindowMode; 
    }
}
