using System;
using System.Collections;
using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Non-generic marker interface for all settings components.
    /// </summary>
    public interface ISettingsComponent
    {
    }

    public abstract class SettingsBase<T> : MonoBehaviour, ISettingsComponent
    {
        protected abstract T Value { get; set; }
        protected abstract string FileName { get; }
        protected abstract string Reference { get; }

        protected SettingsDefinition Definition { get; private set; }
        protected SettingsProfile Profile { get; private set; }

        private bool _isDirty;

        private Action _setter;

        private Action<string> _onProfileChanged;
        private Coroutine _applyCoroutine;

        private bool _initialized;

        public virtual void InitOptions()
        {
        }

        public virtual void InitMetadata()
        {
        }

        public virtual void InitValue()
        {
        }

        public virtual void UpdateSettings()
        {
        }

        protected virtual void SubscribeActions()
        {
        }

        protected virtual void UnsubscribeActions()
        {
        }

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

            Definition = SettingsDefinition.GetOrCreate(FileName);
            Definition.GetValue();

            Profile = SettingsProfile.GetOrCreate(FileName);
            Profile.GetValue();

            _setter = () =>
            {
                InitMetadata();
                InitValue();
                UpdateSettings();
                Definition.SaveIfDirty();
                Profile.SaveIfDirty();
            };

            _onProfileChanged = (changedValueReference) =>
            {
                if (changedValueReference == Reference)
                    Apply();
            };

            Profile.Value.OnChanged += _onProfileChanged;
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
            if (Profile != null && _onProfileChanged != null)
                Profile.Value.OnChanged -= _onProfileChanged;
        }

        protected void Apply()
        {
            if (Profile == null) return;
            _setter?.Invoke();
        }

        protected TValue GetProfileValue<TValue>() =>
            Profile.Value.Get<TValue>(Reference);

        protected TValue SetProfileValue<TValue>(TValue value)
        {
            Profile.Value.Set(Reference, value);
            return value;
        }

        protected TValue GetOrSetProfileValue<TValue>(TValue value)
        {
            if (value == null)
                return GetProfileValue<TValue>();

            SetProfileValue(value);
            return value;
        }
    }
}