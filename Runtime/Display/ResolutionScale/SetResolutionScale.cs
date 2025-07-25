using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEssentials
{
    public class SetResolutionScale : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the resolution scale based on the settings profile.\n" +
            "It allows dynamic resolution scaling if the resolution scale is below 100%.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public int ResolutionScale { get; private set; }

        private const string ResolutionScaleReference = "resolution_scale";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateResolutionScale(UIMenuProfile profile) =>
                ResolutionScale = profile.Get<int>(ResolutionScaleReference);

            UpdateResolutionScale(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == ResolutionScaleReference)
                    UpdateResolutionScale(profile);
            };
        }

        public void Update()
        {
            CameraProvider.Active?.SetDynamicResolution(ResolutionScale < 100);
            DynamicResolutionHandler.SetDynamicResScaler(() => ResolutionScale, 0);
        }
    }
}