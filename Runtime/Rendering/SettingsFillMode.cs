using System;

namespace UnityEssentials
{
    public class SettingsFillMode : SettingsBase<FillMode>
    {
        private const string Info =
            "Sets the fill mode for the camera's render texture, which determines how the rendered image is scaled to fit the screen.\n" + 
            "This can affect performance and visual quality, especially when using dynamic resolution scaling.";

        protected override FillMode Value { get; set; }
        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/FillMode";

        public string[] Options { get; set; }

        public override void InitOptions() =>
            Options = Enum.GetNames(typeof(FillMode));

        public override void InitDefinition() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = (FillMode)GetProfileValue<int>();

        protected override void SubscribeActions() =>
            SettingsDisplayInput.OnChanged += MarkDirty;

        protected override void UnsubscribeActions() =>
            SettingsDisplayInput.OnChanged -= MarkDirty;

        public CameraRenderTextureHandler RenderTextureHandler => 
            _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.FillMode = Value;
        }

        [Console("settings.rendering.fillMode", Info)]
        private string ConsoleFillMode(int? value) =>
            $"FillMode = {(FillMode)GetOrSetProfileValue(value).Value}";
    }
}