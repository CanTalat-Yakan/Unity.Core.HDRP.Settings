using UnityEngine;

namespace UnityEssentials
{
    public class SettingsHighDynamicRange : SettingsBase, ISettingsBase<bool>, ISettingsBoolConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        protected override string ProfileName => "Display";
        protected override string Reference => "HDR";

        public bool Value { get; set; }
        public bool Default => false;

        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<bool>(Reference);

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