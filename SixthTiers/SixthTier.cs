using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection.Emit;
using Assets.Scripts.Unity.UI_New.InGame;
using Harmony;
using MelonLoader;
using SixthTiers.Tasks;
using SixthTiers.Tasks.Towers;
using SixthTiers.Utils;

namespace SixthTiers {
    public class SixthTier : MelonMod {

        public static HarmonyInstance harmony { get; set; }

        static SixthTier (){}
        public static List<TowerTask> towers = new() {
            new WhiteAlbum(),
            new BigJuggus(),
            new Yellow_Submarine(),
            new BlackHoleSun(),
            new NinjaSexParty(),
            new PointOfNoReturn(),
            new UnderWorld(),
            new SkyHigh(),
            new Survivor()
        };
        public override void OnApplicationStart() {
            harmony = Harmony;
            MelonLogger.Msg(ConsoleColor.Red, "Sixth Tier Expansion Loaded!");
            CacheBuilder.Build();
        }

        public override void OnApplicationQuit() => CacheBuilder.Flush();
    }
}