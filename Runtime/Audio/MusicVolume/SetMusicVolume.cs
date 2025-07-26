using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SetMusicVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the music volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the music volume setting and applies the selected volume level to the audio mixer.";

        [field: Space]
        public AudioMixer AudioMixer;

        [field: ReadOnly]
        [field: SerializeField]
        public int MusicVolume { get; private set; }

        private const string MusicVolumeReference = "music_volume";
        private const string MusicVolumeParameter = "music";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            MusicVolume = profile.Get<int>(reference = MusicVolumeReference);

        public void Update() => 
            AudioMixer?.SetFloat(MusicVolumeParameter, MusicVolume.ToDecibelLevel());
    }
}