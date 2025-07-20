using UnityEngine;

namespace UnityEssentials
{
    [RequireComponent(typeof(UIMenuOptionsDataConfigurator))]
    public class GetAspectRatio : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component populates the aspect ratio options in the settings menu.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred aspect ratio.";

        public static string[] Options =
        {
            "Auto",
            "16:10",
            "16:10",
            "21:9",
            "32:9",
            "9:16",
            "10:16",
            "1:1",
            "4:3",
            "2.39:1",
            "2.35:1"
        };

        public void Awake() =>
            GetComponent<UIMenuOptionsDataConfigurator>().Options = Options;
    }
}
