using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetVerticalSynchronization : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component populates the VSync options in the settings menu.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred VSync setting.";

        public static string[] Options { get; private set; } =
        {
            "Don't Sync",
            "Every V Blank",
            "Every Second V Blank"
        };

        public override void InitializeGetter() =>
            GetComponent<UIMenuOptionsDataConfigurator>().Options = Options;
    }
}