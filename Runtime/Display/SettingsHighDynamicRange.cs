using UnityEngine;

namespace UnityEssentials
{
    public class SettingsHighDynamicRange : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        public static bool HighDynamicRange { get; private set; }
        private static string HighDynamicRangeReference { get; set; } = "hdr";

        public override void InitializeSetter(SettingsProfile profile, out string reference) =>
            HighDynamicRange = profile.Value.Get<bool>(reference = HighDynamicRangeReference);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.HighDynamicRange = HighDynamicRange;
        }
    }
}