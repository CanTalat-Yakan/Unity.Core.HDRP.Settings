using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsEffectsVolume : SettingsBase, ISettingsBase<int>, ISettingsSliderConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the effects volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the effects volume setting and applies the selected volume level to the audio mixer.";

        [Space]
        public AudioMixer AudioMixer;

        protected override string ProfileName => "Audio";
        protected override string Reference => "EffectsVolume";
        
        public int Value { get; set; }
        public float MinValue => 0;
        public float MaxValue => 200;
        public float Default => 100;
        
        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        private const string EffectsVolumeParameter = "effects";
        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(EffectsVolumeParameter, Value.ToDecibelLevel());
    }
}