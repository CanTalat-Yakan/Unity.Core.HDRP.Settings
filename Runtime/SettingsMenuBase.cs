using System;
using UnityEngine;

namespace UnityEssentials
{
    [DefaultExecutionOrder(-1009)]
    public abstract class SettingsMenuBase : MonoBehaviour
    {
        [HideInInspector] public bool Dirty;
        [HideInInspector] public Action SetDirty;

        private string _reference;
        private SettingsProfile _profile;
        private Action _setter;

        public virtual void InitValue(SettingsProfile profile, out string reference) { reference = string.Empty; }
        public virtual void InitOptions() { }
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
            InitOptions();

        public void Start()
        {
            const string settingsProfileName = "HDRP_Settings";
            _profile = SettingsProfileFactory.Create(settingsProfileName);
            _profile.GetOrLoad();

            _setter = () => InitValue(_profile, out _reference); ;
            _setter.Invoke();

            SetDirty = () => Dirty = true;

            _profile.Value.OnChanged += (changedValueReference) =>
            {
                if (changedValueReference == _reference)
                    _setter?.Invoke();
            };
        }

        public void Update()
        {
            if (Dirty)
                InitOptions();
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