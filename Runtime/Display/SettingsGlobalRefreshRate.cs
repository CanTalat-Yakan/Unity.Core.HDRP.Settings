using UnityEngine;

namespace UnityEssentials
{
    public class SettingsGlobalRefreshRate : SettingsBase<int>
    {
        [Info, SerializeField]
        private string _info =
            "Sets the global refresh rate for all cameras that use the GlobalRefreshRate component. " +
            "Performance optimization, as scheduling updates at a lower refresh rate can improve CPU frame times while still displaying at a higher fps. " +
            "If set to 0, it will match the current screen refresh rate.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/GlobalRefreshRate";

        public float MinValue => 0;
        public float MaxValue => 1000;
        public float Default => 1000;
        
        public override void InitMetadata(SettingsDefinition definition) =>
            definition.SetIntSlider(Reference, 0, 1000, 1, 1000, "FPS")
                .SetTooltip(_info);
     
        public override void InitValue(SettingsProfile profile) =>
            Value = profile.Value.Get<int>(Reference);

        public override void UpdateSettings()
        {
            if (Value <= 0)
            {
                var ratio = Screen.currentResolution.refreshRateRatio;
                Value = Mathf.CeilToInt(ratio.numerator / ratio.denominator);
            }

            GlobalRefreshRate.SetTarget(Value);
        }
    }
}