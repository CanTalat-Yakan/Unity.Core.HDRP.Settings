namespace UnityEssentials
{
    public class SettingsGlobalRefreshRate : SettingsBase<int>
    {
        private const string Info =
            "Sets the global refresh rate for all cameras that use the GlobalRefreshRate component. " +
            "Performance optimization, as scheduling updates at a lower refresh rate can improve CPU frame times while still displaying at a higher fps. " +
            "If set to 0, the limiter is disabled (unlimited).";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Display";
        protected override string Reference => "Settings/Display/GlobalRefreshRate";

        public float MinValue => 0;
        public float MaxValue => 1000;
        public float Default => 0;

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 0, 1000, 1, 0, "FPS")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public override void UpdateSettings() =>
            // 0 (or less) means unlimited / disabled limiter.
            GlobalRefreshRate.SetTarget(Value);

        [Console("settings.display.globalRefreshRate",Info)]
        private string ConsoleGlobalRefreshRate(int? fps) =>
            $"GlobalRefreshRate = {GetOrSetProfileValue(fps).Value}";
    }
}