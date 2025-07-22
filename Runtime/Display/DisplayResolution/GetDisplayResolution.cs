using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetDisplayResolution : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component retrieves all available display resolutions from the system and populates the menu options.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred screen resolution.";

        public static string[] Options { get; private set; }

        public void Awake()
        {
            Initialize();
            SetDisplaySelection.OnDisplayIndexChanged += Initialize;
        }

        public void Initialize()
        {
            Options = new string[Screen.resolutions.Length];
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                Options[i] = $"{resolution.width}x{resolution.height}";
            }
            Options = Options.Reverse().ToArray();

            GetComponent<UIMenuOptionsDataConfigurator>().Options = Options;
        }
    }
}
