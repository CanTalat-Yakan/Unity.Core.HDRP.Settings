namespace UnityEssentials
{
    public class SettingsHighDynamicRange : SettingsBase<bool>
    {
        private const string Info =
            "Enables or disables High Dynamic Range (HDR) rendering for the camera's render texture.";

        protected override bool Value { get; set; }
        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/HDR";

        public override void InitDefinition() =>
            Definition.SetToggle(Reference)
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<bool>();

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??=
            CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();

        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.HighDynamicRange = Value;
        }

        [Console("settings.rendering.hdr", Info)]
        private string ConsoleHdr(bool? enabled) =>
            $"HDR = {GetOrSetProfileValue(enabled).Value}";
    }
}