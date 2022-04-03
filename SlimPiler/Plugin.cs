using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace DysonSphereProgram.Modding.SlimPiler
{
  [BepInPlugin(GUID, NAME, VERSION)]
  [BepInProcess("DSPGAME.exe")]
  public class Plugin : BaseUnityPlugin
  {
    public const string GUID = "dev.raptor.dsp.SlimPiler";
    public const string NAME = "SlimPiler";
    public const string VERSION = "0.0.1";

    private Harmony _harmony;
    internal static ManualLogSource Log;

    private void Awake()
    {
      Plugin.Log = Logger;
      _harmony = new Harmony(GUID);
      _harmony.PatchAll(typeof(PilerPatch));
      Logger.LogInfo("SlimPiler Awake() called");
    }

    private void OnDestroy()
    {
      Logger.LogInfo("SlimPiler OnDestroy() called");
      _harmony?.UnpatchSelf();
      Plugin.Log = null;
    }
  }
}
