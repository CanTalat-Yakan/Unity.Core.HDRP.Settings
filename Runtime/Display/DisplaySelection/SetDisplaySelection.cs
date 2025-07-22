using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplaySelection : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the active display based on the user's selection from the menu.\n" +
            "It allows users to choose which display to use for rendering.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public int DisplaySelection { get; private set; }

        private const string DisplaySelectionReference = "display_selection";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateDisplaySelection(UIMenuProfile profile) =>
                DisplaySelection = profile.Get<int>(DisplaySelectionReference);

            UpdateDisplaySelection(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == DisplaySelectionReference)
                    UpdateDisplaySelection(profile);
            };
        }

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
