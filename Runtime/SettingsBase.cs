using System;
using System.Collections;
using UnityEngine;

namespace UnityEssentials
{
    public abstract class SettingsBase : MonoBehaviour
    {
        protected abstract string ProfileName { get; }
        protected abstract string Reference { get; }

        private bool _isDirty;

        private SettingsProfile _profile;
        private Action _setter;

        private Action<string> _onProfileChanged;
        private Coroutine _applyCoroutine;

        private bool _initialized;

        public virtual void InitValue(SettingsProfile profile) { }

        public virtual void InitOptions() { }

        public virtual void UpdateSettings() { }

        protected virtual void SubscribeActions() { }

        protected virtual void UnsubscribeActions() { }

        protected void MarkDirty()
        {
            _isDirty = true;
            EnsureApplyScheduled();
        }

        private void EnsureApplyScheduled()
        {
            if (!isActiveAndEnabled) return;
            if (_applyCoroutine != null) return;
            _applyCoroutine = StartCoroutine(ApplyAtEndOfFrame());
        }

        private IEnumerator ApplyAtEndOfFrame()
        {
            yield return null; // coalesce multiple changes this frame
            _applyCoroutine = null;
            if (!isActiveAndEnabled) yield break;
            if (!_isDirty) yield break;
            _isDirty = false;
            InitOptions();
            Apply();
        }

        protected virtual void Awake()
        {
            InitOptions();

            // Prepare profile + handler early so event subscriptions in OnEnable are safe.
            _profile = SettingsProfile.GetOrCreate(ProfileName);
            _profile.GetValue();

            _setter = () =>
            {
                InitValue(_profile);
                UpdateSettings();
            };

            _onProfileChanged = (changedValueReference) =>
            {
                if (changedValueReference == Reference)
                    Apply();
            };

            _profile.Value.OnChanged += _onProfileChanged;
            _initialized = true;
        }

        protected virtual void OnEnable()
        {
            SubscribeActions();
            // Apply once when enabled (covers initial start + re-enables).
            if (_initialized) Apply();
            // In case something marked dirty while disabled, apply when re-enabled.
            EnsureApplyScheduled();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeActions();
            if (_applyCoroutine != null)
            {
                StopCoroutine(_applyCoroutine);
                _applyCoroutine = null;
            }
        }

        protected virtual void OnDestroy()
        {
            if (_profile != null && _onProfileChanged != null)
                _profile.Value.OnChanged -= _onProfileChanged;
        }

        protected void Apply()
        {
            if (_profile == null) return;
            _setter?.Invoke();
        }
    }
}