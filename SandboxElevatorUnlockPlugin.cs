using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SandboxElevatorUnlock
{
    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class SandboxElevatorUnlockPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "com.jrinker03.SandboxElevatorUnlock";
        private const string PluginName = "SandboxElevatorUnlock";
        private const string VersionString = "1.0.0";

        private static readonly Harmony Harmony = new Harmony(MyGUID);

        public static ManualLogSource Log = new ManualLogSource(PluginName);

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Apply all of our patches
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }
    }
}
