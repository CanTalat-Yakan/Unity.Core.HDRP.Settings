using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsVoiceVolume : SettingsBase, ISettingsBase<int>, ISettingsSliderConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the voice volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the voice volume setting and applies the selected volume level to the audio mixer.";

        [Space]
        public AudioMixer AudioMixer;

        public int Value { get; set; }
        public string Reference => "voice_volume";
        
        public float MinValue => 0;
        public float MaxValue => 200;
        public float Default => 100;

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<int>(reference = Reference);

        private const string VoiceVolumeParameter = "voice";
        public override void UpdateSettings() => 
            AudioMixer?.SetFloat(VoiceVolumeParameter, Value.ToDecibelLevel());
    }
}