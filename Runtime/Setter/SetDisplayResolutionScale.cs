using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SetDisplayResolutionScale : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the display resolution scale based on the settings profile.\n" +
            "It allows dynamic resolution scaling if the resolution scale is below 100%.";

        [field: Space]
        [field: SerializeField, ReadOnly] public int ResolutionScale { get; private set; }

        public void Awake()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            profile.OnValueChanged += (reference) =>
            {
                const string ResolutionScaleReference = "resolution_scale";
                if (reference.Equals(ResolutionScaleReference))
                    ResolutionScale = profile.Get<int>(ResolutionScaleReference);
            };
        }

        public void UpdateResolutionScale()
        {
            CameraProvider.Main.allowDynamicResolution = ResolutionScale < 100;
            DynamicResolutionHandler.SetDynamicResScaler(() => ResolutionScale, DynamicResScalePolicyType.ReturnsPercentage);
        }
    }
}
