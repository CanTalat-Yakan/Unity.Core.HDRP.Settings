namespace UnityEssentials
{
    public interface ISettingsBase<T>
    {
        public T Value { get; set; }
    }
    
    public interface ISettingsOptionsConfiguration
    {
        public string[] Options { get; set; }
        public bool Reverse { get; }
        public int Default { get; }
    }
    
    public interface ISettingsSliderConfiguration
    {
        public float MinValue { get; }
        public float MaxValue { get; }
        public float Default { get; }
    }
    
    public interface ISettingsBoolConfiguration
    {
        public bool Default { get; }
    }
}