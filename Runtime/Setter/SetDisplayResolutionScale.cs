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

        private const string ResolutionScaleReference = "resolution_scale";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            UpdateResolutionScale(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == ResolutionScaleReference)
                    UpdateResolutionScale(profile);
            };
        }

        public void Update()
        {
            CameraProvider.Main?.SetDynamicResolution(ResolutionScale < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => ResolutionScale, 0);
        }

        public void UpdateResolutionScale(UIMenuProfile profile) =>
            ResolutionScale = profile.Get<int>(ResolutionScaleReference);
    }
}