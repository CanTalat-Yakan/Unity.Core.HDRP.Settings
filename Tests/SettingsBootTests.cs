using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace UnityEssentials.Tests
{
    public class SettingsBootTests
    {

        [TearDown]
        public void Cleanup_Test_Profile_And_Definition_Files()
        {
#if UNITY_INCLUDE_TESTS
            const string testFileName = "__unityessentials_settingsboot_tests";
            
            // Best-effort cleanup: remove disk artifacts created by SettingsBase<T>.Awake() during these tests.
            // This keeps the project folder clean and avoids test order coupling via persisted data.
            try { SettingsProfile.GetOrCreate(testFileName).DeleteFile(); } catch { /* ignore */ }
            try { SettingsDefinition.GetOrCreate(testFileName).DeleteFile(); } catch { /* ignore */ }
            // Also delete legacy/alternate names if any test changed the filename.
            // (No-op if missing.)
            try { SettingsProfile.GetOrCreate().SaveIfDirty(); } catch { /* ignore */ }
#endif
        }

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
            Assert.IsTrue(types.Contains(typeof(SettingsTestComponent)),
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
            var host = root.transform.Find(nameof(SettingsTestComponent));
            Assert.NotNull(host, "Expected a per-setting child host GameObject under [Settings]");

            var component = host.GetComponent<SettingsTestComponent>();
            Assert.NotNull(component, "Expected TestSettingsComponent to be attached to its host child");
        }

        [UnityTest]
        public System.Collections.IEnumerator Does_Not_Create_Settings_Container_Under_Root()
        {
            yield return null;

            var root = GameObject.Find("[Settings]");
            Assert.NotNull(root);

            var settingsContainer = root.transform.Find("Settings");
            Assert.IsNull(settingsContainer, "Did not expect a redundant [Settings]/Settings container GameObject");

            // Host should be directly under the root.
            var testHost = root.transform.Find(nameof(SettingsTestComponent));
            Assert.NotNull(testHost);
            Assert.AreEqual(root.transform, testHost.parent);
        }

        [UnityTest]
        public System.Collections.IEnumerator Creates_On_Root_When_CreateChildGameObjectPerSetting_Is_False()
        {
            var previous = SettingsBoot.Options.CreateChildGameObjectPerSetting;
            SettingsBoot.Options.CreateChildGameObjectPerSetting = false;
            try
            {
                var existingRoot = GameObject.Find("[Settings]");
                if (existingRoot != null)
                    Object.DestroyImmediate(existingRoot);

                // Re-run bootstrapping deterministically (BeforeSceneLoad won't run again in this play session).
                SettingsBoot.RefreshForTests();

                // Allow Awake/OnEnable on newly added components to run.
                yield return null;

                var root = GameObject.Find("[Settings]");
                Assert.NotNull(root);

                Assert.NotNull(root.GetComponent<SettingsTestComponent>(), "Expected SettingsTestComponent to be attached directly to [Settings] when CreateChildGameObjectPerSetting is false.");

                var host = root.transform.Find(nameof(SettingsTestComponent));
                Assert.IsNull(host, "Did not expect a per-setting host when CreateChildGameObjectPerSetting is false.");
            }
            finally
            {
                SettingsBoot.Options.CreateChildGameObjectPerSetting = previous;
            }
        }
    }

#if UNITY_INCLUDE_TESTS
    public sealed class SettingsTestComponent : SettingsBase<int>
    {
        [Info, SerializeField]
        private string info =
            "A test settings component for unit testing SettingsBoot.";

        protected override int Value { get; set; }
        protected override string FileName => "__unityessentials_settingsboot_tests";
        protected override string Reference => "test.settings.component";
    }
#endif
}