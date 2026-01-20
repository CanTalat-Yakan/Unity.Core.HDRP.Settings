using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsMicrophoneInput : SettingsBase, ISettingsBase<int>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the microphone input based on the user's selection in the settings menu.\n" +
            "It listens for changes in the microphone input setting and applies the selected microphone to the audio system.";

        public static event Action OnChanged;
        private static void RaiseChanged() => OnChanged?.Invoke();

        protected override string ProfileName => "Audio";
        protected override string Reference => "MicrophoneInput";

        public int Value { get; set; }
        public string[] Options { get; set; }
        public bool Reverse => false;
        public int Default => 0;

        private bool _microphoneOptionsUpdated;
        public override void InitOptions()
        {
            Options = new string[Microphone.devices.Length + 1];
            Options[0] = "Default";
            for (int i = 0; i < Microphone.devices.Length; i++)
                Options[i + 1] = $"Microphone {i + 1}";

            _microphoneOptionsUpdated = true;
        }

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        private int _lastMicrophoneInput = -1;
        public override void UpdateSettings()
        {
            if (_lastMicrophoneInput != Value)
                _lastMicrophoneInput = Value;
            else if (_microphoneOptionsUpdated)
                _microphoneOptionsUpdated = false;
            else return;

            RaiseChanged();
        }
    }
}