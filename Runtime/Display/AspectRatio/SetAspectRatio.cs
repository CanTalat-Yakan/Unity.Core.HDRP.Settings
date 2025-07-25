using UnityEngine;

namespace UnityEssentials
{
    public class SetAspectRatio : SettingsMenuSetterBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the aspect ratio of the camera render texture based on the settings profile.\n" +
            "It listens for changes in the aspect ratio setting and applies the selected aspect ratio to the camera render texture handler.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] public Vector2 AspectRatio { get; private set; }

        private const string AspectRatioReference = "aspect_ratio";

        public void Start() =>
            InitializeSetter(AspectRatioReference, (profile) => 
                AspectRatio = GetAspectRatio.Options[profile.Get<int>(AspectRatioReference)].ExtractFromString(':'));

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
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