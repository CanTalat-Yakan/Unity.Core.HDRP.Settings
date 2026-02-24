using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SettingsResolutionScale : SettingsBase<int>
    {
        private const string Info =
            "Enables or disables dynamic resolution and sets the resolution scale factor accordingly.";

        protected override int Value { get; set; }

        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/ResolutionScale";

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 10, 100, 1, 100, "%")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

        public override void UpdateSettings()
        {
            CameraProvider.Active?.SetDynamicResolution(Value < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => Value, 0);
        }

        [Console("settings.rendering.resolutionScale", Info)]
        private string ConsoleResolutionScale(int? percent) =>
            $"ResolutionScale = {GetOrSetProfileValue(percent).Value}";
    }
}