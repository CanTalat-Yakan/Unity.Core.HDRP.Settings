using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsVoiceVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the voice volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the voice volume setting and applies the selected volume level to the audio mixer.";

        [Space]
        public AudioMixer AudioMixer;

        [field: Space]
        [field: Space]
        [field: ReadOnly]
        [field: SerializeField]
        public int VoiceVolume { get; private set; }
        public static string VoiceVolumeReference { get; private set; } = "voice_volume";

        public override void InitializeGetter()
        {
            var configurator = gameObject.AddComponent<UIMenuSliderDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = VoiceVolumeReference;
            configurator.MinValue = 0;
            configurator.MaxValue = 100;
            configurator.Default = 100;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            VoiceVolume = profile.Get<int>(reference = VoiceVolumeReference);

        private const string VoiceVolumeParameter = "voice";
        public override void UpdateSettings() => 
            AudioMixer?.SetFloat(VoiceVolumeParameter, VoiceVolume.ToDecibelLevel());
    }
}