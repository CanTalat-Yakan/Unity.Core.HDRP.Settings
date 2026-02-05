using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityEssentials
{
    /// <summary>
    /// Bootstraps all runtime settings components (derived from <see cref="SettingsBase{T}"/>)
    /// under the <see cref="GlobalSingleton{T}"/> root.
    ///
    /// This component auto-exists as a GlobalSingleton (no manual scene setup required).
    /// </summary>
    public sealed class SettingsHost : GlobalSingleton<SettingsHost>
    {
        private void OnEnable() =>
            EnsureInitialized(gameObject);

        private static void EnsureInitialized(GameObject root)
        {
            var types = FindAllSettingsComponentTypes();

            foreach (var type in types)
            {
                // Always instantiate each setting component on its own child GameObject: <SettingsHost>/<TypeName>
                var settingsObject = GetOrCreateSettingsObject(root, type);
                var existingInScene = FindFirstObjectByType(type) as MonoBehaviour;
                if (existingInScene != null)
                {
                    if (existingInScene.gameObject != settingsObject)
                        // Don’t re-parent potentially user-authored GameObjects; instead ensure we have our own instance.
                        if (settingsObject.GetComponent(type) == null)
                            settingsObject.AddComponent(type);
                }
                else if (settingsObject.GetComponent(type) == null)
                    settingsObject.AddComponent(type);

                // Dedupe within our managed hierarchy.
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

                    Destroy(c);
                }
            }
        }

        private static GameObject GetOrCreateSettingsObject(GameObject root, Type settingType)
        {
            var settingTransform = root.transform.Find(settingType.Name);
            if (settingTransform != null)
                return settingTransform.gameObject;

            var go = new GameObject(settingType.Name);
            go.transform.SetParent(root.transform, worldPositionStays: false);
            return go;
        }

        private static IReadOnlyList<Type> FindAllSettingsComponentTypes()
        {
            var marker = typeof(ISettingsComponent);
            var types = PredefinedAssemblies.GetTypes(marker);

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