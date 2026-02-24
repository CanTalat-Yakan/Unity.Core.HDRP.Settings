using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsFilterMode : SettingsBase<FilterMode>
    {
        private const string Info =
            "Sets the filter mode for the camera's render texture.";

        protected override FilterMode Value { get; set; }
        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/FilterMode";

        public string[] Options { get; set; }

        public override void InitOptions() =>
            Options = Enum.GetNames(typeof(FilterMode));

        public override void InitDefinition() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(Info);
        
        public override void InitValue() =>
            Value = (FilterMode)GetProfileValue<int>();

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.FilterMode = Value;
        }

        [Console("settings.rendering.filterMode", Info)]
        private string ConsoleFillMode(int? value) =>
            $"FilterMode = {(FilterMode)GetOrSetProfileValue(value).Value}";
    }
}