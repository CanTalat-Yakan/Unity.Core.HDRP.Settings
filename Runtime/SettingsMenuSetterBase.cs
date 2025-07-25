using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsMenuSetterBase : MonoBehaviour
    {
        public void InitializeSetter(string reference, Action<UIMenuProfile> update, params Action[] callbacks)
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            update(profile);

            profile.OnValueChanged += (changedValueReference) =>
            {
                if (changedValueReference == reference)
                    update(profile);
            };

            foreach (var callback in callbacks)
                if (callback is Action action)
                    action += () => update(profile);
        }
    }
}