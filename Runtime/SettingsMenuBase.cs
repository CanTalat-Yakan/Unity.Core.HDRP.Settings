using System;
using UnityEngine;

namespace UnityEssentials
{
    [DefaultExecutionOrder(-1009)]
    public class SettingsMenuBase : MonoBehaviour
    {
        [HideInInspector] public bool Dirty;
        [HideInInspector] public Action SetDirty;

        public static string SettingsMenuName { get; private set; } = "Settings";
        public static string SettingsProfileName { get; private set; } = "Settings";

        private string _reference;
        private SettingsProfile _profile;
        private Action _setter;

        public virtual void InitializeSetter(SettingsProfile profile, out string reference) { reference = string.Empty; }
        public virtual void InitializeGetter() { }
        public virtual void UpdateSettings() { }
        public virtual void BindAction(out Action source, out Action toBind) { source = null; toBind = null; }

        private void OnEnable()
        {
            BindAction(out var source, out var toAdd);
            source += toAdd;
        }

        private void OnDisable()
        {
            BindAction(out var source, out var toAdd);
            source -= toAdd;
        }

        public void Awake() =>
            InitializeGetter();

        public void Start()
        {
            _profile = new SettingsProfile(SettingsProfileName);
            _profile.GetOrLoad();

            _setter = () => InitializeSetter(_profile, out _reference); ;
            _setter.Invoke();

            SetDirty = () => Dirty = true;

            _profile.Value.OnValueChanged += (changedValueReference) =>
            {
                if (changedValueReference == _reference)
                    _setter?.Invoke();
            };
        }

        public void Update()
        {
            if (Dirty)
                InitializeGetter();
        }

        public void LateUpdate()
        {
            if (Dirty)
                _setter?.Invoke();

            UpdateSettings();

            Dirty = false;
        }
    }
}