using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(MenuOptionsDataConfigurator))]
    public class SettingsMicrophoneInput : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the microphone input based on the user's selection in the settings menu.\n" +
            "It listens for changes in the microphone input setting and applies the selected microphone to the audio system.";

        public static string MicrophoneInput { get; private set; }
        private static string[] MicrophoneInputOptions { get; set; }
        private static string MicrophoneInputReference { get; set; } = "microphoneInput";

        public override void InitializeGetter()
        {
            MicrophoneInputOptions = new string[Microphone.devices.Length + 1];
            MicrophoneInputOptions[0] = "Default";
            for (int i = 1; i < Microphone.devices.Length; i++)
            {
                var microphone = Microphone.devices[i];
                MicrophoneInputOptions[i] = $"Microphone {i}";
            }

            var configurator = gameObject.AddComponent<MenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = MicrophoneInputReference;
            configurator.Options = MicrophoneInputOptions;
            configurator.ConfigureMenuData();
        }
    }
}