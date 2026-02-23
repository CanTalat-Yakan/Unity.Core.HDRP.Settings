using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsFilterMode : SettingsBase<FilterMode>
    {
        [Info, SerializeField]
        private string _info =
            "Sets the filter mode for the camera's render texture.";

        protected override FilterMode Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/FilterMode";

        public string[] Options { get; set; }

        public override void InitOptions() =>
            Options = Enum.GetNames(typeof(FilterMode));

        public override void InitMetadata() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(_info);
        
        public override void InitValue() =>
            Value = (FilterMode)Profile.Value.Get<int>(Reference);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.FilterMode = Value;
        }

        [Console("settings.display.filterMode", "Gets/sets filter mode index (FilterMode enum int).")]
        private string ConsoleFilterMode(int? value)
        {
            if (value == null) return $"FilterMode = {(FilterMode)Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, value.Value);
            return $"FilterMode = {(FilterMode)value.Value}";
        }
    }
}