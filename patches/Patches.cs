using BBI.Unity.Game;
using BepInEx.Logging;
using HarmonyLib;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Unity.Entities;

namespace TemplatePatch.Patches
{
    [HarmonyPatch]
    class TemplatePatch_Gamestate_Patch
    {
        [HarmonyTargetMethod]
        [UsedImplicitly]
        public static MethodBase TargetMethod()
        {
            return typeof(GameSession).GetMethod("Initialize", BindingFlags.Public | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool Postfix()
        {
            try
            {
                Plugin.Log(LogLevel.Info, "hello session");
                Main.EventSystem.AddHandler(new Carbon.Core.Events.EventHandler<GameStateChangedEvent>(GameStateChangedEventHandler));
            }
            catch (Exception e) { Plugin.Log(LogLevel.Error, e.ToString()); }
            return true;

        }

        public static void GameStateChangedEventHandler(GameStateChangedEvent ev)
        {
            try
            {
                Plugin.Log(LogLevel.Debug, $"received GameStateChangedEvent {{ GameState:{ev.GameState}, PrevGameState:{ev.PrevGameState} }}");
                if (ev.GameState == GameSession.GameState.Gameplay && (ev.PrevGameState == GameSession.GameState.Splash || ev.PrevGameState == GameSession.GameState.Loading))
                {
                    // looks like this is the start of a new shift!
                    Plugin.Log(LogLevel.Info, "looks like there's a new shift running");
                    if (GameSession.CurrentSessionType == GameSession.SessionType.WeeklyShip)
                    {
                        Plugin.Log(LogLevel.Debug, "looks like we're in RACE");
                    }
                }
                if (
                    ev.GameState == GameSession.GameState.GameComplete /* Shift Summary screen */ ||
                    ev.GameState == GameSession.GameState.None /* Esc -> Quit in RACE (maybe mark as abandoned specifically somewhere?) */
                    )
                {
                    Plugin.Log(LogLevel.Debug, "a shift just ended");
                }
            }
            catch (Exception e) { Plugin.Log(LogLevel.Error, e.ToString()); }
        }
    }

    [HarmonyPatch]
    class TemplatePatch_GameState_Patch_RemoveHooks
    {
        [HarmonyTargetMethod]
        [UsedImplicitly]
        public static MethodBase TargetMethod()
        {
            return typeof(GameSession).GetMethod("Shutdown", BindingFlags.Public | BindingFlags.Instance);
        }

        [HarmonyPrefix]
        [UsedImplicitly]
        public static bool Postfix()
        {
            Plugin.Log(LogLevel.Info, "goodbye session");
            Main.EventSystem.RemoveHandler(new Carbon.Core.Events.EventHandler<GameStateChangedEvent>(TemplatePatch_Gamestate_Patch.GameStateChangedEventHandler));

            return true;
        }
    }
}
