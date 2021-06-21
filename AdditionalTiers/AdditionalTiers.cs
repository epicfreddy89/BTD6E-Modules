using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using Assets.Scripts.Unity.UI_New.InGame;
using HarmonyLib;
using MelonLoader;
using AdditionalTiers.Tasks;
using AdditionalTiers.Tasks.Towers.Tier6s;
using AdditionalTiers.Utils;
using AdditionalTiers.Utils.Assets;
using AdditionalTiers.Utils.Components;
using AdditionalTiers.Utils.Towers;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Simulation.Towers;
using Assets.Scripts.Utils;
using Il2CppSystem.Collections;
using UnhollowerRuntimeLib;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static HarmonyLib.AccessTools;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalTiers.AdditionalTiers), "Additional Tier Addon", "1.2", "1330 Studios LLC")]

namespace AdditionalTiers {
    public class AdditionalTiers : MelonMod {

        public static TowerTask[] towers = new TowerTask[] {
            new WhiteAlbum(),
            new BigJuggus(),
            new Yellow_Submarine(),
            new BlackHoleSun(),
            new NinjaSexParty(),
            new PointOfNoReturn(),
            new UnderWorld(),
            new SkyHigh(),
            new Survivor(),
            new SuperFly()/*,
            new Whitesnake()*/
        };

        public override void OnApplicationStart() {            
            if (!MelonPreferences.HasEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier")) {
                MelonPreferences.CreateCategory("Additional Tier Addon Config", "Additional Tier Addon Config");
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier", (float) 1);
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 damage multiplier", (float) 1);
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