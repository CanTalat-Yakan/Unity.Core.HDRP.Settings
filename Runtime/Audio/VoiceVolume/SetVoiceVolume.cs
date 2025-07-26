using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SetVoiceVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the voice volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the voice volume setting and applies the selected volume level to the audio mixer.";

        [field: Space]
        public AudioMixer AudioMixer;

        [field: ReadOnly]
        [field: SerializeField]
        public int VoiceVolume { get; private set; }

        private const string VoiceVolumeReference = "voice_volume";
        private const string VoiceVolumeParameter = "voice";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            VoiceVolume = profile.Get<int>(reference = VoiceVolumeReference);

        public void Update() => 
            AudioMixer?.SetFloat(VoiceVolumeParameter, VoiceVolume.ToDecibelLevel());
    }
}