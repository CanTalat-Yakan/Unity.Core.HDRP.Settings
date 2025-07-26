using UnityEngine;

namespace UnityEssentials
{
    public class SetWindowMode : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the window mode based on the user's selection in the settings menu.\n" +
            "It listens for changes in the window mode setting and applies the selected FullScreenMode to the game window.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public int WindowMode { get; private set; }

        private const string WindowModeReference = "window_mode";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            WindowMode = profile.Get<int>(reference = WindowModeReference);

        public void Update() =>
            Screen.fullScreenMode = (FullScreenMode)WindowMode;
    }
}