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

        // Child naming to keep hierarchy tidy and avoid collisions with user objects.
        private const string ChildPrefix = "Settings/";

        private static bool _quitting;
        private static GameObject _root;

        [Tooltip("If enabled, SettingsBoot will create the persistent root if it doesn't exist.")]
        public bool AutoCreateRoot = true;

        [Tooltip("If enabled, SettingsBoot will ensure every discovered settings component exists under the persistent root.")]
        public bool AutoInstantiateSettings = true;

        [Tooltip("If enabled, each settings component will live on its own child GameObject under the persistent root (recommended for clarity in the hierarchy).")]
        public bool CreateChildGameObjectPerSetting = true;

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
            var types = FindAllSettingsComponentTypes();

            // Options should be driven by the boot component living on the root.
            // If missing (e.g., temporarily during creation), default to the safer/clearer behavior.
            var boot = root.GetComponent<SettingsBoot>();
            var createChildPerSetting = boot == null || boot.CreateChildGameObjectPerSetting;

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
                        // If the component is on an object with multiple components (or user content), we avoid moving
                        // the whole GameObject; instead, we add a fresh component to our host.
                        // This keeps SettingsBoot from unexpectedly re-parenting user objects.
                        var existingGo = existingInScene.gameObject;
                        var canMoveWholeObject = existingGo.GetComponents<Component>().Length <= 2; // Transform + this component

                        if (createChildPerSetting && !canMoveWholeObject)
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
                foreach (var inst in inRootHierarchy)
                {
                    if (inst is not Component c) continue;

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

            var childName = ChildPrefix + settingType.Name;
            var existing = root.transform.Find(childName);
            if (existing != null)
                return existing.gameObject;

            var go = new GameObject(childName);
            go.transform.SetParent(root.transform, worldPositionStays: false);
            return go;
        }

        // Keep the old signature for any existing internal callers (if any).
        private static GameObject GetOrCreateHost(GameObject root, Type settingType) =>
            GetOrCreateHost(root, settingType, createChildPerSetting: true);

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
    }
}
