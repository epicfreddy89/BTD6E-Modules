using AdditionalTiers.Tasks;
using AdditionalTiers.Utils;
using AdditionalTiers.Utils.Assets;
using AdditionalTiers.Utils.Towers;
using Assets.Scripts.Simulation.Towers;
using HarmonyLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using static HarmonyLib.AccessTools;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalTiers.AdditionalTiers), "Additional Tier Addon", "1.3", "1330 Studios LLC")]

namespace AdditionalTiers {
    public class AdditionalTiers : MelonMod {

        public static TowerTask[] towers;

        public override void OnApplicationStart() {
            var ttypes = Assembly.GetTypes().Where(a => typeof(TowerTask).IsAssignableFrom(a) && !typeof(TowerTask).FullName.Equals(a.FullName));
            List<TowerTask> tts = new();
            foreach (var type in ttypes) tts.Add((TowerTask)Activator.CreateInstance(type));
            towers = tts.ToArray();

            if (!MelonPreferences.HasEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier")) {
                MelonPreferences.CreateCategory("Additional Tier Addon Config", "Additional Tier Addon Config");
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier", (float)1);
            }

            Globals.Load();

            HarmonyInstance.Patch(Method(typeof(Tower), nameof(Tower.Hilight)), postfix: new HarmonyMethod(Method(typeof(HighlightManager), nameof(HighlightManager.Highlight))));
            HarmonyInstance.Patch(Method(typeof(Tower), nameof(Tower.UnHighlight)), postfix: new HarmonyMethod(Method(typeof(HighlightManager), nameof(HighlightManager.UnHighlight))));

            MelonLogger.Msg(ConsoleColor.Red, "Additional Tier Addon Loaded!");
            CacheBuilder.Build();
            Tasks.Assets.DisplayFactory.Build();
        }

        public override void OnApplicationQuit() {
            MelonLogger.Msg($"Last Win32 Error - {Marshal.GetLastWin32Error()}");
            Tasks.Assets.DisplayFactory.Flush();
            CacheBuilder.Flush();
        }
    }
}