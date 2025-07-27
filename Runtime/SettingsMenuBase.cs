using System;
using UnityEngine;

namespace UnityEssentials
{
    [DefaultExecutionOrder(-1009)]
    public class SettingsMenuBase : MonoBehaviour
    {
        private string _reference;
        private Action _callback;
        private UIMenuProfile _profile;

        public virtual void InitializeSetter(UIMenuProfile profile, out string reference) { reference = string.Empty; }

        public virtual void InitializeGetter() { }

        public void InvokeUpdateValueCallback() =>
            _callback?.Invoke();

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out _profile))
                return;

            _callback = () => InitializeSetter(_profile, out _reference);;
            _callback.Invoke();

            _profile.OnValueChanged += (changedValueReference) =>
            {
                if (changedValueReference == _reference) 
                    InvokeUpdateValueCallback();
            };
        }

        public void Awake() =>
            InitializeGetter();
    }
}