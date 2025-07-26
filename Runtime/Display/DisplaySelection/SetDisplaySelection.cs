using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplaySelection : SettingsMenuBase
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

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            DisplaySelection = profile.Get<int>(reference = DisplaySelectionReference);

        public static Action OnDisplayIndexChanged;

        private int _lastDisplaySelection = -1;
        public void Update()
        {
            if (_lastDisplaySelection != DisplaySelection)
                _lastDisplaySelection = DisplaySelection;
            else if (GetDisplaySelection.Dirty)
                GetDisplaySelection.Dirty = false;
            else return;

            if (Display.displays.Length > DisplaySelection && DisplaySelection >= 0)
                Display.displays[DisplaySelection].Activate();

            OnDisplayIndexChanged?.Invoke();

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