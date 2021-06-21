using System;
using AdditionalBloons.Tasks;
using AdditionalBloons.Utils;
using Assets.Scripts.Models.Bloons;
using Assets.Scripts.Simulation.Bloons;
using Assets.Scripts.Simulation.Track;
using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.InGame.BloonMenu;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib;
using UnhollowerRuntimeLib.XrefScans;

[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
[assembly: MelonInfo(typeof(AdditionalBloons.AdditionalBloons), "Additional Bloon Addon", "1.2", "1330 Studios LLC")]

namespace AdditionalBloons {
    public class AdditionalBloons : MelonMod {
        public override void OnApplicationStart() {
            //Config

            HarmonyInstance.Patch(AccessTools.Method(typeof(GameModelLoader), nameof(GameModelLoader.Load)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonCreator), nameof(BloonCreator.GameLoad))));
            HarmonyInstance.Patch(AccessTools.Method(typeof(BloonMenu), nameof(BloonMenu.CreateBloonButtons)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonCreator), nameof(BloonCreator.BloonMenuCreate))));
            HarmonyInstance.Patch(AccessTools.Method(typeof(SpawnBloonButton), nameof(SpawnBloonButton.SpawnBloon)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonCreator), nameof(BloonCreator.SpawnBloon))));
            HarmonyInstance.Patch(AccessTools.Method(typeof(InGame), nameof(InGame.Update)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Run))));
            HarmonyInstance.Patch(AccessTools.Method(typeof(Bloon), nameof(Bloon.Damage)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Damage))));
            HarmonyInstance.Patch(AccessTools.Method(typeof(Spawner), nameof(Spawner.Emit)), prefix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Emit))));
            HarmonyInstance.Patch(AccessTools.Method(typeof(InGame), nameof(InGame.Quit)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Quit))));
            HarmonyInstance.Patch(AccessTools.Method(typeof(InGame), nameof(InGame.Restart)), postfix: new HarmonyMethod(AccessTools.Method(typeof(BloonTaskRunner), nameof(BloonTaskRunner.Quit))));

            CacheBuilder.Build();

            MelonLogger.Msg(ConsoleColor.Red, "Additional Bloon Addon Loaded!");
        }
    }
}