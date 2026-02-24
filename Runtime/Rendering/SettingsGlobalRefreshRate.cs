namespace UnityEssentials
{
    public class SettingsGlobalRefreshRate : SettingsBase<int>
    {
        private const string Info =
            "Sets the global refresh rate for all cameras that use the GlobalRefreshRate component.\n" +
            "If set to 0, the limiter is disabled (unlimited).";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Rendering";
        protected override string Reference => "Settings/Rendering/GlobalRefreshRate";

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

        [Console("settings.rendering.globalRefreshRate",Info)]
        private string ConsoleGlobalRefreshRate(int? fps) =>
            $"GlobalRefreshRate = {GetOrSetProfileValue(fps).Value}";
    }
}