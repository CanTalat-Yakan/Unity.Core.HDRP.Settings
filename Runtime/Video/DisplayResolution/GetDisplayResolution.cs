using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetDisplayResolution : MonoBehaviour
    {
        [Info]
        [SerializeField] private string _info =
            "This component retrieves all available display resolutions from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred screen resolution.";

        public void Awake()
        {
            var displayResolution = new string[Screen.resolutions.Length];
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                displayResolution[i] = $"{resolution.width}x{resolution.height}";
            }

            GetComponent<UIMenuOptionsDataConfigurator>().Options = displayResolution;
            Debug.Log("Options Updated");
        }
    }
}
