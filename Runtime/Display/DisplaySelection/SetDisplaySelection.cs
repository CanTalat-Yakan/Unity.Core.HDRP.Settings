using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplaySelection : SettingsMenuSetterBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display selection based on the user's choice in the settings menu.\n" +
            "It listens for changes in the display selection setting and activates the corresponding display.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public int DisplaySelection { get; private set; }

        private const string DisplaySelectionReference = "display_selection";

        public void Start() =>
            InitializeSetter(DisplaySelectionReference, (profile) =>
                DisplaySelection = profile.Get<int>(DisplaySelectionReference));

        public UIMenuGenerator Generator => _generator ??= GetGenerator();
        private UIMenuGenerator _generator;

        public static Action OnDisplayIndexChanged;

        private int _lastDisplaySelection = -1;
        public void Update()
        {
            if (_lastDisplaySelection == DisplaySelection)
                return;

            if (Display.displays.Length > DisplaySelection && DisplaySelection >= 0)
                Display.displays[DisplaySelection].Activate();

            OnDisplayIndexChanged?.Invoke();

            Generator?.Redraw?.Invoke();
            _lastDisplaySelection = DisplaySelection;
        }

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