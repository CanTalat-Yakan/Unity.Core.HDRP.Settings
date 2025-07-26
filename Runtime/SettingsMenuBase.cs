using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsMenuBase : MonoBehaviour
    {
        [HideInInspector] public string Reference;
        [HideInInspector] public Action UpdateValue;

        public virtual void InitializeSetter(UIMenuProfile profile, out string reference) { reference = string.Empty; }

        public virtual void InitializeGetter() { }

        public void Start()
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            UpdateValue = () => { InitializeSetter(profile, out Reference); };
            UpdateValue.Invoke();

            profile.OnValueChanged += (changedValueReference) =>
            {
                if (changedValueReference == Reference)
                    UpdateValue.Invoke();
            };
        }

        public void Awake() =>
            InitializeGetter();
    }
}