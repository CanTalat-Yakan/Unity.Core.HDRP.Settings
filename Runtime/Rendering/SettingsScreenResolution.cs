using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsScreenResolution : SettingsBase<Vector2Int>
    {
        private const string Info =
            "Listens for changes in the screen resolution setting and applies the selected resolution to the application window.";

        protected override Vector2Int Value { get; set; }
        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/ScreenResolution";

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

        public override void InitDefinition() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = Options[GetProfileValue<int>()]
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

        [Console("settings.rendering.screenResolution", Info)]
        private string ConsoleScreenResolution(int? index) =>
            $"ScreenResolution index = {Options[GetOrSetProfileValue(index).Value]}";

        [Console("settings.rendering.screenResolutionForced",Info)]
        private string ConsoleScreenResolutionForced(int width, int height)
        {
            Value = new Vector2Int(width, height);
            UpdateSettings();
            return $"ScreenResolution = {Value.x}x{Value.y}";
        }
    }
}