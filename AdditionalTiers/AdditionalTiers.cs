using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using Assets.Scripts.Unity.UI_New.InGame;
using Harmony;
using MelonLoader;
using AdditionalTiers.Tasks;
using AdditionalTiers.Tasks.Towers.Tier6s;
using AdditionalTiers.Utils;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalTiers.AdditionalTiers), "Additional Tier Addon", "1.1", "1330 Studios LLC")]

namespace AdditionalTiers {
    public class AdditionalTiers : MelonMod {
        public static HarmonyInstance harmony { get; set; }

        public static List<TowerTask> towers = new() {
            new WhiteAlbum(),
            new BigJuggus(),
            new Yellow_Submarine(),
            new BlackHoleSun(),
            new NinjaSexParty(),
            new PointOfNoReturn(),
            new UnderWorld(),
            new SkyHigh(),
            new Survivor(),
            new SuperFly()
        };

        public override void OnApplicationStart() {
            harmony = Harmony;
            if (!MelonPreferences.HasEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier")) {
                MelonPreferences.CreateCategory("Additional Tier Addon Config", "Additional Tier Addon Config");
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier", (float) 1);
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 damage multiplier", (float) 1);
            }
            
            Globals.Load();

            MelonLogger.Msg(ConsoleColor.Red, "Additional Tier Addon Loaded!");
            CacheBuilder.Build();
            Tasks.Assets.DisplayFactory.Build();
        }

        public override void OnApplicationQuit() => CacheBuilder.Flush();
    }
}