using System;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsRenderResolution : SettingsBase<Vector2Int>
    {
        [Info, SerializeField] private string _info =
            "Specifies a resolution for rendering, which can differ from the screen resolution. " +
            "Performance optimization, as rendering at a lower resolution can improve GPU frame times while still displaying at a higher resolution.";

        protected override Vector2Int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/RenderResolution";

        public string[] Options { get; set; }

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

        public override void InitMetadata() =>
            Definition.SetOptions(Reference, Options)
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Options[Profile.Value.Get<int>(Reference)]
                .ExtractVector2FromString('x').ToVector2Int();

        protected override void SubscribeActions() =>
            SettingsDisplayInput.OnChanged += MarkDirty;

        protected override void UnsubscribeActions() =>
            SettingsDisplayInput.OnChanged -= MarkDirty;

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??=
            CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();

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

        [Console("settings.display.renderResolution", "Gets/sets render resolution option index.")]
        private string ConsoleRenderResolution(int? index)
        {
            if (index == null) return $"RenderResolution index = {Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, index.Value);
            return $"RenderResolution index = {Options[index.Value]}";
        }
    }
}