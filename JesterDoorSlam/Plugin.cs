using BepInEx;
using HarmonyLib;

namespace JesterDoorSlam;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
    public static Plugin Instance;
    
    private void Awake()
    {
        Instance ??= this;

        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        harmony.PatchAll(typeof(JesterPatch));
    }
}

[HarmonyPatch(typeof(JesterAI))]
public class JesterPatch
{
    [HarmonyPatch(nameof(JesterAI.Update))]
    [HarmonyPostfix]
    private static void SlamOpenDoors(ref JesterAI __instance)
    {
        if (__instance.currentBehaviourStateIndex == 2 && __instance.agent.speed >= 10f)
        {
            __instance.useSecondaryAudiosOnAnimatedObjects = true;
        }
        else
        {
            __instance.useSecondaryAudiosOnAnimatedObjects = false;
        }
    }
}