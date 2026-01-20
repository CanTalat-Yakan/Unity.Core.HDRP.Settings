using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SettingsResolutionScale : SettingsBase, ISettingsBase<int>, ISettingsSliderConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the resolution scale based on the settings profile.\n" +
            "It allows dynamic resolution scaling if the resolution scale is below 100%.";

        protected override string ProfileName => "Display";
        protected override string Reference => "ResolutionScale";

        public int Value { get; set; }
        public float MinValue => 10;
        public float MaxValue => 100;
        public float Default => 100;
        
        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        public override void UpdateSettings()
        {
            CameraProvider.Active?.SetDynamicResolution(Value < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => Value, 0);
        }
    }
}