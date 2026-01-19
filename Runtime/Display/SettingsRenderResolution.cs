using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderResolution : SettingsBase, ISettingsBase<Vector2Int>, ISettingsOptionsConfiguration
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        public Vector2Int Value { get; set; }
        public string Reference => "render_resolution";

        public string[] Options { get; set; }
        public bool Reverse { get; }

        public override void InitOptions()
        {
            Options = new string[Screen.resolutions.Length + 1];
            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = Screen.resolutions[i];
                Options[i] = $"{resolution.width}x{resolution.height}";
            }
            Options[^1] = "Native";
            Options = Options.Reverse().ToArray();
        }

        public override void InitValue(SettingsProfile profile, out string reference) =>
            Value = Options[profile.Value.Get<int>(reference = Reference)]
                .ExtractVector2FromString('x').ToVector2Int();

        public override void BindAction(out Action source, out Action toBind) =>
            (source, toBind) = (SettingsDisplayInput.OnDisplayInputChanged, SetDirty);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public override void UpdateSettings()
        {
            if (RenderTextureHandler == null)
                return;

            if (Value.x <= 0 || Value.y <= 0)
                Value = new(Screen.currentResolution.width, Screen.currentResolution.height);

            RenderTextureHandler.Settings.RenderWidth = Value.x;
            RenderTextureHandler.Settings.RenderHeight = Value.y;
        }
    }
}