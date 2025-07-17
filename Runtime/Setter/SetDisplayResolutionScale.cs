using UnityEngine;

namespace UnityEssentials
{
    public class SetDisplayResolutionScale : MonoBehaviour
    {
        private UIMenuSliderData _data;

        private int _resolutionScale;
        private int _antiAliasing;

        private const string MenuName = "Settings";
        private const string ProfileName = "Settings";
        private const string ResolutionScaleReference = "resolution_scale";
        private const string AntiAliasingReference = "anti_aliasing";

        public void Awake()
        {
            if (!UIMenu.TryGetData(MenuName, ResolutionScaleReference, out _data))
                return;

            if (!UIMenu.TryGetProfile(ProfileName, out var profile))
                return;

            _resolutionScale = profile.Get<int>(_data);
        }

        public void Update()
        {
            bool allowDynamicScaling = _resolutionScale < 100;

            int usingSTPantiAliasing = 4;
            if (_antiAliasing == usingSTPantiAliasing)
                allowDynamicScaling = true;

            //_cameraData.allowDynamicResolution = allowDynamicScaling;
        }
    }
}
