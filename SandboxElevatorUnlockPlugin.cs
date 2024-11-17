using BepInEx;
using BepInEx.Logging;
using EquinoxsModUtils;
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

            EMU.Events.SaveStateLoaded += Events_SaveStateLoaded;

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        private void Events_SaveStateLoaded(object sender, System.EventArgs e)
        {
            // Sandbox mode is game mode preset 3
            if (FlowManager.instance.CurrentPreset.uniqueId == 3)
            {
                ref ElevatorInstance elevatorInstance = ref ElevatorInstance.GetElevatorForFloor(0).Get();
                int numStrata = FlowManager.instance.curLevel.strataDefinitions.Length;

                if (elevatorInstance.curDigTier < numStrata - 1)
                {
                    for (int i = elevatorInstance.curDigTier + 1; i < numStrata; i++)
                    {
                        Logger.LogInfo($"Unlocking strata {i}.");
                        elevatorInstance.UpdateDigTier(i, false);
                        elevatorInstance.digPoints = elevatorInstance.targetDigPoints;
                    }
                }
            }
        }
    }
}
