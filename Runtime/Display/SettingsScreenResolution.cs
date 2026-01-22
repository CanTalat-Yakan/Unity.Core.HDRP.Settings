using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsScreenResolution : SettingsBase<Vector2Int>
    {
        [Info, SerializeField]
        private string _info =
            "Listens for changes in the screen resolution setting and applies the selected resolution to the application window.";

        protected override Vector2Int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/ScreenResolution";

        public string[] Options { get; set; }

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

        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetOptions(Reference, Options)
                .SetTooltip(_info);
        
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