using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SetEffectsVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the effects volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the effects volume setting and applies the selected volume level to the audio mixer.";

        [field: Space]
        public AudioMixer AudioMixer;

        [field: ReadOnly]
        [field: SerializeField]
        public int EffectsVolume { get; private set; }

        private const string EffectsVolumeReference = "effects_volume"; 
        private const string EffectsVolumeParameter = "effects";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            EffectsVolume = profile.Get<int>(reference = EffectsVolumeReference);

        public void Update() => 
            AudioMixer?.SetFloat(EffectsVolumeParameter, EffectsVolume.ToDecibelLevel());
    }
}