using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnityEssentials.Tests
{
    public class SettingsBootTests
    {
        [UnityTest]
        public System.Collections.IEnumerator CreatesPersistentRootAndInstantiatesSettings()
        {
            // Let RuntimeInitializeOnLoadMethod run.
            yield return null;

            var root = GameObject.Find("[Settings]");
            Assert.NotNull(root, "Expected Settings root to exist");

            // At least the boot should exist.
            Assert.NotNull(root.GetComponent<SettingsBoot>(), "Expected SettingsBoot on root");

            // Should have at least one SettingsBase-derived component in this package.
            var settings = root.GetComponents<MonoBehaviour>().Where(m => m is ISettingsComponent).ToArray();
            Assert.IsTrue(settings.Length > 0, "Expected one or more settings components to be instantiated");

            // Root should be parentless (DontDestroyOnLoad scene root object).
            Assert.IsTrue(root.transform.parent == null);
        }
    }
}
