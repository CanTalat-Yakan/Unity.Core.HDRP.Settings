using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplayResolution : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display resolution based on the settings profile.\n" +
            "It allows changing the screen resolution dynamically from the menu.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public Vector2Int DisplayResolution { get; private set; }

        private const string DisplayResolutionReference = "display_resolution";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            DisplayResolution = GetDisplayResolution.Options[profile.Get<int>(reference = DisplayResolutionReference)]
                .ExtractVector2FromString('x').ToVector2Int();

        private void OnEnable() => SetDisplaySelection.OnDisplaySelectionChanged += InvokeUpdateValueCallback;
        private void OnDisable() => SetDisplaySelection.OnDisplaySelectionChanged -= InvokeUpdateValueCallback;

        public void Update()
        {
            if (DisplayResolution.x <= 0 || DisplayResolution.y <= 0)
                DisplayResolution = new(Screen.currentResolution.width, Screen.currentResolution.height);

            Screen.SetResolution(DisplayResolution.x, DisplayResolution.y, Screen.fullScreenMode);
        }
    }
}