using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetDisplaySelection : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component retrieves all available displays from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred display.";

        public static string[] Options { get; private set; }

        public void Awake() =>
            InitializeGetter();

        public UIMenuGenerator Generator => _generator ??= GetGenerator();
        private UIMenuGenerator _generator;

        public void OnEnable() => Display.onDisplaysUpdated += () =>
        {
            InitializeGetter();
            Generator?.Redraw?.Invoke();
        };

        public void OnDisable() => Display.onDisplaysUpdated -= () =>
        {
            InitializeGetter();
            Generator?.Redraw?.Invoke();
        };

        public void InitializeGetter()
        {
            Options = new string[Display.displays.Length];
            for (int i = 0; i < Display.displays.Length; i++)
            {
                var display = Display.displays[i];
                Options[i] = $"Display {i + 1}";
            }

            GetComponent<UIMenuOptionsDataConfigurator>().Options = Options;
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