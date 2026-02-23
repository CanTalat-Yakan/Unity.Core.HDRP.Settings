using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SettingsResolutionScale : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Enables or disables dynamic resolution and sets the resolution scale factor accordingly.";

        protected override int Value { get; set; }

        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/ResolutionScale";

        public override void InitMetadata() =>
            Definition.SetIntSlider(Reference, 10, 100, 1, 100, "%")
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

        public override void UpdateSettings()
        {
            CameraProvider.Active?.SetDynamicResolution(Value < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => Value, 0);
        }

        [Console("settings.display.resolutionScale", "Gets/sets dynamic resolution scale (percent).")]
        private string ConsoleResolutionScale(int? percent)
        {
            if (percent == null) return $"ResolutionScale = {Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, percent.Value);
            return $"ResolutionScale = {percent.Value}";
        }
    }
}