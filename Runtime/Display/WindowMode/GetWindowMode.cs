using System;
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

        public void Awake()
        {
            var fullScreenMode = Enum.GetNames(typeof(FullScreenMode));

            GetComponent<UIMenuOptionsDataConfigurator>().Options = fullScreenMode;
        }
    }
}
