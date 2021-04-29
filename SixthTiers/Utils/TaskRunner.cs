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
using Harmony;
using MelonLoader;
using UnhollowerRuntimeLib;

namespace SixthTiers.Utils {
    public class TaskRunner {
        [HarmonyPatch(typeof(InGame), nameof(InGame.Update))]
        public static class Update {
            [HarmonyPostfix]
            public static void Postfix(ref InGame __instance) {
                if (__instance == null) {
                    for (var j = 0; j < SixthTier.towers.Count; j++)
                        SixthTier.towers[j].onLeave();
                    return;
                }
                if (__instance.bridge == null) return;

                var at = __instance.bridge.GetAllTowers();
                for (var i = 0; i < at.Count; i++)
                    for (var j = 0; j < SixthTier.towers.Count; j++)
                        if (SixthTier.towers[j].requirements(at[i]))
                            if (at[i].tower.namedMonkeyName != SixthTier.towers[j].identifier)
                                SixthTier.towers[j].onComplete(at[i]);
                            else if (at[i].tower.namedMonkeyName == SixthTier.towers[i].identifier)
                                SixthTier.towers[j].recurring(at[i]);
            }
        }
    }
}