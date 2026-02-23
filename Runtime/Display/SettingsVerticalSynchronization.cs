using UnityEngine;

namespace UnityEssentials
{
    public class SettingsVerticalSynchronization : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Listens for changes in the vertical synchronization (VSync) setting and applies the selected VSync count to the QualitySettings.";

        protected override int Value { get; set; }

        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/VSync";

        public string[] Options { get; set; }

        public override void InitOptions() =>
            Options = new[]
            {
                "Disabled",
                "Full",
                "Half"
            };

        public override void InitMetadata() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public override void UpdateSettings() =>
            QualitySettings.vSyncCount = Value;

        [Console("settings.display.vsync", "Gets/sets VSync count (0=Disabled, 1=Full, 2=Half).")]
        private string ConsoleVSync(int? count) =>
            $"VSync = {GetOrSetProfileValue(count).Value}";
    }
}