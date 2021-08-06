using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.Models.Map;
using Assets.Scripts.Models.Profile;
using Assets.Scripts.Models.Towers;
using Assets.Scripts.Models.Towers.Behaviors.Attack;
using Assets.Scripts.Simulation;
using Assets.Scripts.Unity;
using Assets.Scripts.Unity.Bridge;
using Assets.Scripts.Unity.Player;
using Assets.Scripts.Unity.UI_New.InGame;
using HarmonyLib;
using MelonLoader;
using UnhollowerRuntimeLib;

namespace AdditionalTiers.Utils {
    public class TaskRunner {
        [HarmonyPatch(typeof(InGame), nameof(InGame.Update))]
        private class Update {
            [HarmonyPostfix]
            public static void Postfix(ref InGame __instance) {
                

                if (__instance == null || __instance.bridge == null || __instance.bridge.GetAllTowers() == null) return;

                var allTowers = __instance.bridge.GetAllTowers();
                var allAdditionalTiers = AdditionalTiers.towers;
                for (var indexOfTowers = 0; indexOfTowers < allTowers.Count; indexOfTowers++) {
                    var towerToSimulation = allTowers.ToArray()[indexOfTowers];
                    if (towerToSimulation != null && !towerToSimulation.destroyed)
                        for (var indexOfAdditionalTiers = 0; indexOfAdditionalTiers < allAdditionalTiers.Length; indexOfAdditionalTiers++) {
                            if (!allAdditionalTiers[indexOfAdditionalTiers].requirements(towerToSimulation)) continue;
                            if (towerToSimulation.tower.namedMonkeyName != AdditionalTiers.towers[indexOfAdditionalTiers].identifier)
                                AdditionalTiers.towers[indexOfAdditionalTiers].onComplete(towerToSimulation);
                            else if (towerToSimulation.tower.namedMonkeyName == AdditionalTiers.towers[indexOfAdditionalTiers].identifier) 
                                AdditionalTiers.towers[indexOfAdditionalTiers].recurring(towerToSimulation);
                    }
                }
            }
        }

        [HarmonyPatch(typeof(InGame), nameof(InGame.Quit))]
        [HarmonyPatch(typeof(InGame), nameof(InGame.Restart))]
        private class Reset {
            [HarmonyPostfix]
            private static void RunLeave() {
                for (var towerIndex = AdditionalTiers.towers.Length - 1; towerIndex >= 0; towerIndex--)
                    AdditionalTiers.towers[towerIndex].onLeave();
            }
        }
    }
}