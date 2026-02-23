using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMusicVolume : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Adjusts the volume level for music audio within the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/MusicVolume";

        public override void InitMetadata() =>
            Definition.SetIntSlider(Reference, 0, 200, 100, 100, "%")
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

        public AudioMixer AudioMixer =>
            _audioMixer ??= AssetResolver.TryGet<AudioMixer>("UnityEssentials_AudioMixer", true);

        private AudioMixer _audioMixer;

        private const string MusicVolumeParameter = "music";

        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(MusicVolumeParameter, Value.ToDecibelLevel());

        [Console("settings.audio.musicVolume", "Gets/sets music volume (0-200).")]
        private string ConsoleMusicVolume(int? value)
        {
            if (value == null) return $"MusicVolume = {Profile.Value.Get<int>(Reference)}";
            Profile.Value.Set(Reference, value.Value);
            return $"MusicVolume = {value.Value}";
        }
    }
}