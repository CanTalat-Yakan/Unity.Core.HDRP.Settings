using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SetMasterVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the master volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the master volume setting and applies the selected volume level to the audio mixer.";

        [field: Space]
        public AudioMixer AudioMixer;

        [field: ReadOnly]
        [field: SerializeField]
        public int MasterVolume { get; private set; }

        private const string MasterVolumeReference = "master_volume";
        private const string MasterVolumeParameter = "master";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            MasterVolume = profile.Get<int>(reference = MasterVolumeReference);

        public void Update() => 
            AudioMixer?.SetFloat(MasterVolumeParameter, MasterVolume.ToDecibelLevel());
    }
}