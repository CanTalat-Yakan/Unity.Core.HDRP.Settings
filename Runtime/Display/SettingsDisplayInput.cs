using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayInput : SettingsBase, ISettingsBase<int>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display input based on the user's choice in the settings menu.\n" +
            "It listens for changes in the display input setting and activates the corresponding display.\n\n" +
            "This component retrieves all available displays from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred display.";

        public static event Action Changed;
        private static void RaiseChanged() => Changed?.Invoke();

        protected override string ProfileName => "Display";
        protected override string Reference => "DisplayInput";

        public int Value { get; set; }
        public string[] Options { get; set; }
        public bool Reverse => false;
        public int Default => 0;

        protected override void SubscribeActions() =>
            Display.onDisplaysUpdated += MarkDirty;

        protected override void UnsubscribeActions() =>
            Display.onDisplaysUpdated -= MarkDirty;

        private bool _displayOptionsUpdated;
        public override void InitOptions()
        {
            Options = new string[Display.displays.Length + 1];
            Options[0] = "Default";
            for (int i = 0; i < Display.displays.Length; i++)
                Options[i + 1] = $"Display {i + 1}";

            _displayOptionsUpdated = true;
        }

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

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
            
            RaiseChanged();
        }
    }
}