using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetWindowMode : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info = 
            "This component populates the window mode options in the settings menu by retrieving all available FullScreenMode enum names.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred window mode.";

        public static string[] Options { get; private set; }

        public void Awake()
        {
            Options = Enum.GetNames(typeof(FullScreenMode))
                .Select(mode => ObjectNames.NicifyVariableName(mode))
                .ToArray();

            GetComponent<UIMenuOptionsDataConfigurator>().Options = Options;
        }
    }
}
