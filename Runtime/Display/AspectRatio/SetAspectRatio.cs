using UnityEngine;

namespace UnityEssentials
{
    public class SetAspectRatio : MonoBehaviour
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the render resolution based on the user's selection in the settings menu.\n" +
            "It listens for changes in the render resolution setting and applies the selected resolution to the camera render texture handler.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public Vector2 AspectRatio { get; private set; }

        private const string AspectRatioReference = "render_resolution";

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            void UpdateAspectRatio(UIMenuProfile profile) =>
                AspectRatio = GetRenderResolution.Options[profile.Get<int>(AspectRatioReference)].ExtractFromString(':');

            UpdateAspectRatio(profile);
            profile.OnValueChanged += (reference) =>
            {
                if (reference == AspectRatioReference)
                    UpdateAspectRatio(profile);
            };
        }

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Main?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public void Update()
        {
            if (RenderTextureHandler == null)
                return;

            var aspectRatio = AspectRatio.x / AspectRatio.y;
            if (AspectRatio.x <= 0 || AspectRatio.y <= 0)
                aspectRatio = 0;

            RenderTextureHandler.Settings.AspectRatio = aspectRatio;
        }
    }
}
