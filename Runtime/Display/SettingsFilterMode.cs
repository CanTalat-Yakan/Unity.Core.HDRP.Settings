using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsFilterMode : SettingsBase, ISettingsBase<FilterMode>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the filter mode based on the user's selection in the settings menu.\n" +
            "It listens for changes in the filter mode setting and applies the selected FilterMode to the camera render texture handler.\n\n" +
            "This component populates the filter mode options in the settings menu by retrieving all available FilterMode enum names.\n" +
            "It is intended for use with UIMenuOptionsDataConfigurator to allow users to select their preferred texture filtering mode.";

        protected override string ProfileName => "Display";
        protected override string Reference => "FilterMode";

        public FilterMode Value { get; set; }
        public string[] Options { get; set; }
        public bool Reverse => false;
        public int Default => 0;

        public override void InitOptions() =>
            Options = Enum.GetNames(typeof(FilterMode));

        public override void InitValue(SettingsProfile profile) =>
            Value = (FilterMode)profile.Value.Get<int>(Reference);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.FilterMode = Value;
        }
    }
}