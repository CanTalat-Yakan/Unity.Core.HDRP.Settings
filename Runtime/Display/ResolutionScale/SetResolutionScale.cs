using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SetResolutionScale : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the resolution scale based on the settings profile.\n" +
            "It allows dynamic resolution scaling if the resolution scale is below 100%.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public int ResolutionScale { get; private set; }

        private const string ResolutionScaleReference = "resolution_scale";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            ResolutionScale = profile.Get<int>(reference = ResolutionScaleReference);

        public void Update()
        {
            CameraProvider.Active?.SetDynamicResolution(ResolutionScale < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => ResolutionScale, 0);
        }
    }
}