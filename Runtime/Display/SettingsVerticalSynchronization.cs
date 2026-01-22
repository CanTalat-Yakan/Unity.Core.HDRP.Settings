using UnityEngine;

namespace UnityEssentials
{
    public class SettingsVerticalSynchronization : SettingsBase<int>
    {
        [Info, SerializeField]
        private string _info =
            "Listens for changes in the vertical synchronization (VSync) setting and applies the selected VSync count to the QualitySettings.";

        protected override int Value { get; set; }
        
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/VSync";

        public string[] Options { get; set; }
        
        public override void InitOptions() =>
            Options = new[]
            {
                "Disabled",
                "Every VBlank",
                "Every Second VBlank"
            };

        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetOptions(Reference, Options)
                .SetTooltip(_info);

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        public override void UpdateSettings() =>
            QualitySettings.vSyncCount = Value;
    }
}