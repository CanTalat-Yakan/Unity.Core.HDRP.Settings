using UnityEngine;

namespace UnityEssentials
{
    public class SetVerticalSynchronization : SettingsMenuSetterBase
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

        public void Start() =>
            InitializeSetter(VSyncReference, (profile) =>
                VSync = profile.Get<int>(VSyncReference));

        public void Update() =>
            QualitySettings.vSyncCount = VSync;
    }
}