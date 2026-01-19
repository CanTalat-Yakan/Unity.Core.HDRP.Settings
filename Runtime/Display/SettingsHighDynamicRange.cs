using UnityEngine;

namespace UnityEssentials
{
    public class SettingsHighDynamicRange : SettingsMenuBase, ISettingsBase<bool>
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        public bool Value { get; set; }
        public string Reference => "hdr";

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = profile.Value.Get<bool>(reference = Reference);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.HighDynamicRange = Value;
        }
    }
}