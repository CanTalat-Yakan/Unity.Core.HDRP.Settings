using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetDisplaySelection : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component retrieves all available displays from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred display.";

        public static string[] Options { get; private set; }
        public static bool Dirty { get; set; }

        public override void InitializeGetter()
        {
            Options = new string[Display.displays.Length];
            for (int i = 0; i < Display.displays.Length; i++)
            {
                var display = Display.displays[i];
                Options[i] = $"Display {i + 1}";
            }

            GetComponent<UIMenuOptionsDataConfigurator>().Options = Options;

            Dirty = true;
        }

        public void OnEnable() => Display.onDisplaysUpdated += () => InitializeGetter();
        public void OnDisable() => Display.onDisplaysUpdated -= () => InitializeGetter();
    }
}