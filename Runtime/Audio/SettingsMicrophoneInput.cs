using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsMicrophoneInput : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Selects the microphone input device for audio capture. 'Default' uses the system's default microphone.";

        public static event Action OnChanged;
        private static void RaiseChanged() => OnChanged?.Invoke();

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/MicrophoneInput";

        public string[] Options { get; set; }

        private bool _microphoneOptionsUpdated;

        public override void InitOptions()
        {
            Options = new string[Microphone.devices.Length + 1];
            Options[0] = "Default";
            for (int i = 0; i < Microphone.devices.Length; i++)
                Options[i + 1] = $"Microphone {i + 1}";

            _microphoneOptionsUpdated = true;
        }

        public override void InitMetadata() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

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

        [Console("settings.audio.microphoneInput", "Gets/sets microphone input option index (0=Default).")]
        private string ConsoleMicrophoneInput(int? index) =>
            $"MicrophoneInput index = {GetOrSetProfileValue(index).Value}";
    }
}