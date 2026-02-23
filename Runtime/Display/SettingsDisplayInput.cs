using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayInput : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Selects which display to use for rendering when multiple displays are connected.";

        public static event Action OnChanged;
        private static void RaiseChanged() => OnChanged?.Invoke();

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/DisplayInput";

        public string[] Options { get; set; }

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

        public override void InitMetadata() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

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

        [Console("settings.display.displayInput", "Gets/sets display index (0=Default).")]
        private string ConsoleDisplayInput(int? index) =>
            $"DisplayInput index = {Options[GetOrSetProfileValue(index).Value]}";
    }
}