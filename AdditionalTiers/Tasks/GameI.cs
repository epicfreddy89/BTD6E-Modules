using Assets.Scripts.Models;
using HarmonyLib;

namespace AdditionalTiers.Tasks {
    public class GameI {
        [HarmonyPatch(typeof(GameModelLoader), nameof(GameModelLoader.Load))]
        public static class Loaded {
            [HarmonyPostfix]
            public static void Postfix(ref GameModel __result) {
                for (var i = 0; i < AdditionalTiers.towers.Length; i++) AdditionalTiers.towers[i].gameLoad(__result);
            }
        }
    }
}