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

            EMU.Events.GameLoaded += Events_GameLoaded;

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");
            Log = Logger;
        }

        private void Events_GameLoaded()
        {
            // Sandbox mode is game mode preset 3
            if (FlowManager.instance.CurrentPreset.uniqueId == 3)
            {
                Logger.LogInfo($"Checking elevator unlock states.");

                ref ElevatorInstance elevatorInstance = ref ElevatorInstance.GetElevatorForFloor(0).Get();
                int numStrata = FlowManager.instance.curLevel.strataDefinitions.Length;

                if (elevatorInstance.curDigTier < numStrata - 1)
                {
                    Logger.LogInfo($"Unlocking strata.");
                    for (int i = elevatorInstance.curDigTier + 1; i <= numStrata; i++)
                    {
#if DEBUG
                        Logger.LogInfo($"Strata {i}.");
#endif
                        elevatorInstance.digPoints = elevatorInstance.targetDigPoints + 1;
                        elevatorInstance.UpdateDigTier(i, false);
                    }
                }
            }
        }
    }
}
