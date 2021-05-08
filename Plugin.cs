using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using System.IO;


namespace TemplatePatch
{
    [BepInPlugin(UUID, "TemplatePatch", "1.4.0.0")]
    [BepInProcess("Shipbreaker.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private const string UUID = "com.github.hssb-racers.templatepatch";
        private static ManualLogSource _logSource;
        public static ConfigEntry<string> ConfigDataFolder { get; private set; }


        [UsedImplicitly]
        public void Awake()
        {
            _logSource = Logger;
            // this is really just a demo for a config option, it doesn't do anything valuable
            ConfigDataFolder = Config.Bind(
                "TemplatePatch",
                "DataFolder",
                Path.Combine(Paths.GameRootPath, "TemplatePatch"),
                "Just a demo folder that will be created when the Plugin starts up."
                );


            Log(LogLevel.Info, "TemplatePatch loaded.");

            if (!Directory.Exists(ConfigDataFolder.Value))
            {
                Log(LogLevel.Info, $"DataFolder {ConfigDataFolder.Value} doesn't appear to exist, attempting to create...");
                Directory.CreateDirectory(ConfigDataFolder.Value);
                Log(LogLevel.Info, $"Succeeded creating {ConfigDataFolder.Value}!");
            }

            var harmony = new Harmony(UUID);
            harmony.PatchAll();
            Log(LogLevel.Info, "TemplatePatch patched in!");

            foreach (var patchedMethod in harmony.GetPatchedMethods())
            {
                Log(
                    LogLevel.Info,
                    $"Patched: {patchedMethod.DeclaringType?.FullName}:{patchedMethod}");
            }
        }

        public static void Log(LogLevel level, string msg)
        {
            _logSource.Log(level, msg);
        }
    }
}


