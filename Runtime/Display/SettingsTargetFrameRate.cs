using UnityEngine;

namespace UnityEssentials
{
    public class SettingsTargetFrameRate : SettingsBase<int>
    {
        [Info, SerializeField]
        private string _info =
            "Sets the target frame rate for the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/TargetFramerate";

        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetIntSlider(Reference, 0, 1000, 1, 0, "FPS")
                .SetTooltip(_info);

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        public override void UpdateSettings() =>
            Application.targetFrameRate = Value;
    }
}