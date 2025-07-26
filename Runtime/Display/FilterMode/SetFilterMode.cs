using UnityEngine;

namespace UnityEssentials
{
    public class SetFilterMode : SettingsMenuBase
    {
        [Info]
        [SerializeField]
        private string _info =
            "This component sets the filter mode based on the user's selection in the settings menu.\n" +
            "It listens for changes in the filter mode setting and applies the selected FilterMode to the camera render texture handler.";

        [field: Space]
        [field: ReadOnly]
        [field: SerializeField] 
        public int FilterMode { get; private set; }

        private const string FilterModeReference = "filter_mode";

        public override void InitializeSetter(UIMenuProfile profile, out string reference) =>
            FilterMode = profile.Get<int>(reference = FilterModeReference);

        public CameraRenderTextureHandler RenderTextureHandler => _renderTextureHandler ??= CameraProvider.Active?.GetComponent<CameraRenderTextureHandler>();
        private CameraRenderTextureHandler _renderTextureHandler;

        public void Update()
        {
            if (RenderTextureHandler == null)
                return;

            RenderTextureHandler.Settings.FilterMode = (FilterMode)FilterMode;
        }
    }
}