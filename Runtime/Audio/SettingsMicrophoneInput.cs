using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class SettingsMicrophoneInput : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the microphone input based on the user's selection in the settings menu.\n" +
            "It listens for changes in the microphone input setting and applies the selected microphone to the audio system.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField]
        public string MicrophoneInput { get; private set; }
        public static string[] MicrophoneInputOptions { get; private set; }
        public static string MicrophoneInputReference { get; private set; } = "microphoneInput";

        public override void InitializeGetter()
        {
            MicrophoneInputOptions = new string[Microphone.devices.Length + 1];
            MicrophoneInputOptions[0] = "Default";
            for (int i = 1; i < Microphone.devices.Length; i++)
            {
                var microphone = Microphone.devices[i];
                MicrophoneInputOptions[i] = $"Microphone {i}";
            }

            var configurator = gameObject.AddComponent<UIMenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = MicrophoneInputReference;
            configurator.Options = MicrophoneInputOptions;
            configurator.ConfigureMenuData();
        }
    }
}