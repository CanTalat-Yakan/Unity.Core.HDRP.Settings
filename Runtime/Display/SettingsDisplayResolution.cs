using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayResolution : SettingsBase, ISettingsBase<Vector2Int>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display resolution based on the settings profile.\n" +
            "It allows changing the screen resolution dynamically from the menu.\n\n" +
            "This component retrieves all available display resolutions from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred screen resolution.";

        protected override string ProfileName => "Display";
        protected override string Reference => "DisplayResolution";

        public Vector2Int Value { get; set; }
        public string[] Options { get; set; }
        public bool Reverse => false;
        public int Default => 0;

        public override void InitOptions()
        {
            Options = new string[Screen.resolutions.Length + 1];
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                Options[i] = $"{resolution.width}x{resolution.height}";
            }
            Options[^1] = "Native";
            Options = Options.Reverse().ToArray();
        }

        public override void InitValue(SettingsProfile profile) =>
            Value = Options[profile.Value.Get<int>(Reference)]
                .ExtractVector2FromString('x').ToVector2Int();

        protected override void SubscribeActions() =>
            SettingsDisplayInput.OnChanged += MarkDirty;

        protected override void UnsubscribeActions() =>
            SettingsDisplayInput.OnChanged -= MarkDirty;

        public override void UpdateSettings()
        {
            if (Value.x <= 0 || Value.y <= 0)
                Value = new(Screen.currentResolution.width, Screen.currentResolution.height);

            Screen.SetResolution(Value.x, Value.y, Screen.fullScreenMode);
        }
    }
}