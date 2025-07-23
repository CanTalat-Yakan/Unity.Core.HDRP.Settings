using UnityEngine;

namespace UnityEssentials
{
    public class SetVerticalSynchronization : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the VSync count based on the user's selection in the settings menu.\n" +
            "It listens for changes in the VSync setting and applies the selected value to Unity's QualitySettings.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public int VSync { get; private set; }

        private const string VSyncReference = "v-sync";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateVSync(UIMenuProfile profile) =>
                VSync = profile.Get<int>(VSyncReference);

            UpdateVSync(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == VSyncReference)
                    UpdateVSync(profile);
            };
        }

        public void Update() =>
            QualitySettings.vSyncCount = VSync;
    }
}
