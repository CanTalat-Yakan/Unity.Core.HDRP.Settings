using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Bootstraps all runtime settings components (derived from <see cref="SettingsBase{T}"/>)
    /// into a single persistent root that survives from initial boot to application quit.
    /// 
    /// Place this in your boot scene, or rely on the auto-create hook (see below).
    /// </summary>
    [DefaultExecutionOrder(-10_000)]
    public class SettingsBoot : MonoBehaviour
    {
        private const string RootObjectName = "[Settings]";

        private static bool _quitting;
        private static GameObject _root;

        [Tooltip("If enabled, SettingsBoot will create the persistent root if it doesn't exist.")]
        public bool AutoCreateRoot = true;

        [Tooltip("If enabled, SettingsBoot will ensure every discovered settings component exists under the persistent root.")]
        public bool AutoInstantiateSettings = true;

        private void Awake()
        {
            if (_quitting) return;

            if (!AutoCreateRoot && _root == null)
                return;

            var root = GetOrCreateRoot();

            // If this SettingsBoot isn't on the root, keep behavior single-owner and avoid duplicates.
            if (gameObject != root)
            {
                // Ensure settings are instantiated once even if multiple boots exist.
                if (AutoInstantiateSettings)
                    EnsureAllSettings(root);

                Destroy(gameObject);
                return;
            }

            if (AutoInstantiateSettings)
                EnsureAllSettings(root);
        }

        private void OnApplicationQuit() => _quitting = true;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void AutoCreateBeforeSceneLoad()
        {
            if (_quitting) return;

            // In Enter Play Mode Options (domain reload disabled), static fields may persist.
            // If the object got destroyed between sessions, rebind the reference.
            if (_root == null)
            {
                var existing = GameObject.Find(RootObjectName);
                if (existing != null)
                    _root = existing;
            }

            // If a root already exists (e.g., in a boot scene), don't create a new one.
            if (_root != null)
            {
                DontDestroyOnLoad(_root);
                EnsureHasBootComponent(_root);
                return;
            }

            var root = new GameObject(RootObjectName);
            _root = root;
            DontDestroyOnLoad(root);
            EnsureHasBootComponent(root);
        }

        private static void EnsureHasBootComponent(GameObject root)
        {
            // Ensure the boot exists on the root so settings will get instantiated.
            if (!root.TryGetComponent<SettingsBoot>(out _))
                root.AddComponent<SettingsBoot>();
        }

        private static GameObject GetOrCreateRoot()
        {
            // In Enter Play Mode Options (domain reload disabled), a cached reference might point to a destroyed object.
            if (_root == null)
            {
                var existing = GameObject.Find(RootObjectName);
                if (existing != null)
                    _root = existing;
            }

            if (_root != null)
            {
                DontDestroyOnLoad(_root);
                return _root;
            }

            _root = new GameObject(RootObjectName);
            DontDestroyOnLoad(_root);
            return _root;
        }

        private static void EnsureAllSettings(GameObject root)
        {
            // Add missing settings components and move any scene instances under the root.
            var types = FindAllSettingsComponentTypes();

            // Make sure the root has exactly one of each settings component type.
            foreach (var type in types)
            {
                // 1) If one exists somewhere else in the scene, prefer it and re-parent.
#if UNITY_2023_1_OR_NEWER
                var existingInScene = UnityEngine.Object.FindFirstObjectByType(type) as MonoBehaviour;
#else
                var existingInScene = UnityEngine.Object.FindObjectOfType(type) as MonoBehaviour;
#endif
                if (existingInScene != null)
                {
                    // If it's not already on the root, move it.
                    if (existingInScene.gameObject != root)
                        existingInScene.transform.SetParent(root.transform, worldPositionStays: false);
                }
                else
                {
                    // 2) None exist in the scene -> ensure one exists on the root.
                    if (root.GetComponent(type) == null)
                        root.AddComponent(type);
                }

                // 3) If there are duplicates on the root, keep the first and remove the rest.
                //    This can happen if multiple boots ran or a scene included components on the root prefab.
                var rootInstances = root.GetComponents(type);
                for (var i = 1; i < rootInstances.Length; i++)
                {
                    if (rootInstances[i] is Component c)
                        UnityEngine.Object.Destroy(c);
                }
            }
        }

        private static IReadOnlyList<Type> FindAllSettingsComponentTypes()
        {
            var marker = typeof(ISettingsComponent);

            // Use predefined runtime assemblies for performance and consistency.
            var types = PredefinedAssemblyUtilities.GetTypes(marker);

            // Filter to runtime-safe MonoBehaviours.
            return types
                .Where(t => t != null)
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t))
                .Where(t => marker.IsAssignableFrom(t))
                .OrderBy(t => t.FullName, StringComparer.Ordinal)
                .ToArray();
        }
    }
}
