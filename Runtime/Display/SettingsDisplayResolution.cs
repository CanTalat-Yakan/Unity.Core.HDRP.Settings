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

        public Vector2Int Value { get; set; }
        public string Reference => "display_resolution";
        
        public string[] Options { get; set; }
        public bool Reverse => false;

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

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = Options[profile.Value.Get<int>(reference = Reference)]
                .ExtractVector2FromString('x').ToVector2Int();

        public override void 
            BindAction(out Action source, out Action toBind) =>
            (source, toBind) = (SettingsDisplayInput.OnDisplayInputChanged, SetDirty);

        public override void UpdateSettings()
        {
            if (Value.x <= 0 || Value.y <= 0)
                Value = new(Screen.currentResolution.width, Screen.currentResolution.height);

            Screen.SetResolution(Value.x, Value.y, Screen.fullScreenMode);
        }
    }
}