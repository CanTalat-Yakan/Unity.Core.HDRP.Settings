using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMusicVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the music volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the music volume setting and applies the selected volume level to the audio mixer.";

        [Space]
        public AudioMixer AudioMixer;

        public static int MusicVolume { get; private set; }
        private static string MusicVolumeReference { get; set; } = "music_volume";

        public override void InitializeGetter()
        {
            var configurator = gameObject.AddComponent<MenuSliderDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = MusicVolumeReference;
            configurator.MinValue = 0;
            configurator.MaxValue = 200;
            configurator.Default = 100;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(MenuProfile profile, out string reference) =>
            MusicVolume = profile.Get<int>(reference = MusicVolumeReference);

        private const string MusicVolumeParameter = "music";
        public override void UpdateSettings() => 
            AudioMixer?.SetFloat(MusicVolumeParameter, MusicVolume.ToDecibelLevel());
    }
}