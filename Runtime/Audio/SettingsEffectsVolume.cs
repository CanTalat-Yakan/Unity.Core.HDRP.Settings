using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsEffectsVolume : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the effects volume for the audio mixer based on the user's selection in the settings menu.\n" +
            "It listens for changes in the effects volume setting and applies the selected volume level to the audio mixer.";

        [Space]
        public AudioMixer AudioMixer;

        public static int EffectsVolume { get; private set; }
        private static string EffectsVolumeReference { get; set; } = "effects_volume";

        public override void InitializeGetter()
        {
            var configurator = gameObject.AddComponent<MenuSliderDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = EffectsVolumeReference;
            configurator.MinValue = 0;
            configurator.MaxValue = 200;
            configurator.Default = 100;
            configurator.ConfigureMenuData();
        }

        public override void InitializeSetter(MenuProfile profile, out string reference) =>
            EffectsVolume = profile.Get<int>(reference = EffectsVolumeReference);

        private const string EffectsVolumeParameter = "effects";
        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(EffectsVolumeParameter, EffectsVolume.ToDecibelLevel());
    }
}