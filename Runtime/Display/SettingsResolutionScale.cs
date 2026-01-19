using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SettingsResolutionScale : SettingsMenuBase, ISettingsBase<int>, ISettingsSliderConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the resolution scale based on the settings profile.\n" +
            "It allows dynamic resolution scaling if the resolution scale is below 100%.";

        public int Value { get; set; }
        public string Reference => "resolution_scale";
        
        public float MinValue => 10;
        public float MaxValue => 100;
        public float Default => 100;
        
        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<int>(reference = Reference);

        public override void UpdateSettings()
        {
            CameraProvider.Active?.SetDynamicResolution(Value < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => Value, 0);
        }
    }
}