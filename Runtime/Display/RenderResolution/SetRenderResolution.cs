using UnityEngine;

namespace UnityEssentials
{
    public class SetRenderResolution : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public Vector2Int RenderResolution { get; private set; }

        private const string RenderResolutionReference = "render_resolution";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            RenderResolution = GetRenderResolution.Options[profile.Get<int>(reference = RenderResolutionReference)]
                .ExtractVector2FromString('x').ToVector2Int();

        private void OnEnable() => SetDisplaySelection.OnDisplaySelectionChanged += InvokeUpdateValueCallback;
        private void OnDisable() => SetDisplaySelection.OnDisplaySelectionChanged -= InvokeUpdateValueCallback;

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public void Update()
        {
            if (RenderTextureHandler == null)
                return;

            if (RenderResolution.x <= 0 || RenderResolution.y <= 0)
                RenderResolution = new(Screen.currentResolution.width, Screen.currentResolution.height);

            RenderTextureHandler.Settings.RenderWidth = RenderResolution.x;
            RenderTextureHandler.Settings.RenderHeight = RenderResolution.y;
        }
    }
}