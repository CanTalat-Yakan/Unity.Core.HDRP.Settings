using UnityEngine;

namespace UnityEssentials
{
    public class SettingsVerticalSynchronization : SettingsBase, ISettingsBase<int>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the VSync count based on the user's selection in the settings menu.\n" +
            "It listens for changes in the VSync setting and applies the selected value to Unity's QualitySettings.\n\n" +
            "This component populates the VSync options in the settings menu.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred VSync setting.";

        protected override string ProfileName => "Display";
        protected override string Reference => "VSync";

        public int Value { get; set; }
        public string[] Options { get; set; }
        public int Default => 0;

        public override void InitOptions() =>
            Options = new[]
            {
                "Disabled",
                "Every VBlank",
                "Every Second VBlank"
            };

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        public override void UpdateSettings() =>
            QualitySettings.vSyncCount = Value;
    }
}