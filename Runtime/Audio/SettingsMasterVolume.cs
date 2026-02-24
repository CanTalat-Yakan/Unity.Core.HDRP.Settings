using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMasterVolume : SettingsBase<int>
    {
        private const string Info =
            "Adjusts the overall master volume level within the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/MasterVolume";

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 0, 100, 100, 100, "%")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public AudioMixer AudioMixer =>
            _audioMixer ??= AssetResolver.TryGet<AudioMixer>("UnityEssentials_AudioMixer", true);

        private AudioMixer _audioMixer;

        private const string MasterVolumeParameter = "master";

        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(MasterVolumeParameter, Value.ToDecibelLevel());

        [Console("settings.audio.masterVolume", Info)]
        private string ConsoleMasterVolume(int? volume) =>
            $"MasterVolume = {GetOrSetProfileValue(volume).Value}";
    }
}