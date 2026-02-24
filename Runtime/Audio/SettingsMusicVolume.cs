using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMusicVolume : SettingsBase<int>
    {
        private const string Info =
            "Adjusts the volume level for music audio within the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/MusicVolume";

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 0, 200, 100, 100, "%")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public AudioMixer AudioMixer =>
            _audioMixer ??= AssetResolver.TryGet<AudioMixer>("UnityEssentials_AudioMixer", true);

        private AudioMixer _audioMixer;

        private const string MusicVolumeParameter = "music";

        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(MusicVolumeParameter, Value.ToDecibelLevel());

        [Console("settings.audio.musicVolume", Info)]
        private string ConsoleMusicVolume(int? volume) =>
            $"MusicVolume = {GetOrSetProfileValue(volume).Value}";
    }
}