using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsDisplayInput : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display input based on the user's choice in the settings menu.\n" +
            "It listens for changes in the display input setting and activates the corresponding display.\n\n" +
            "This component retrieves all available displays from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred display.";

        public static int DisplayInput { get; private set; }
        private static string[] DisplayInputOptions { get; set; }
        private static string DisplayInputReference { get; set; } = "display_input";
        [HideInInspector] public static Action OnDisplayInputChanged { get; private set; }

        private bool _initialized;
        private bool _displayOptionsUpdated;
        public override void InitializeGetter()
        {
            if (!_initialized)
            {
                Display.onDisplaysUpdated += () => SetDirty();
                _initialized = true;
            }

            DisplayInputOptions = new string[Display.displays.Length + 1];
            DisplayInputOptions[0] = "Default";
            for (int i = 1; i < Display.displays.Length; i++)
            {
                var display = Display.displays[i];
                DisplayInputOptions[i] = $"Display {i}";
            }

            var configurator = gameObject.AddComponent<UIMenuOptionsDataConfigurator>();
            configurator.MenuName = SettingsMenuName;
            configurator.DataReference = DisplayInputReference;
            configurator.Options = DisplayInputOptions;
            configurator.ConfigureMenuData();

            _displayOptionsUpdated = true;
        }

        public override void BindAction(out Action source, out Action toBind) =>
            (source, toBind) = (OnDisplayInputChanged, SetDirty);

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            DisplayInput = profile.Get<int>(reference = DisplayInputReference);

        private int _lastDisplayInput = -1;
        public override void UpdateSettings()
        {
            if (_lastDisplayInput != DisplayInput)
                _lastDisplayInput = DisplayInput;
            else if (_displayOptionsUpdated)
                _displayOptionsUpdated = false;
            else return;

            if (Display.displays.Length > DisplayInput && DisplayInput >= 0)
                Display.displays[DisplayInput].Activate();

            OnDisplayInputChanged?.Invoke();
            Generator?.Redraw?.Invoke();
        }

        public UIMenuGenerator Generator => _generator ??= GetGenerator();
        private UIMenuGenerator _generator;

        private UIMenuGenerator GetGenerator()
        {
            if (_generator != null)
                return _generator;
            if (UIMenu.RegisteredMenus.TryGetValue("Settings", out var menu))
                return _generator = menu.Generator;
            return null;
        }
    }
}