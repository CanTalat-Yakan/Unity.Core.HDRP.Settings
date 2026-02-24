using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsWindowMode : SettingsBase<FullScreenMode>
    {
        private const string Info =
            "Listens for changes in the window mode setting and applies the selected mode to the application window.";

        protected override FullScreenMode Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/WindowMode";

        public string[] Options { get; set; }

        public override void InitOptions() =>
            Options = Enum.GetNames(typeof(FullScreenMode))
                .Select(name => name.Format())
                .ToArray();

        public override void InitDefinition() =>
            Definition.SetOptions(Reference, Options, reverseOrder: true)
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = (FullScreenMode)GetProfileValue<int>();

        public override void UpdateSettings() =>
            Screen.fullScreenMode = Value;

        [Console("settings.display.windowMode", Info)]
        private string ConsoleWindowMode(int? mode) =>
            $"WindowMode = {GetOrSetProfileValue(mode).Value}";
    }
}