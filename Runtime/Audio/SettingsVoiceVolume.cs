using UnityEngine;
using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsVoiceVolume : SettingsBase<int>
    {
        [Info, SerializeField] private string _info =
            "Adjusts the volume level for voice audio within the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/VoiceVolume";

        public override void InitMetadata() =>
            Definition.SetIntSlider(Reference, 0, 200, 100, 100, "%")
                .SetTooltip(_info);

        public override void InitValue() =>
            Value = Profile.Value.Get<int>(Reference);

        public AudioMixer AudioMixer =>
            _audioMixer ??= AssetResolver.TryGet<AudioMixer>("UnityEssentials_AudioMixer", true);

        private AudioMixer _audioMixer;

        private const string VoiceVolumeParameter = "voice";

        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(VoiceVolumeParameter, Value.ToDecibelLevel());

        [Console("settings.audio.voiceVolume", "Gets/sets voice volume (0-200).")]
        private string ConsoleVoiceVolume(int? volume) =>
            $"VoiceVolume = {GetOrSetProfileValue(volume).Value}";
    }
}