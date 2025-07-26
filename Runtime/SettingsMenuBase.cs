using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsMenuBase : MonoBehaviour
    {
        private string _reference;
        private Action _updateValueCallback;

        public virtual void InitializeSetter(UIMenuProfile profile, out string reference) { reference = string.Empty; }

        public virtual void InitializeGetter() { }

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            _updateValueCallback = () => InitializeSetter(profile, out _reference);;
            _updateValueCallback.Invoke();

            profile.OnValueChanged += (changedValueReference) =>
            {
                if (changedValueReference == _reference) 
                    InvokeUpdateValueCallback();
            };
        }

        public void Awake() =>
            InitializeGetter();

        public void InvokeUpdateValueCallback() =>
            _updateValueCallback?.Invoke();
    }
}