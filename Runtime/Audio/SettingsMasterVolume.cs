using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsMasterVolume : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Adjusts the overall master volume level within the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/MasterVolume";

        public override void InitMetadata() =>
            Definition.SetIntSlider(Reference, 0, 100, 100, 100, "%")
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

        public AudioMixer AudioMixer =>
            _audioMixer ??= AssetResolver.TryGet<AudioMixer>("UnityEssentials_AudioMixer", true);

        private AudioMixer _audioMixer;

        private const string MasterVolumeParameter = "master";

        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(MasterVolumeParameter, Value.ToDecibelLevel());

        [Console("settings.audio.masterVolume", "Gets/sets master volume (0-100).")]
        private string ConsoleMasterVolume(int? volume) =>
            $"MasterVolume = {GetOrSetProfileValue(volume).Value}";
    }
}