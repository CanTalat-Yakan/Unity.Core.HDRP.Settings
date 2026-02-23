using UnityEngine;

namespace UnityEssentials
{
    public class SettingsGlobalRefreshRate : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Sets the global refresh rate for all cameras that use the GlobalRefreshRate component. " +
            "Performance optimization, as scheduling updates at a lower refresh rate can improve CPU frame times while still displaying at a higher fps. " +
            "If set to 0, the limiter is disabled (unlimited).";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/GlobalRefreshRate";

        public float MinValue => 0;
        public float MaxValue => 1000;
        public float Default => 0;

        public override void InitMetadata() =>
            Definition.SetIntSlider(Reference, 0, 1000, 1, 0, "FPS")
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

        public override void UpdateSettings() =>
            // 0 (or less) means unlimited / disabled limiter.
            GlobalRefreshRate.SetTarget(Value);

        [Console("settings.display.globalRefreshRate",
            "Gets/sets global refresh rate target FPS (<=0 disables limiter).")]
        private string ConsoleGlobalRefreshRate(int? fps) =>
            $"GlobalRefreshRate = {GetOrSetProfileValue(fps).Value}";
    }
}