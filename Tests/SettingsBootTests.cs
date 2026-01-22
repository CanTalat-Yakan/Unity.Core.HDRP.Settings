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

            // Settings may be hosted on the root or on child GameObjects under it.
            var settings = root.GetComponentsInChildren<MonoBehaviour>(includeInactive: true)
                .Where(m => m is ISettingsComponent)
                .ToArray();
            Assert.IsTrue(settings.Length > 0, "Expected one or more settings components to be instantiated");

            // Root should be parentless (DontDestroyOnLoad scene root object).
            Assert.IsTrue(root.transform.parent == null);
        }

        [Test]
        public void PredefinedAssemblyUtility_Finds_TestSettingsComponent_Type()
        {
            var types = PredefinedAssemblyUtility.GetTypes(typeof(ISettingsComponent));
            Assert.IsNotNull(types);

            // This is the key regression guard: if we can't discover this type, SettingsBoot can't instantiate it.
            Assert.IsTrue(types.Contains(typeof(TestSettingsComponent)),
                "Expected PredefinedAssemblyUtilities.GetTypes(ISettingsComponent) to include TestSettingsComponent. " +
                "If this fails, the assembly scanning rules are too restrictive for asmdef-based projects.");
        }

        [UnityTest]
        public System.Collections.IEnumerator Creates_Child_GameObject_For_TestSettingsComponent()
        {
            yield return null;

            var root = GameObject.Find("[Settings]");
            Assert.NotNull(root);

            // Expect the child host to exist if CreateChildGameObjectPerSetting is enabled (default true).
            var host = root.transform.Find("Settings/" + nameof(TestSettingsComponent));
            Assert.NotNull(host, "Expected a per-setting child host GameObject under [Settings]");

            var component = host.GetComponent<TestSettingsComponent>();
            Assert.NotNull(component, "Expected TestSettingsComponent to be attached to its host child");
        }

        [UnityTest]
        public System.Collections.IEnumerator Creates_Settings_Container_Under_Root()
        {
            yield return null;

            var root = GameObject.Find("[Settings]");
            Assert.NotNull(root);

            var settingsContainer = root.transform.Find("Settings");
            Assert.NotNull(settingsContainer, "Expected [Settings]/Settings container GameObject to exist");

            var testHost = settingsContainer.Find(nameof(TestSettingsComponent));
            Assert.NotNull(testHost, "Expected [Settings]/Settings/<TypeName> host to exist for TestSettingsComponent");
        }
    }

#if UNITY_INCLUDE_TESTS
    /// <summary>
    /// Minimal concrete settings component used by tests to validate SettingsBoot discovery/instantiation.
    /// </summary>
    public sealed class TestSettingsComponent : SettingsBase<int>
    {
        // Keep it simple: no file IO impact beyond whatever SettingsProfile/Definition already does.
        protected override int Value { get; set; }
        protected override string FileName => "__unityessentials_settingsboot_tests";
        protected override string Reference => "test.settings.component";
    }
#endif
}