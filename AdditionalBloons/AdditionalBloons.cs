using System;
using AdditionalBloons.Tasks;
using AdditionalBloons.Utils;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Track;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.BloonMenu;
using Harmony;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalBloons.AdditionalBloons), "Additional Bloon Addon", "1.0", "1330 Studios LLC")]

namespace AdditionalBloons {
    public class AdditionalBloons : MelonMod {
        public override void OnApplicationStart() {
            //Config

            Harmony.Patch(AccessTools.Method(typeof(GameModelLoader), nameof(GameModelLoader.Load)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonCreator), nameof(BloonCreator.GameLoad))));
            Harmony.Patch(AccessTools.Method(typeof(BloonMenu), nameof(BloonMenu.CreateBloonButtons)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonCreator), nameof(BloonCreator.BloonMenuCreate))));
            Harmony.Patch(AccessTools.Method(typeof(SpawnBloonButton), nameof(SpawnBloonButton.SpawnBloon)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonCreator), nameof(BloonCreator.SpawnBloon))));
            Harmony.Patch(AccessTools.Method(typeof(InGame), nameof(InGame.Update)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Run))));
            Harmony.Patch(AccessTools.Method(typeof(Bloon), nameof(Bloon.Damage)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Damage))));
            Harmony.Patch(AccessTools.Method(typeof(Spawner), nameof(Spawner.Emit)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Emit))));
            Harmony.Patch(AccessTools.Method(typeof(InGame), nameof(InGame.Quit)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Quit))));
            Harmony.Patch(AccessTools.Method(typeof(InGame), nameof(InGame.Restart)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Quit))));

            CacheBuilder.Build();

            MelonLogger.Msg(ConsoleColor.Red, "Additional Bloon Addon Loaded!");
        }
    }
}