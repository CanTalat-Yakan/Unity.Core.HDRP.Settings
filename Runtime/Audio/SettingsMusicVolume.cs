using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMusicVolume : SettingsBase<int>
    {
        [Info, SerializeField]
        private string _info =
            "Adjusts the volume level for music audio within the application.";

        [Space]
        public AudioMixer AudioMixer;

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/MusicVolume";
        
        public float MinValue => 0;
        public float MaxValue => 200;
        public float Default => 100;

        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetIntSlider(Reference, 0, 200, 100)
                .SetTooltip(_info);

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        private const string MusicVolumeParameter = "music";
        public override void UpdateSettings() => 
            AudioMixer?.SetFloat(MusicVolumeParameter, Value.ToDecibelLevel());
    }
}