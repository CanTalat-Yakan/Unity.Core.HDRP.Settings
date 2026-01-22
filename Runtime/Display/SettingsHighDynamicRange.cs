using UnityEngine;

namespace UnityEssentials
{
    public class SettingsHighDynamicRange : SettingsBase<bool>
    {
        [Info, SerializeField]
        private string _info = 
            "Enables or disables High Dynamic Range (HDR) rendering for the camera's render texture.";

        protected override bool Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/HDR";

        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetToggle(Reference)
                .SetTooltip(_info);
        
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