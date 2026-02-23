using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsWindowMode : SettingsBase<FullScreenMode>
    {
        [Info, SerializeField] private string _info =
            "Listens for changes in the window mode setting and applies the selected mode to the application window.";

        protected override FullScreenMode Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/WindowMode";

        public string[] Options { get; set; }

        public override void InitOptions() =>
            Options = Enum.GetNames(typeof(FullScreenMode))
                .Select(name => name.Format())
                .ToArray();

        public override void InitMetadata() =>
            Definition.SetOptions(Reference, Options, reverseOrder: true)
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = (FullScreenMode)Profile.Value.Get<int>(Reference);

        public override void UpdateSettings() =>
            Screen.fullScreenMode = Value;

        [Console("settings.display.windowMode", "Gets/sets window mode (FullScreenMode enum name or value).")]
        private string ConsoleWindowMode(int? mode)
        {
            if (mode == null) return $"WindowMode = {(FullScreenMode)Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, mode.Value);
            return $"WindowMode = {(FullScreenMode)mode.Value}";
        }
    }
}