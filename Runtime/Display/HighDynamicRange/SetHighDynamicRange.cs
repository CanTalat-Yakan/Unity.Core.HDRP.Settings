using UnityEngine;

namespace UnityEssentials
{
    public class SetHighDynamicRange : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public bool HighDynamicRange { get; private set; }

        private const string HighDynamicRangeReference = "hdr";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            HighDynamicRange = profile.Get<bool>(reference = HighDynamicRangeReference);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public void Update()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.HighDynamicRange = HighDynamicRange;
        }
    }
}