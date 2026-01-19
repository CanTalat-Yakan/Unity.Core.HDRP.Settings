namespace UnityEssentials
{
    public interface ISettingsBase<T>
    {
        public T Value { get; set; }
        public string Reference { get; }
    }
    
    public interface ISettingsOptionsConfiguration
    {
        public string[] Options { get; set; }
        public bool Reverse { get; }
    }
    
    public interface ISettingsSliderConfiguration
    {
        public float MinValue { get; }
        public float MaxValue { get; }
        public float Default { get; }
    }
}