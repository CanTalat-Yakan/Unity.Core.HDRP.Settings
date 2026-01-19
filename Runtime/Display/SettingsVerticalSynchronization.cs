using UnityEngine;

namespace UnityEssentials
{
    public class SettingsVerticalSynchronization : SettingsMenuBase, ISettingsBase<int>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the VSync count based on the user's selection in the settings menu.\n" +
            "It listens for changes in the VSync setting and applies the selected value to Unity's QualitySettings.\n\n" +
            "This component populates the VSync options in the settings menu.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred VSync setting.";

        public int Value { get; set; }
        public string Reference => "v-sync";
        
        public string[] Options { get; set; }
        public bool Reverse { get; }

        public override void InitOptions() =>
            Options = new[]
            {
                "Disabled",
                "Every VBlank",
                "Every Second VBlank"
            };

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<int>(reference = Reference);

        public override void UpdateSettings() =>
            QualitySettings.vSyncCount = Value;
    }
}