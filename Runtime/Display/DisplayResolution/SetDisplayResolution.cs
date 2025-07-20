using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplayResolution : MonoBehaviour
    {
        [Info]
        [SerializeField] private string _info =
            "This component sets the display resolution based on the settings profile.\n" +
            "It allows changing the screen resolution dynamically from the menu.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public Vector2Int DisplayResolution { get; private set; }

        private const string DisplayResolutionReference = "display_resolution";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateDisplayResolution(UIMenuProfile profile) =>
                DisplayResolution = profile.Get<string>(DisplayResolutionReference).ExtractFromString('x').ToVector2Int();

            UpdateDisplayResolution(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == DisplayResolutionReference)
                    UpdateDisplayResolution(profile);
            };
        }

        public void Update() =>
            Screen.SetResolution(DisplayResolution.x, DisplayResolution.y, Screen.fullScreenMode);
    }
}
