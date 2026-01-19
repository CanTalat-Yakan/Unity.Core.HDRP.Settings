using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMasterVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the master volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the master volume setting and applies the selected volume level to the audio mixer.";

        [Space]
        public AudioMixer AudioMixer;

        public static int MasterVolume { get; private set; }
        private static string MasterVolumeReference { get; set; } = "master_volume";

        public override void InitializeGetter()
        {
            var configurator = gameObject.AddComponent<MenuSliderDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = MasterVolumeReference;
            configurator.MinValue = 0;
            configurator.MaxValue = 100;
            configurator.Default = 100;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(SettingsProfile profile, out string reference) =>
            MasterVolume = profile.Value.Get<int>(reference = MasterVolumeReference);

        private const string MasterVolumeParameter = "master";
        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(MasterVolumeParameter, MasterVolume.ToDecibelLevel());
    }
}