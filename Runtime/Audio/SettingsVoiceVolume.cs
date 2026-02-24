using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsVoiceVolume : SettingsBase<int>
    {
        private const string Info =
            "Adjusts the volume level for voice audio within the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/VoiceVolume";

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 0, 200, 100, 100, "%")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public AudioMixer AudioMixer =>
            _audioMixer ??= AssetResolver.TryGet<AudioMixer>("UnityEssentials_AudioMixer", true);

        private AudioMixer _audioMixer;

        private const string VoiceVolumeParameter = "voice";

        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(VoiceVolumeParameter, Value.ToDecibelLevel());

        [Console("settings.audio.voiceVolume", Info)]
        private string ConsoleVoiceVolume(int? volume) =>
            $"VoiceVolume = {GetOrSetProfileValue(volume).Value}";
    }
}