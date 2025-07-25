using System;
using UnityEngine;

namespace UnityEssentials
{
    public class SettingsMenuSetterBase : MonoBehaviour
    {
        public void InitializeSetter(string reference, Action<UIMenuProfile> update, Action callback = null)
        {
            if (!UIMenu.TryGetProfile("Settings", out var profile))
                return;

            update(profile);

            profile.OnValueChanged += (changedValueReference) =>
            {
                if (changedValueReference == reference)
                    update(profile);
            };

            if (callback != null)
                callback += () => update(profile);
        }
    }
}