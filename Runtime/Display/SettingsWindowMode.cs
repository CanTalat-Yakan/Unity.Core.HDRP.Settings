using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsWindowMode : SettingsMenuBase, ISettingsBase<FullScreenMode>, ISettingsOptionsConfiguration
    {
        [Info] [SerializeField] private string _info =
            "This component sets the window name based on the user's selection in the settings menu.\n" +
            "It listens for changes in the window name setting and applies the selected FullScreenMode to the game window.\n\n" +
            "This component populates the window name options in the settings menu by retrieving all available FullScreenMode enum names.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred window name.";

        public FullScreenMode Value { get; set; }
        public string Reference => "window_mode";

        public string[] Options { get; set; }
        public bool Reverse => true;

        public override void InitOptions() =>
            Options = Enum.GetNames(typeof(FullScreenMode))
                .Select(name => name.Format())
                .ToArray();

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = (FullScreenMode)profile.Value.Get<int>(reference = Reference);

        public override void UpdateSettings() =>
            Screen.fullScreenMode = Value;
    }
}