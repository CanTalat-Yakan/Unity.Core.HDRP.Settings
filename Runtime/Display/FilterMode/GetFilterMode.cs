using System;
using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetFilterMode : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component populates the filter mode options in the settings menu by retrieving all available FilterMode enum names.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred texture filtering mode.";

        public static string[] Options { get; private set; }

        public override void InitializeGetter()
        {
            Options = Enum.GetNames(typeof(FilterMode));

            GetComponent<UIMenuOptionsDataConfigurator>().Options = Options;
        }
    }
}