using Assets.Scripts.Unity.UI_New.InGame;
using Harmony;

namespace AdditionalTiers.Utils.Towers {
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
                        for (var indexOfAdditionalTiers = 0; indexOfAdditionalTiers < allAdditionalTiers.Count; indexOfAdditionalTiers++) {
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
                for (var towerIndex = AdditionalTiers.towers.Count - 1; towerIndex >= 0; towerIndex--)
                    AdditionalTiers.towers[towerIndex].onLeave();
            }
        }
    }
}