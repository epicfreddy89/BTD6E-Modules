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
using AdditionalTiers.Utils.Assets;
using AdditionalTiers.Utils.Components;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Utils;
using Il2CppSystem.Collections;
using UnhollowerRuntimeLib;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalTiers.AdditionalTiers), "Additional Tier Addon", "1.2", "1330 Studios LLC")]

namespace AdditionalTiers {
    public class AdditionalTiers : MelonMod {

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
            new SuperFly(),
            new Whitesnake()
        };

        public override void OnApplicationStart() {
            if (!MelonPreferences.HasEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier")) {
                MelonPreferences.CreateCategory("Additional Tier Addon Config", "Additional Tier Addon Config");
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 required pop count multiplier", (float) 1);
                MelonPreferences.CreateEntry("Additional Tier Addon Config", "Tier 6 damage multiplier", (float) 1);
            }
            
            Globals.Load();
            ClassInjector.RegisterTypeInIl2Cpp<AnimatedEnergyTexture>();
            ClassInjector.RegisterTypeInIl2Cpp<AnimatedFlameTexture>();
            ClassInjector.RegisterTypeInIl2Cpp<AnimatedDarkFlameTexture>();
            
            MelonLogger.Msg(ConsoleColor.Red, "Additional Tier Addon Loaded!");
            CacheBuilder.Build();
            Tasks.Assets.DisplayFactory.Build();
        }

        public override void OnApplicationQuit() {
            Tasks.Assets.DisplayFactory.Flush();
            CacheBuilder.Flush();
        }

        public override void OnGUI() {
            
        }
    }
}