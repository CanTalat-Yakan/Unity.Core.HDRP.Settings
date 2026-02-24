using UnityEngine;

namespace UnityEssentials
{
    public class SettingsVerticalSynchronization : SettingsBase<int>
    {
        private const string Info =
            "Listens for changes in the vertical synchronization (VSync) setting and applies the selected VSync count to the QualitySettings.";

        protected override int Value { get; set; }

        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/VSync";

        public string[] Options { get; set; }

        public override void InitOptions() =>
            Options = new[]
            {
                "Disabled",
                "Full",
                "Half"
            };

        public override void InitDefinition() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public override void UpdateSettings() =>
            QualitySettings.vSyncCount = Value;

        [Console("settings.rendering.vsync", Info)]
        private string ConsoleVSync(int? count) =>
            $"VSync = {GetOrSetProfileValue(count).Value}";
    }
}