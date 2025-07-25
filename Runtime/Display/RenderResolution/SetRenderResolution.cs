using UnityEngine;

namespace UnityEssentials
{
    public class SetRenderResolution : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public Vector2Int RenderResolution { get; private set; }

        private const string RenderResolutionReference = "render_resolution";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateRenderResolution(UIMenuProfile profile) =>
                RenderResolution = GetRenderResolution.Options[profile.Get<int>(RenderResolutionReference)].ExtractFromString('x').ToVector2Int();

            UpdateRenderResolution(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == RenderResolutionReference)
                    UpdateRenderResolution(profile);
            };
            SetDisplaySelection.OnDisplayIndexChanged += () => UpdateRenderResolution(profile);
        }

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Main?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public void Update()
        {
            if (RenderTextureHandler == null)
                return;

            var renderResolution = RenderResolution;
            if(RenderResolution.x <= 0 || RenderResolution.y <= 0)
                renderResolution = new(Screen.currentResolution.width, Screen.currentResolution.height);

            RenderTextureHandler.Settings.RenderWidth = renderResolution.x;
            RenderTextureHandler.Settings.RenderHeight = renderResolution.y;
        }
    }
}
