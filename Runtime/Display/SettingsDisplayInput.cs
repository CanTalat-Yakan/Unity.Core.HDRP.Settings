using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayInput : SettingsMenuBase, ISettingsBase<int>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display input based on the user's choice in the settings menu.\n" +
            "It listens for changes in the display input setting and activates the corresponding display.\n\n" +
            "This component retrieves all available displays from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred display.";

        public int Value { get; set; }
        public string Reference => "display_input";
        
        public string[] Options { get; set; }
        public bool Reverse => false;
        
        [HideInInspector] public static Action OnDisplayInputChanged { get; private set; }

        private bool _initialized;
        private bool _displayOptionsUpdated;
        public override void InitOptions()
        {
            if (!_initialized)
            {
                Display.onDisplaysUpdated += () => SetDirty();
                _initialized = true;
            }

            Options = new string[Display.displays.Length + 1];
            Options[0] = "Default";
            for (int i = 0; i < Display.displays.Length; i++)
            {
                var display = Display.displays[i];
                Options[i + 1] = $"Display {i + 1}";
            }

            _displayOptionsUpdated = true;
        }

        public override void BindAction(out Action source, out Action toBind) =>
            (source, toBind) = (OnDisplayInputChanged, SetDirty);

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<int>(reference = Reference);

        private int _lastDisplayInput = -1;
        public override void UpdateSettings()
        {
            if (_lastDisplayInput != Value)
                _lastDisplayInput = Value;
            else if (_displayOptionsUpdated)
                _displayOptionsUpdated = false;
            else return;

            if (Display.displays.Length > Value && Value >= 0)
                Display.displays[Value].Activate();

            OnDisplayInputChanged?.Invoke();
            //Redraw UI;
        }
    }
}