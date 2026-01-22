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

        /// <summary>
        /// Internal toggles for boot behavior.
        /// These are intentionally not public/serialized because SettingsBoot is designed to be auto-created.
        /// </summary>
        public static class Options
        {
            /// <summary>If true, SettingsBoot will create the persistent root if it doesn't exist.</summary>
            public static bool AutoCreateRoot = true;

            /// <summary>If true, SettingsBoot will ensure every discovered settings component exists under the persistent root.</summary>
            public static bool AutoInstantiateSettings = true;

            /// <summary>
            /// If true, each settings component will live on its own child GameObject under the persistent root.
            /// If false, all settings components will be attached to the root itself.
            /// </summary>
            public static bool CreateChildGameObjectPerSetting = true;
        }

        private void Awake()
        {
            if (_quitting) return;

            if (!Options.AutoCreateRoot && _root == null)
                return;

            var root = GetOrCreateRoot();

            // If this SettingsBoot isn't on the root, keep behavior single-owner and avoid duplicates.
            if (gameObject != root)
            {
                // Ensure settings are instantiated once even if multiple boots exist.
                if (Options.AutoInstantiateSettings)
                    EnsureAllSettings(root);

                Destroy(gameObject);
                return;
            }

            if (Options.AutoInstantiateSettings)
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
            var types = FindAllSettingsComponentTypes();

            var createChildPerSetting = Options.CreateChildGameObjectPerSetting;

            foreach (var type in types)
            {
                var targetHost = GetOrCreateHost(root, type, createChildPerSetting);

                // 1) Find an existing instance anywhere (including root/children/scenes).
#if UNITY_2023_1_OR_NEWER
                var existingInScene = UnityEngine.Object.FindFirstObjectByType(type) as MonoBehaviour;
#else
                var existingInScene = UnityEngine.Object.FindObjectOfType(type) as MonoBehaviour;
#endif

                // 2) If an instance exists, prefer it.
                if (existingInScene != null)
                {
                    // If it's already on the target host we're done.
                    if (existingInScene.gameObject != targetHost)
                    {
                        // Special case: when hosting directly on the root, we must not attempt to re-parent a component
                        // under its own GameObject (that would be a no-op and would not move the component).
                        // Instead, ensure the component exists on the root.
                        if (!createChildPerSetting)
                        {
                            if (targetHost.GetComponent(type) == null)
                                targetHost.AddComponent(type);
                        }
                        else
                        {
                            // If the component is on an object with multiple components (or user content), we avoid moving
                            // the whole GameObject; instead, we add a fresh component to our host.
                            // This keeps SettingsBoot from unexpectedly re-parenting user objects.
                            var existingGo = existingInScene.gameObject;
                            var canMoveWholeObject = existingGo.GetComponents<Component>().Length <= 2; // Transform + this component

                            if (!canMoveWholeObject)
                            {
                                if (targetHost.GetComponent(type) == null)
                                    targetHost.AddComponent(type);
                            }
                            else
                            {
                                existingInScene.transform.SetParent(targetHost.transform, worldPositionStays: false);
                            }
                        }
                    }
                }
                else
                {
                    // 3) None exist in the scene -> ensure one exists on our host.
                    if (targetHost.GetComponent(type) == null)
                        targetHost.AddComponent(type);
                }

                // 4) If there are duplicates under our settings root (root + children), destroy extras on our managed hosts.
                //    We only dedupe within the [Settings] hierarchy to avoid breaking intentionally duplicated scene setups.
                var inRootHierarchy = root.GetComponentsInChildren(type, includeInactive: true);
                var kept = false;
                foreach (Component c in inRootHierarchy)
                {
                    if (c == null)
                        continue;

                    if (!kept)
                    {
                        kept = true;
                        continue;
                    }

                    UnityEngine.Object.Destroy(c);
                }
            }
        }

        private static GameObject GetOrCreateHost(GameObject root, Type settingType, bool createChildPerSetting)
        {
            if (!createChildPerSetting)
                return root;

            // Ensure a dedicated host child exists: [Settings]/<TypeName>
            var hostTransform = root.transform.Find(settingType.Name);
            if (hostTransform != null)
                return hostTransform.gameObject;

            var go = new GameObject(settingType.Name);
            go.transform.SetParent(root.transform, worldPositionStays: false);
            return go;
        }

        private static IReadOnlyList<Type> FindAllSettingsComponentTypes()
        {
            var marker = typeof(ISettingsComponent);

            // Use predefined runtime assemblies for performance and consistency.
            var types = PredefinedAssemblyUtility.GetTypes(marker);

            // Filter to runtime-safe MonoBehaviours.
            return types
                .Where(t => t != null)
                .Where(t => !t.IsAbstract)
                .Where(t => typeof(MonoBehaviour).IsAssignableFrom(t))
                .Where(t => marker.IsAssignableFrom(t))
                .OrderBy(t => t.FullName, StringComparer.Ordinal)
                .ToArray();
        }

#if UNITY_INCLUDE_TESTS
        /// <summary>
        /// Test-only hook: re-run bootstrapping deterministically within the same play session.
        /// Needed because RuntimeInitializeOnLoadMethod(BeforeSceneLoad) only runs once per play session.
        /// </summary>
        public static void RefreshForTests()
        {
            if (_quitting) return;

            var root = GetOrCreateRoot();
            EnsureHasBootComponent(root);

            if (Options.AutoInstantiateSettings)
                EnsureAllSettings(root);
        }
#endif
    }
}
