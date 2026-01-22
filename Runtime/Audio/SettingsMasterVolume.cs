using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMasterVolume : SettingsBase<int>
    {
        [Info, SerializeField]
        private string _info =
            "Adjusts the overall master volume level within the application.";

        [Space]
        public AudioMixer AudioMixer;

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/MasterVolume";
        
        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetIntSlider(Reference, 0, 100, 100)
                .SetTooltip(_info);

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        private const string MasterVolumeParameter = "master";
        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(MasterVolumeParameter, Value.ToDecibelLevel());
    }
}