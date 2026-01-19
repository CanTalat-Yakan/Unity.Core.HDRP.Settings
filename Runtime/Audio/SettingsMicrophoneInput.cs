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

        public int Value { get; set; }
        public string Reference => "microphoneInput";
        
        public string[] Options { get; set; }
        public bool Reverse => false;
        
        [HideInInspector] public static Action OnMicrophoneInputChanged { get; private set; }

        private bool _microphoneOptionsUpdated;
        public override void InitOptions()
        {
            Options = new string[Microphone.devices.Length + 1];
            Options[0] = "Default";
            for (int i = 1; i < Microphone.devices.Length; i++)
            {
                var microphone = Microphone.devices[i];
                Options[i] = $"Microphone {i}";
            }

            _microphoneOptionsUpdated = true;
        }
        
        private int _lastMicrophoneInput = -1;
        public override void UpdateSettings()
        {
            if (_lastMicrophoneInput != Value)
                _lastMicrophoneInput = Value;
            else if (_microphoneOptionsUpdated)
                _microphoneOptionsUpdated = false;
            else return;

            OnMicrophoneInputChanged?.Invoke();
            //Redraw UI;
        }
    }
}