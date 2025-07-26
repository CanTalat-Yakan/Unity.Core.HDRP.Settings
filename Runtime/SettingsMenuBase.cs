using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsMenuBase : MonoBehaviour
    {
        [HideInInspector] public string Reference;
        [HideInInspector] public Action Callback;

        public virtual void InitializeSetter(UIMenuProfile profile, out string reference) { reference = string.Empty; }

        public virtual void InitializeGetter() { }

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            Callback = () => { InitializeSetter(profile, out Reference); };
            Callback.Invoke();

            profile.OnValueChanged += (changedValueReference) =>
            {
                if (changedValueReference == Reference)
                    InitializeSetter(profile, out _);
            };
        }

        public void Awake() =>
            InitializeGetter();
    }
}