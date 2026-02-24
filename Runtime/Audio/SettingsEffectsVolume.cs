using UnityEngine.Audio;

namespace UnityEssentials
{
    public class SettingsEffectsVolume : SettingsBase<int>
    {
        private const string Info =
            "Adjusts the volume level for sound effects within the application.";

        protected override int Value { get; set; }
        protected override string FileName => "Settings/Audio";
        protected override string Reference => "Settings/Audio/EffectsVolume";

        public override void InitDefinition() =>
            Definition.SetIntSlider(Reference, 0, 200, 100, 100, "%")
                .SetTooltip(Info);

        public override void InitValue() =>
            Value = GetProfileValue<int>();

        public AudioMixer AudioMixer =>
            _audioMixer ??= AssetResolver.TryGet<AudioMixer>("UnityEssentials_AudioMixer", true);

        private AudioMixer _audioMixer;

        private const string EffectsVolumeParameter = "effects";

        public override void UpdateSettings() =>
            AudioMixer?.SetFloat(EffectsVolumeParameter, Value.ToDecibelLevel());

        [Console("settings.audio.effectsVolume", Info)]
        private string ConsoleEffectsVolume(int? volume) =>
            $"EffectsVolume = {GetOrSetProfileValue(volume).Value}";
    }
}