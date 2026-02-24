using UnityEngine;

namespace UnityEssentials
{
    public class SettingsTargetFrameRate : SettingsBase<int>
    {
        private const string Info =
            "Sets the target frame rate for the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/TargetFramerate";

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 0, 1000, 1, 0, "FPS")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public override void UpdateSettings() =>
            Application.targetFrameRate = Value;

        [Console("settings.display.targetFrameRate",Info)]
        private string ConsoleTargetFrameRate(int? fps) =>
            $"TargetFrameRate = {GetOrSetProfileValue(fps).Value}";
    }
}